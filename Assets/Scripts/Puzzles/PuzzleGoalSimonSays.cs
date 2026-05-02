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

    public Entity Gem;

    // Particles
    private ParticleComponent goalParticles;
    private bool particlesSwitched = true;

    // Sounds
    private AudioSource moveSFX;
    public Entity activateSFX;
    public Entity completeSFX;
    public Entity collectGemSFX;
    public Entity failSFX;

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

        moveSFX = entity.GetComponent<AudioSource>();

        goalParticles = entity.GetComponent<ParticleComponent>();
        goalParticles.Stop();

        LockAllPillars();

        Gem.GetComponent<BoxCollider>().SetActive(false);
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
                HandleCompleted();
                break;
        }

        if (!isMoving && !particlesSwitched)
        {
            goalParticles.Stop();
            particlesSwitched = true;
        }

        if (DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed && !puzzle2Completed)
        {
            CompletePuzzleAuto();
        }
    }

    void StartMovement(Vector3 newTarget)
    {
        startPosition = entity.transform.position;
        targetPosition = newTarget;

        Vector3 diff = targetPosition - startPosition;
        float distance = (float)diff.magnitude;

        if (distance <= 0.0001f)
        {
            isMoving = false;
            return;
        }

        moveDuration = distance / movementSpeed;
        moveTimer = 0.0f;

        isMoving = true;

        goalParticles.Play();
        moveSFX.Play();
        particlesSwitched = false;
    }

    void MoveTowardsTarget()
    {
        if (moveDuration <= 0.0001f)
        {
            entity.transform.position = targetPosition;
            isMoving = false;
            return;
        }

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
                        simonPillars[i].Lock();
                }

                if (simonPillars[i] != null)
                    simonPillars[i].ForceActive();
            }
        }

        if (allOnGoal && !simonStarted)
        {
            Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(true);
            if (goalCollider != null && goalCollider.IsColliding && Player.Instance.Input.interactKeyPressed)
            {
                simonStarted = true;
                Debug.LogWarning("Starting Simon Says.");
                activateSFX.GetComponent<AudioSource>().Play();
                Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(false);
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
            int r = Loopie.Random.Range(i, fullSequence.Count);
            fullSequence[i] = fullSequence[r];
            fullSequence[r] = temp;
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
                    simonPillars[index].ForceActive();

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

        for (int i = 0; i < pillarPressedThisRound.Length; i++)
            pillarPressedThisRound[i] = false;

        ResetAllPillars();
        UnlockAllPillars();
        currentState = State.PlayerInput;
    }

    void UpdatePlayerInput()
    {
        for (int i = 0; i < simonPillars.Length; i++)
        {
            var pillar = simonPillars[i];
            if (pillar == null) continue;

            if (pillar.wasPressed && !pillarPressedThisRound[i])
            {
                Debug.Log($"Player input: {i}");

                pillarPressedThisRound[i] = true;

                CheckPlayerInput(i);
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
                timer = 0.0f;
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

        Debug.Log("Enemies theoretically spawned");

        LockAllPillars();
        currentState = State.WaitingForPillars;
        simonStarted = false;
        failSFX.GetComponent<AudioSource>().Play();
    }

    void HandleCompleted()
    {
        foreach (var pillar in simonPillars)
        {
            if (pillar != null) pillar.ForceActive();
        }

        if (Gem.GetComponent<BoxCollider>().IsColliding && Player.Instance.Input.interactKeyPressed)
        {
            Gem.SetActive(false);

            DatabaseRegistry.playerDB.Player.gemWaterCollected = true;
            DatabaseRegistry.playerDB.Player.hasGrappling = true;

            collectGemSFX.GetComponent<AudioSource>().Play();
        }
    }

    void CompletePuzzle()
    {
        if (puzzle2Completed) return;

        puzzle2Completed = true;

        Debug.Log("Puzzle Fully Completed!");

        currentState = State.Completed;
        Gem.GetComponent<BoxCollider>().SetActive(true);
        Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(true);

        completeSFX.GetComponent<AudioSource>().Play();
    }

    void CompletePuzzleAuto()
    {
        if (puzzle2Completed) return;

        puzzle2Completed = true;

        Debug.Log("Puzzle Fully Completed!");

        DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed = true;

        Gem.SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);
        Gem.GetComponent<BoxCollider>().SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);
        Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);

        ResetAllPillars();

        foreach (var pillar in basePillars)
        {
            if (pillar != null) pillar.CompletePillarAuto();
        }

        foreach (var pillar in simonPillars)
        {
            if (pillar != null) pillar.Lock();
        }

        foreach (var pillar in simonPillars)
        {
            if (pillar != null) pillar.ForceActive();
        }

        successfulRounds = maxRounds;
        pendingMoves = maxRounds;

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
            if (pillar != null) pillar.ResetState();
    }
}