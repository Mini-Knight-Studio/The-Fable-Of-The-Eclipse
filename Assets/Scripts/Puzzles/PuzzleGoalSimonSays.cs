using System;
using Loopie;
using System.Collections.Generic;

class PuzzleGoalSimonSays : Component
{
    public Entity Pillar1;
    public Entity Pillar2;
    public Entity Pillar3;
    public Entity Pillar4;

    private MovingPillar[] basePillars;
    private MovingPillarSimonSays[] simonPillars;
    private bool[] pillarTriggered;
    private bool[] pillarPressedThisRound;

    private bool puzzle2Completed = false;

    private BoxCollider goalCollider;

    public float movementSpeed = 2.0f;
    public float movementDistance = 1.0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;
    private bool isMoving = false;
    private int pendingMoves = 0;
    private int pendingUpMoves = 0;

    private List<int> sequence = new List<int>();
    private List<int> fullSequence = new List<int>();
    private int playerIndex = 0;
    private int round = 0;
    public int maxRounds = 4;
    private int successfulRounds = 0;

    private float timer = 0.0f;
    public float showDelay = 1.0f;
    private int showIndex = 0;

    private bool simonStarted = false;
    private float inputCooldown = 0.0f;

    private enum State
    {
        WaitingForPillars,
        ShowingSequence,
        PlayerInput,
        Success,
        Fail,
        Completed
    }

    private State currentState = State.WaitingForPillars;

    void OnCreate()
    {
        basePillars = new MovingPillar[4];
        simonPillars = new MovingPillarSimonSays[4];
        pillarTriggered = new bool[4];
        pillarPressedThisRound = new bool[4];

        goalCollider = entity.GetComponent<BoxCollider>();

        Entity[] pillarEntities = { Pillar1, Pillar2, Pillar3, Pillar4 };

        for (int i = 0; i < 4; i++)
        {
            if (pillarEntities[i] != null)
            {
                basePillars[i] = pillarEntities[i].GetComponent<MovingPillar>();
                simonPillars[i] = pillarEntities[i].GetComponent<MovingPillarSimonSays>();
            }
        }

        LockAllPillars();
    }

    void OnUpdate()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
        else if (pendingMoves > 0)
        {
            pendingMoves--;
            StartMovement(entity.transform.position + new Vector3(0, -movementDistance, 0));
        }
        else if (pendingUpMoves > 0)
        {
            pendingUpMoves--;
            StartMovement(entity.transform.position + new Vector3(0, movementDistance, 0));
        }

        switch (currentState)
        {
            case State.WaitingForPillars:
                CheckBasePuzzle();
                break;

            case State.ShowingSequence:
                UpdateSequencePlayback();
                break;

            case State.PlayerInput:
                UpdatePlayerInput();
                break;

            case State.Success:
                HandleSuccess();
                break;

            case State.Fail:
                HandleFail();
                break;

            case State.Completed:
                break;
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

    void CheckBasePuzzle()
    {
        bool allOnGoal = true;

        for (int i = 0; i < basePillars.Length; i++)
        {
            if (basePillars[i] == null) continue;

            if (!basePillars[i].onGoalPosition)
            {
                allOnGoal = false;
                pillarTriggered[i] = false;
            }
            else 
            {
                if (!pillarTriggered[i])
                {
                    pillarTriggered[i] = true;

                    if (simonPillars[i] != null)
                    {
                        simonPillars[i].Lock();
                    }
                }

                if (simonPillars[i] != null)
                {
                    simonPillars[i].ForceActive();
                }
            }
        }

        if (allOnGoal && !simonStarted)
        {
            if (goalCollider != null && goalCollider.IsColliding && Input.IsKeyPressed(KeyCode.E))
            {
                simonStarted = true;
                Debug.LogWarning("Starting Simon Says.");
                StartSimonPhase();
            }
        }
    }

    void StartSimonPhase()
    {
        ResetAllPillars();
        sequence.Clear();

        fullSequence = new List<int> { 0, 1, 2, 3 };
        for (int i = 0; i < fullSequence.Count; i++)
        {
            int temp = fullSequence[i];
            int randomIndex = Loopie.Random.Range(i, fullSequence.Count);
            fullSequence[i] = fullSequence[randomIndex];
            fullSequence[randomIndex] = temp;
        }

        round = 0;
        NextRound();
    }

    void NextRound()
    {
        round++;
        if (round > maxRounds)
        {
            CompletePuzzle();
            return;
        }

        Debug.LogWarning($"Starting Round: {round}");

        sequence.Add(fullSequence[round - 1]);

        LockAllPillars();
        ResetAllPillars();

        showIndex = 0;
        timer = 0.0f;
        currentState = State.ShowingSequence;
    }

    void UpdateSequencePlayback()
    {
        timer += Time.deltaTime;

        if (timer >= showDelay)
        {
            timer = 0.0f;
            ResetAllPillars();

            if (showIndex < sequence.Count)
            {
                int index = sequence[showIndex];
                Debug.Log($"Playback: Showing Pillar {index}");

                if (simonPillars[index] != null)
                {
                    simonPillars[index].ForceActive();
                }

                showIndex++;
            }
            else
            {
                PreparePlayerPhase();
            }
        }
    }

    void PreparePlayerPhase()
    {
        playerIndex = 0;
        inputCooldown = 0.5f;

        for (int i = 0; i < pillarPressedThisRound.Length; i++)
        {
            pillarPressedThisRound[i] = false;
        }

        ResetAllPillars();
        UnlockAllPillars();
        currentState = State.PlayerInput;
    }

    void UpdatePlayerInput()
    {
        if (inputCooldown > 0)
        {
            inputCooldown -= Time.deltaTime;
            return;
        }

        for (int i = 0; i < simonPillars.Length; i++)
        {
            if (simonPillars[i] != null && simonPillars[i].active && !pillarPressedThisRound[i])
            {
                Debug.Log($"Player input: {i}");

                pillarPressedThisRound[i] = true;
                simonPillars[i].ForceActive();

                CheckPlayerInput(i);
                inputCooldown = 0.4f;
                break;
            }
        }
    }

    void CheckPlayerInput(int index)
    {
        if (sequence[playerIndex] == index)
        {
            playerIndex++;
            if (playerIndex >= sequence.Count)
            {
                pendingMoves++;
                successfulRounds++;
                currentState = State.Success;
            }
        }
        else
        {
            currentState = State.Fail;
        }
    }

    void HandleSuccess()
    {
        LockAllPillars();
        timer += Time.deltaTime;

        if (timer > 1.0f)
        {
            timer = 0;
            NextRound();
        }
    }

    void HandleFail()
    {
        Debug.LogWarning("Failed! Restarting Simon Phase.");

        pendingUpMoves += successfulRounds;
        successfulRounds = 0;

        // Spawn enemies here

        LockAllPillars();
        StartSimonPhase();
    }

    void CompletePuzzle()
    {
        if (puzzle2Completed) return;

        puzzle2Completed = true;

        Debug.Log("Puzzle Fully Completed!");
        GlobalDatabase.Data.Puzzles.Puzzle2Completed = true;

        currentState = State.Completed;
    }

    void LockAllPillars()
    {
        foreach (var pillar in simonPillars)
            if (pillar != null) pillar.Lock();
    }

    void UnlockAllPillars()
    {
        foreach (var pillar in simonPillars)
            if (pillar != null) pillar.Unlock();
    }

    void ResetAllPillars()
    {
        foreach (var pillar in simonPillars)
            if (pillar != null) pillar.active = false;
    }

    void CompletePillarsAuto()
    {
        Pillar1.GetComponent<MovingPillar>().CompletePillarAuto();
        Pillar2.GetComponent<MovingPillar>().CompletePillarAuto();
        Pillar3.GetComponent<MovingPillar>().CompletePillarAuto();
        Pillar4.GetComponent<MovingPillar>().CompletePillarAuto();
    }
}