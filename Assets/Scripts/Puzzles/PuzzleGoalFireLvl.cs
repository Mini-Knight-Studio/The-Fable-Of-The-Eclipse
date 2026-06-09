using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

class PuzzleGoalFireLvl : Component
{
    [Header("References")]
    private MovingPillar[] pillars;
    private bool[] pillarTriggered;
    private Vector3[] pillarInitialPositions;

    public Entity Pillar1;
    public Entity Pillar2;
    public Entity Pillar3;

    public Entity Gem;

    public Entity PlayerCollidersHolder;

    public Entity puzzle3BlockerEntity;
    public Puzzle3Blocker puzzle3Blocker;

    [Header("Exposed Reset Layout")]
    public Vector3 Pillar1ResetPosition;
    public Vector3 Pillar2ResetPosition;
    public Vector3 Pillar3ResetPosition;

    [Header("Settings")]
    public float movementSpeed = 2.0f;
    public float movementDistance = 1.0f;

    private bool isMoving = false;
    private int pendingMoves = 0;

    private bool puzzle3Completed;
    private bool isCollecting = false;

    private float cameraShakeDuration = 1f;
    private float cameraShakeAmount = 0.1f;
    private float cameraShakeRotation = 0.1f;
    private float cameraShakeAmountVel = 10f;
    private float cameraShakeRotationVel = 10f;

    public float collectTime = 1f;

    [Header("Puzzle Reset")]
    public Entity PuzzleReseter;
    public Entity PuzzleReseterMoon;
    public Entity puzzleReseterPrompt;
    public Entity focusPointOnReset;

    public float resetPillarMovementDistance = 5f;
    public float resetDuration = 3f;
    public float easeIntensity = 1.5f;
    public float cameraZoom = 20f;

    private float puzzleResetMoonInitY = 0f;

    private BoxCollider puzzleReseterCollider;

    [Header("Feedback")]
    private ParticleComponent goalParticles;
    private bool particlesSwitched = true;

    private AudioSource moveSFX;
    public Entity completeSFX;
    public Entity collectGemSFX;

    public Entity buttonResetSFX;
    public Entity pillarsResetSFX;
    public Entity pillarsResetSFX2;

    public string popupName = "Popup_GemFire";

    private Vector3 initialGoalPosition;

    private bool canStartCinematic = false;

    void OnCreate()
    {
        pillars = new MovingPillar[3];
        pillarTriggered = new bool[3];
        pillarInitialPositions = new Vector3[3];

        pillars[0] = Pillar1.GetComponent<MovingPillar>();
        pillars[1] = Pillar2.GetComponent<MovingPillar>();
        pillars[2] = Pillar3.GetComponent<MovingPillar>();

        pillarInitialPositions[0] = Pillar1ResetPosition != Vector3.Zero ? Pillar1ResetPosition : (Pillar1 != null ? Pillar1.transform.position : Vector3.Zero);
        pillarInitialPositions[1] = Pillar2ResetPosition != Vector3.Zero ? Pillar2ResetPosition : (Pillar2 != null ? Pillar2.transform.position : Vector3.Zero);
        pillarInitialPositions[2] = Pillar3ResetPosition != Vector3.Zero ? Pillar3ResetPosition : (Pillar3 != null ? Pillar3.transform.position : Vector3.Zero);

        Gem.GetComponent<BoxCollider>().SetActive(false);

        moveSFX = entity.GetComponent<AudioSource>();

        puzzle3Blocker = puzzle3BlockerEntity.GetComponent<Puzzle3Blocker>();

        goalParticles = entity.GetComponent<ParticleComponent>();
        goalParticles.Stop();

        if (PuzzleReseter != null)
        {
            puzzleReseterCollider = PuzzleReseter.GetComponent<BoxCollider>();
            puzzleResetMoonInitY = PuzzleReseterMoon.transform.position.y;
        }

        initialGoalPosition = entity.transform.position;
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        CheckPillars();

        if (pendingMoves > 0 && !isMoving)
        {
            StartCoroutine(ProcessMovementQueue());
        }

        if (Player.Instance.Grapple.IsGrappling && PlayerCollidersHolder.Active)
        {
            PlayerCollidersHolder.SetActive(false);
        }
        else if (!Player.Instance.Grapple.IsGrappling && !PlayerCollidersHolder.Active)
        {
            PlayerCollidersHolder.SetActive(true);
        }

        if (PuzzleReseter != null)
        {
            if (puzzleReseterCollider.IsColliding && !puzzle3Completed)
            {
                if (!puzzleReseterPrompt.Active)
                {
                    puzzleReseterPrompt.SetActive(true);
                }

                if (Player.Instance.Input.interactKeyPressed)
                {
                    StartCoroutine(ResetPuzzle());
                    puzzleReseterPrompt.SetActive(false);
                }
            }
            else
            {
                if (puzzleReseterPrompt.Active)
                {
                    puzzleReseterPrompt.SetActive(false);
                }
            }
        }

        if (canStartCinematic)
        {
            puzzle3Blocker.StartCompletitionCinematic();
            canStartCinematic = false;
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

        if (DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed && !puzzle3Completed)
        {
            puzzle3Completed = true;

            CompletePuzzleAuto();

            if (!isCollecting)
            {
                Gem.SetActive(!DatabaseRegistry.playerDB.Player.gemFireCollected);
                Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(!DatabaseRegistry.playerDB.Player.gemFireCollected);
                Gem.GetComponent<BoxCollider>().SetActive(!DatabaseRegistry.playerDB.Player.gemFireCollected);
            }
        }

        if (allOnGoal && !puzzle3Completed)
        {
            puzzle3Completed = true;

            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle3Completed = true;

            completeSFX.GetComponent<AudioSource>().Play();

            Gem.GetComponent<BoxCollider>().SetActive(true);
            Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(true);
        }

        if (!isCollecting && Gem.GetComponent<BoxCollider>().IsColliding && Player.Instance.Input.interactKeyPressed)
        {
            StartCoroutine(Collect());
        }
    }

    IEnumerator ProcessMovementQueue()
    {
        isMoving = true;

        goalParticles.Play();
        moveSFX.Play();
        particlesSwitched = false;

        while (pendingMoves > 0)
        {
            pendingMoves--;

            Vector3 startPos = entity.transform.position;
            Vector3 targetPos = startPos + new Vector3(0, -movementDistance, 0);

            float distance = (float)(targetPos - startPos).magnitude;
            float moveDuration = distance / movementSpeed;
            float timer = 0.0f;

            Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount, cameraShakeRotation, cameraShakeAmountVel, cameraShakeRotationVel);

            while (timer < moveDuration)
            {
                timer += Time.deltaTime;
                float t = timer / moveDuration;
                entity.transform.position = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }

            entity.transform.position = targetPos;
        }

        goalParticles.Stop();
        particlesSwitched = true;
        isMoving = false;
    }

    void CompletePuzzleAuto()
    {
        Pillar1.GetComponent<MovingPillar>().CompletePillarAuto();
        Pillar2.GetComponent<MovingPillar>().CompletePillarAuto();
        Pillar3.GetComponent<MovingPillar>().CompletePillarAuto();

        completeSFX.GetComponent<AudioSource>().Play();
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

        collectGemSFX.GetComponent<AudioSource>().Play();

        DatabaseRegistry.playerDB.Player.gemFireCollected = true;

        if (UIPopupManager.Instance != null)
        {
            UIPopupManager.Instance.ShowPopup(popupName);
        }

        canStartCinematic = true;

        isCollecting = false;
    }

    public void CallResetPuzzle()
    {
        StartCoroutine(ResetPuzzle());
    }

    IEnumerator ResetPuzzle()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        if (buttonResetSFX != null) buttonResetSFX.GetComponent<AudioSource>().Play();

        float buttonPressDuration = 1.5f;
        float buttonLowerY = puzzleResetMoonInitY - 0.75f;
        float buttonTimer = 0f;

        while (buttonTimer < buttonPressDuration)
        {
            buttonTimer += Time.deltaTime;
            float t = buttonTimer / buttonPressDuration;
            float currentButtonY = Mathf.Lerp(puzzleResetMoonInitY, buttonLowerY, t);
            PuzzleReseterMoon.transform.position = new Vector3(PuzzleReseterMoon.transform.position.x, currentButtonY, PuzzleReseterMoon.transform.position.z);
            yield return null;
        }
        PuzzleReseterMoon.transform.position = new Vector3(PuzzleReseterMoon.transform.position.x, buttonLowerY, PuzzleReseterMoon.transform.position.z);

        Player.Instance.Camera.FocusOnPoint(focusPointOnReset.transform.position, cameraZoom, 4);

        if (pillarsResetSFX != null) pillarsResetSFX.GetComponent<AudioSource>().Play();
        if (pillarsResetSFX2 != null) pillarsResetSFX2.GetComponent<AudioSource>().Play();

        Dictionary<dynamic, Vector3> lowerStartPositions = new Dictionary<dynamic, Vector3>();
        Dictionary<dynamic, Vector3> lowerTargetPositions = new Dictionary<dynamic, Vector3>();
        Dictionary<dynamic, float> pillarBaseX = new Dictionary<dynamic, float>();
        Dictionary<dynamic, float> pillarBaseZ = new Dictionary<dynamic, float>();

        float trembleMagnitude = 0.075f;
        float trembleFrequency = 50f;

        foreach (var pillar in pillars)
        {
            if (pillar != null)
            {
                lowerStartPositions[pillar] = pillar.transform.position;
                lowerTargetPositions[pillar] = new Loopie.Vector3(pillar.transform.position.x, pillar.transform.position.y - resetPillarMovementDistance, pillar.transform.position.z);
                pillarBaseX[pillar] = pillar.transform.position.x;
                pillarBaseZ[pillar] = pillar.transform.position.z;
            }
        }

        float elapsedTime = 0f;
        while (elapsedTime < resetDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Min(elapsedTime / resetDuration, 1f);
            float curvedT = Mathf.Pow(t, easeIntensity);

            float shakeX = Mathf.Sin(elapsedTime * trembleFrequency) * trembleMagnitude;
            float shakeZ = Mathf.Cos(elapsedTime * trembleFrequency) * trembleMagnitude;

            foreach (var pillar in pillars)
            {
                if (pillar != null)
                {
                    float currentHeight = Mathf.Lerp(lowerStartPositions[pillar].y, lowerTargetPositions[pillar].y, curvedT);
                    pillar.transform.position = new Vector3(pillarBaseX[pillar] + shakeX, currentHeight, pillarBaseZ[pillar] + shakeZ);
                }
            }
            yield return null;
        }

        foreach (var pillar in pillars)
        {
            if (pillar != null) pillar.transform.position = lowerTargetPositions[pillar];
        }

        Dictionary<dynamic, Vector3> riseStartPositions = new Dictionary<dynamic, Vector3>();
        Dictionary<dynamic, Vector3> riseTargetPositions = new Dictionary<dynamic, Vector3>();

        for (int i = 0; i < pillars.Length; i++)
        {
            var pillar = pillars[i];
            if (pillar != null)
            {
                pillar.ResetPillarInstant();

                riseTargetPositions[pillar] = pillarInitialPositions[i];
                riseStartPositions[pillar] = new Loopie.Vector3(pillarInitialPositions[i].x, pillarInitialPositions[i].y - resetPillarMovementDistance, pillarInitialPositions[i].z);

                pillar.transform.position = riseStartPositions[pillar];

                pillarBaseX[pillar] = pillar.transform.position.x;
                pillarBaseZ[pillar] = pillar.transform.position.z;
            }
        }

        yield return new WaitForSeconds(2.5f);

        if (pillarsResetSFX != null) pillarsResetSFX.GetComponent<AudioSource>().Play();
        if (pillarsResetSFX2 != null) pillarsResetSFX2.GetComponent<AudioSource>().Play();

        elapsedTime = 0f;
        while (elapsedTime < resetDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Min(elapsedTime / resetDuration, 1f);
            float curvedT = Mathf.Pow(t, easeIntensity);

            float shakeX = Mathf.Sin(elapsedTime * trembleFrequency) * trembleMagnitude;
            float shakeZ = Mathf.Cos(elapsedTime * trembleFrequency) * trembleMagnitude;

            foreach (var pillar in pillars)
            {
                if (pillar != null)
                {
                    float currentHeight = Mathf.Lerp(riseStartPositions[pillar].y, riseTargetPositions[pillar].y, curvedT);
                    pillar.transform.position = new Vector3(pillarBaseX[pillar] + shakeX, currentHeight, pillarBaseZ[pillar] + shakeZ);
                }
            }
            yield return null;
        }

        foreach (var pillar in pillars)
        {
            if (pillar != null)
            {
                pillar.transform.position = riseTargetPositions[pillar];
                yield return null;
            }
        }

        buttonTimer = 0f;
        while (buttonTimer < buttonPressDuration)
        {
            buttonTimer += Time.deltaTime;
            float t = buttonTimer / buttonPressDuration;
            float currentButtonY = Mathf.Lerp(buttonLowerY, puzzleResetMoonInitY, t);
            PuzzleReseterMoon.transform.position = new Vector3(PuzzleReseterMoon.transform.position.x, currentButtonY, PuzzleReseterMoon.transform.position.z);
            yield return null;
        }
        PuzzleReseterMoon.transform.position = new Vector3(PuzzleReseterMoon.transform.position.x, puzzleResetMoonInitY, PuzzleReseterMoon.transform.position.z);

        Vector3 currentGoalPos = entity.transform.position;
        float goalDist = (float)(initialGoalPosition - currentGoalPos).magnitude;

        if (goalDist > 0.001f)
        {
            goalParticles.Play();
            moveSFX.Play();
            Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount, cameraShakeRotation, cameraShakeAmountVel, cameraShakeRotationVel);

            float moveDuration = goalDist / movementSpeed;
            float moveTimer = 0f;

            while (moveTimer < moveDuration)
            {
                moveTimer += Time.deltaTime;
                float t = moveTimer / moveDuration;
                entity.transform.position = Vector3.Lerp(currentGoalPos, initialGoalPosition, t);
                yield return null;
            }

            entity.transform.position = initialGoalPosition;
            goalParticles.Stop();
        }

        for (int i = 0; i < pillarTriggered.Length; i++)
        {
            if (pillars[i] != null)
            {
                pillarTriggered[i] = false;
            }
        }
        pendingMoves = 0;

        Player.Instance.Camera.StopFocus();
        GameManager.SetState(GameManager.GameState.DEFAULT);
        yield return null;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}