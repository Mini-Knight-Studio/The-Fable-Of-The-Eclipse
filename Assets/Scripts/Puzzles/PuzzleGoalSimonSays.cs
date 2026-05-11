using System;
using Loopie;
using System.Collections.Generic;
using System.Collections;
using System.Media;

class PuzzleGoalSimonSays : Component
{
    [Header("References")]
    public Entity Pillar1;
    public Entity Pillar2;
    public Entity Pillar3;
    public Entity Pillar4;

    private MovingPillar[] basePillars;
    private MovingPillarSimonSays[] simonPillars;
    private bool[] pillarTriggered;

    private bool puzzle2Completed = false;
    private bool isCollecting = false;

    private BoxCollider goalCollider;

    [Header("Settings")]
    public float movementSpeed = 2.0f;
    public float movementDistance = 1.0f;

    private List<int> sequence = new List<int>();
    private List<int> fullSequence = new List<int>();
    private int playerIndex = 0;
    private int round = 0;
    public int maxRounds = 4;
    private int successfulRounds = 0;

    private float cameraShakeDuration = 1f;
    private float cameraShakeAmount = 0.5f;
    private float cameraShakeRotation = 0.5f;
    private float cameraShakeAmountVel = 10f;
    private float cameraShakeRotationVel = 10f;

    public float collectTime = 1f;

    [Header("Timing Settings")]
    public float playbackActiveTime = 0.8f;
    public float playbackInactiveTime = 0.2f;
    public float roundStartDelay = 1.0f;

    private bool simonStarted = false;

    public Entity Gem;

    [Header("Feedback")]
    // Particles
    private ParticleComponent goalParticles;

    // Sounds
    private AudioSource moveSFX;
    public Entity activateSFX;
    public Entity completeSFX;
    public Entity collectGemSFX;
    public Entity failSFX;
    public Entity roundSuccessSFX;

    public string popupName = "Popup_GemWater";

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
        HidePromptAllPillars();

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
            Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount, cameraShakeRotation, cameraShakeAmountVel, cameraShakeRotationVel);

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

            if (!basePillars[i].stopedOnGoal)
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
        fullSequence.Clear();

        int lastIndex = -1;
        for (int i = 0; i < maxRounds; i++)
        {
            int nextIndex;
            do
            {
                nextIndex = Loopie.Random.Range(0, 4);
            } while (nextIndex == lastIndex);

            fullSequence.Add(nextIndex);
            lastIndex = nextIndex;
        }

        round = 0;
        successfulRounds = 0;

        while (round < maxRounds)
        {
            round++;
            sequence.Add(fullSequence[round - 1]);

            LockAllPillars();
            ResetAllPillars();

            yield return new WaitForSeconds(roundStartDelay);

            foreach (int index in sequence)
            {
                if (simonPillars[index] != null) simonPillars[index].ForceActive();
                HidePromptAllPillars();
                yield return new WaitForSeconds(playbackActiveTime);

                ResetAllPillars();
                yield return new WaitForSeconds(playbackInactiveTime);
            }

            playerIndex = 0;
            UnlockAllPillars();
            ShowPromptAllPillars();

            bool roundFailed = false;
            while (playerIndex < sequence.Count)
            {
                int pressedIndex = -1;
                for (int i = 0; i < simonPillars.Length; i++)
                {
                    if (simonPillars[i] != null && simonPillars[i].wasPressed)
                    {
                        pressedIndex = i;
                        break;
                    }
                }

                if (pressedIndex != -1)
                {
                    if (sequence[playerIndex] == pressedIndex)
                    {
                        playerIndex++;
                        simonPillars[pressedIndex].ResetState();
                        simonPillars[pressedIndex].interactPrompt.SetActive(false);
                        
                        if (playerIndex >= sequence.Count) HidePromptAllPillars();

                        yield return new WaitForSeconds(1f);

                        if (playerIndex < sequence.Count) simonPillars[pressedIndex].interactPrompt.SetActive(true);
                        simonPillars[pressedIndex].ResetState();
                    }
                    else
                    {
                        simonPillars[pressedIndex].ResetState();
                        roundFailed = true;
                        break;
                    }
                }
                yield return null;
            }

            if (roundFailed)
            {
                failSFX.GetComponent<AudioSource>().Play();
                HidePromptAllPillars();
                yield return StartCoroutine(MoveRoutine(new Vector3(0, movementDistance * successfulRounds, 0)));

                simonStarted = false;
                currentState = State.WaitingForPillars;
                yield break;
            }
            else
            {
                successfulRounds++;
                yield return StartCoroutine(MoveRoutine(new Vector3(0, -movementDistance, 0)));
                roundSuccessSFX.GetComponent<AudioSource>().Play();
                HidePromptAllPillars();
                yield return new WaitForSeconds(roundStartDelay);
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

        if (!isCollecting && Gem.GetComponent<BoxCollider>().IsColliding && Player.Instance.Input.interactKeyPressed)
        {
            StartCoroutine(Collect());
        }
    }

    IEnumerator Collect()
    {
        isCollecting = true;

        Gem.GetComponent<BoxCollider>().SetActive(false);
        Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(false);
        Gem.GetComponent<Gem_Idle>().SetActive(false);

        Entity player = Player.Instance.entity;
        Vector3 initialPosition = Gem.transform.position;
        Vector3 initialScale = Gem.transform.scale;
        float timer = 0;

        while (timer < collectTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / collectTime);

            Gem.transform.position = Vector3.Lerp(initialPosition, player.transform.position + new Vector3(0, 2, 0), t);
            Gem.transform.scale = Vector3.Lerp(initialScale, Vector3.Zero, t);

            yield return null;
        }

        Gem.SetActive(false);

        DatabaseRegistry.playerDB.Player.gemWaterCollected = true;
        DatabaseRegistry.playerDB.Player.hasGrappling = true;

        collectGemSFX.GetComponent<AudioSource>().Play();

        if (UIPopupManager.Instance != null)
        {
            UIPopupManager.Instance.ShowPopup(popupName);
        }

        isCollecting = false;
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

        if (!isCollecting)
        {
            Gem.SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);
            Gem.GetComponent<BoxCollider>().SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);
            Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(!DatabaseRegistry.playerDB.Player.gemWaterCollected);
        }

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

    void ShowPromptAllPillars()
    {
        foreach (var pillar in simonPillars) if (pillar != null) pillar.interactPrompt.SetActive(true);
    }

    void HidePromptAllPillars()
    {
        foreach (var pillar in simonPillars) if (pillar != null) pillar.interactPrompt.SetActive(false);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}