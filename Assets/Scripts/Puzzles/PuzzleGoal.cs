using System;
using Loopie;

class PuzzleGoal : Component
{
    private MovingPillar[] pillars;
    private bool[] pillarTriggered;

    public Entity Pillar1;
    public Entity Pillar2;
    public Entity Pillar3;
    public Entity Pillar4;

    public float movementSpeed = 2.0f;
    public float movementDistance = 1.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;

    private bool isMoving = false;
    private int pendingMoves = 0;

    private bool puzzle1Completed;

    void OnCreate()
    {
        pillars = new MovingPillar[4];
        pillarTriggered = new bool[4];

        pillars[0] = Pillar1.GetComponent<MovingPillar>();
        pillars[1] = Pillar2.GetComponent<MovingPillar>();
        pillars[2] = Pillar3.GetComponent<MovingPillar>();
        pillars[3] = Pillar4.GetComponent<MovingPillar>();

        if (GlobalDatabase.Data.Exists())
        {
            GlobalDatabase.Data.Load();
        }
    }

    void OnUpdate()
    {
        CheckPillars();

        if (isMoving)
        {
            MoveTowardsTarget();
        }
        else if (pendingMoves > 0)
        {
            pendingMoves--;
            StartMovement(entity.transform.position + new Vector3(0, -movementDistance, 0));
        }
    }

    void CheckPillars()
    {
        bool allOnGoal = true;

        for (int i = 0; i < pillars.Length; i++)
        {
            if (pillars[i] == null) continue;

            if (pillars[i].onGoalPosition && !pillarTriggered[i])
            {
                pillarTriggered[i] = true;
                pendingMoves++;
            }

            if (!pillars[i].onGoalPosition)
            {
                allOnGoal = false;
            }
        }

        // Commented so that the puzzle is not completed automatically after completing it once (for other devs)
        //
        //if (GlobalDatabase.Data.Puzzles.Puzzle1Completed && !puzzle1Completed)
        //{
        //    puzzle1Completed = true;

        //    if (GlobalDatabase.Data.Puzzles.Puzzle1Completed)
        //    {
        //        CompletePuzzleAuto();
        //    }
        //}

        if (allOnGoal && !puzzle1Completed)
        {
            puzzle1Completed = true;

            GlobalDatabase.Data.Puzzles.Puzzle1Completed = true;
            GlobalDatabase.Data.Save();
        }
    }

    void StartMovement(Vector3 newTarget)
    {
        startPosition = entity.transform.position;
        targetPosition = newTarget;

        Vector3 difference = targetPosition - startPosition;
        float distance = (float)difference.magnitude;

        moveDuration = distance / movementSpeed;
        moveTimer = 0.0f;

        isMoving = true;
    }

    void MoveTowardsTarget()
    {
        moveTimer += Time.deltaTime;

        float t = moveTimer / moveDuration;

        if (t >= 1.0f)
        {
            entity.transform.position = targetPosition;
            isMoving = false;
            return;
        }

        entity.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    }

    void CompletePuzzleAuto()
    {
        Pillar1.GetComponent<MovingPillar>().CompletePillarAuto();
        Pillar2.GetComponent<MovingPillar>().CompletePillarAuto();
        Pillar3.GetComponent<MovingPillar>().CompletePillarAuto();
        Pillar4.GetComponent<MovingPillar>().CompletePillarAuto();
    }
};