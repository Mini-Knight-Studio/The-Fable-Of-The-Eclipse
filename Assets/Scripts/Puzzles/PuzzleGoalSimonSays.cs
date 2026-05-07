using System;
using Loopie;
using System.Collections.Generic;
using System.Collections;

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

    private List<int> sequence = new List<int>();
    private List<int> fullSequence = new List<int>();
    private int playerIndex = 0;
    private int round = 0;
    public int maxRounds = 4;
    private int successfulRounds = 0;

    public float showDelay = 1.0f;
    private bool simonStarted = false;

    public Entity Gem;

    // Particles
    private ParticleComponent goalParticles;

    // Sounds
    private AudioSource moveSFX;
    public Entity activateSFX;
    public Entity completeSFX;
    public Entity collectGemSFX;
    public Entity failSFX;

    private enum State
    {
        WaitingForPillars,
        PlayingSimon,
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
        if (Pause.isPaused) { return; }

        if (DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed && !puzzle2Completed)
        {
            CompletePuzzleAuto();
        }

        switch (currentState)
        {
            case State.WaitingForPillars:
                CheckBasePuzzle();
                break;

            case State.PlayingSimon:
                break;

            case State.Completed:
                HandleCompleted();
                break;
        }
    }

    IEnumerator MoveRoutine(Vector3 relativeMove)
    {
        Vector3 startPosition = entity.transform.position;
        Vector3 targetPosition = startPosition + relativeMove;
        float distance = (float)(targetPosition - startPosition).magnitude;

        if (distance > 0.0001f)
        {
            float moveDuration = distance / movementSpeed;
            float moveTimer = 0.0f;

            goalParticles.Play();
            moveSFX.Play();

            while (moveTimer < moveDuration)
            {
                moveTimer += Time.deltaTime;
                float t = moveTimer / moveDuration;
                entity.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            entity.transform.position = targetPosition;
            goalParticles.Stop();
        }
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
                    if (simonPillars[i] != null) simonPillars[i].Lock();
                }

                if (simonPillars[i] != null) simonPillars[i].ForceActive();
            }
        }

        if (allOnGoal && !simonStarted)
        {
            Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(true);
            if (goalCollider != null && goalCollider.IsColliding && Player.Instance.Input.interactKeyPressed)
            {
                simonStarted = true;
                activateSFX.GetComponent<AudioSource>().Play();
                Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(false);
                StartCoroutine(SimonSaysRoutine());
            }
        }
    }

    IEnumerator SimonSaysRoutine()
    {
        currentState = State.PlayingSimon;
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
        successfulRounds = 0;

        while (round < maxRounds)
        {
            round++;
            sequence.Add(fullSequence[round - 1]);

            LockAllPillars();
            ResetAllPillars();
            yield return new WaitForSeconds(showDelay);

            foreach (int index in sequence)
            {
                if (simonPillars[index] != null) simonPillars[index].ForceActive();
                yield return new WaitForSeconds(showDelay);
                ResetAllPillars();
                yield return new WaitForSeconds(showDelay * 0.5f);
            }

            playerIndex = 0;
            for (int i = 0; i < pillarPressedThisRound.Length; i++) pillarPressedThisRound[i] = false;
            UnlockAllPillars();

            bool roundFailed = false;
            while (playerIndex < sequence.Count)
            {
                int pressedIndex = -1;
                for (int i = 0; i < simonPillars.Length; i++)
                {
                    if (simonPillars[i] != null && simonPillars[i].wasPressed && !pillarPressedThisRound[i])
                    {
                        pressedIndex = i;
                        break;
                    }
                }

                if (pressedIndex != -1)
                {
                    pillarPressedThisRound[pressedIndex] = true;
                    if (sequence[playerIndex] == pressedIndex)
                    {
                        playerIndex++;
                    }
                    else
                    {
                        roundFailed = true;
                        break;
                    }
                }
                yield return null;
            }

            if (roundFailed)
            {
                failSFX.GetComponent<AudioSource>().Play();
                yield return StartCoroutine(MoveRoutine(new Vector3(0, movementDistance * successfulRounds, 0)));

                simonStarted = false;
                currentState = State.WaitingForPillars;
                yield break;
            }
            else
            {
                successfulRounds++;
                yield return StartCoroutine(MoveRoutine(new Vector3(0, -movementDistance, 0)));
                yield return new WaitForSeconds(1.0f);
            }
        }

        CompletePuzzle();
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
        currentState = State.Completed;
        Gem.GetComponent<BoxCollider>().SetActive(true);
        Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(true);
        completeSFX.GetComponent<AudioSource>().Play();
    }

    void CompletePuzzleAuto()
    {
        if (puzzle2Completed) return;
        puzzle2Completed = true;
        DatabaseRegistry.puzzlesDB.Puzzles.Puzzle2Completed = true;

        Gem.SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);
        Gem.GetComponent<BoxCollider>().SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);
        Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);

        ResetAllPillars();
        foreach (var pillar in basePillars) if (pillar != null) pillar.CompletePillarAuto();
        foreach (var pillar in simonPillars) if (pillar != null) { pillar.Lock(); pillar.ForceActive(); }

        entity.transform.position += new Vector3(0, -movementDistance * maxRounds, 0);
        currentState = State.Completed;
    }

    void LockAllPillars()
    {
        foreach (var pillar in simonPillars) if (pillar != null) pillar.Lock();
    }

    void UnlockAllPillars()
    {
        foreach (var pillar in simonPillars) if (pillar != null) pillar.Unlock();
    }

    void ResetAllPillars()
    {
        foreach (var pillar in simonPillars) if (pillar != null) pillar.ResetState();
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}