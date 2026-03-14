using System;
using Loopie;

class PuzzleGoal : Component
{
    public string Pillar1Name;
    public string Pillar2Name;
    public string Pillar3Name;
    public string Pillar4Name;

    private MovingPillar[] pillars;
    private bool[] pillarTriggered;

    private Entity Pillar1;
    private Entity Pillar2;
    private Entity Pillar3;
    private Entity Pillar4;

    public float movementSpeed = 2.0f;
    public float movementDistance = 1.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;

    private bool isMoving = false;
    private int pendingMoves = 0;

    void OnCreate()
    {
        pillars = new MovingPillar[4];
        pillarTriggered = new bool[4];

        pillars[0] = Entity.FindEntityByName(Pillar1Name)?.GetComponent<MovingPillar>();
        pillars[1] = Entity.FindEntityByName(Pillar2Name)?.GetComponent<MovingPillar>();
        pillars[2] = Entity.FindEntityByName(Pillar3Name)?.GetComponent<MovingPillar>();
        pillars[3] = Entity.FindEntityByName(Pillar4Name)?.GetComponent<MovingPillar>();
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
        for (int i = 0; i < pillars.Length; i++)
        {
            if (pillars[i] == null) continue;

            if (pillars[i].onGoalPosition && !pillarTriggered[i])
            {
                pillarTriggered[i] = true;
                pendingMoves++;
            }
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
};