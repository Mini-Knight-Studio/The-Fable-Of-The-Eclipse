using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

class PuzzleGoalFireLvl : Component
{
    [Header("References")]
    private MovingPillar[] pillars;
    private bool[] pillarTriggered;

    public Entity Pillar1;
    public Entity Pillar2;
    public Entity Pillar3;

    public Entity Gem;

    public Entity PlayerCollidersHolder;

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
    public Entity puzzleReseterPrompt;
    public Entity focusPointOnReset;

    public float resetPillarMovementDistance = 5f;
    public float resetDuration = 3f;
    public float easeIntensity = 1.5f;
    public float cameraZoom = 20f;

    private BoxCollider puzzleReseterCollider;

    [Header("Feedback")]
    private ParticleComponent goalParticles;
    private bool particlesSwitched = true;

    private AudioSource moveSFX;
    public Entity completeSFX;
    public Entity collectGemSFX;

    public string popupName = "Popup_GemFire";

    private Vector3 initialGoalPosition;

    void OnCreate()
    {
        pillars = new MovingPillar[3];
        pillarTriggered = new bool[3];

        pillars[0] = Pillar1.GetComponent<MovingPillar>();
        pillars[1] = Pillar2.GetComponent<MovingPillar>();
        pillars[2] = Pillar3.GetComponent<MovingPillar>();

        Gem.GetComponent<BoxCollider>().SetActive(false);

        moveSFX = entity.GetComponent<AudioSource>();

        goalParticles = entity.GetComponent<ParticleComponent>();
        goalParticles.Stop();

        if (PuzzleReseter != null)
        {
            puzzleReseterCollider = PuzzleReseter.GetComponent<BoxCollider>();
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

        //if (UIPopupManager.Instance != null)
        //{
        //    UIPopupManager.Instance.ShowPopup(popupName);
        //}

        isCollecting = false;
    }

    IEnumerator ResetPuzzle()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        Player.Instance.Camera.FocusOnPoint(focusPointOnReset.transform.position, cameraZoom, 4);

        Dictionary<dynamic, Vector3> lowerStartPositions = new Dictionary<dynamic, Vector3>();
        Dictionary<dynamic, Vector3> lowerTargetPositions = new Dictionary<dynamic, Vector3>();

        foreach (var pillar in pillars)
        {
            if (pillar != null)
            {
                lowerStartPositions[pillar] = pillar.transform.position;
                lowerTargetPositions[pillar] = new Loopie.Vector3(pillar.transform.position.x, pillar.transform.position.y - resetPillarMovementDistance, pillar.transform.position.z);
            }
        }

        float elapsedTime = 0f;
        while (elapsedTime < resetDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Min(elapsedTime / resetDuration, 1f);
            float curvedT = Mathf.Pow(t, easeIntensity);

            foreach (var pillar in pillars)
            {
                if (pillar != null)
                {
                    pillar.transform.position = Vector3.Lerp(lowerStartPositions[pillar], lowerTargetPositions[pillar], curvedT);
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

        foreach (var pillar in pillars)
        {
            if (pillar != null)
            {
                pillar.ResetPillarInstant();

                riseTargetPositions[pillar] = pillar.transform.position;

                riseStartPositions[pillar] = new Loopie.Vector3(pillar.transform.position.x, pillar.transform.position.y - resetPillarMovementDistance, pillar.transform.position.z);

                pillar.transform.position = riseStartPositions[pillar];
            }
        }

        yield return new WaitForSeconds(2.5f);

        elapsedTime = 0f;
        while (elapsedTime < resetDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Min(elapsedTime / resetDuration, 1f);
            float curvedT = Mathf.Pow(t, easeIntensity);

            foreach (var pillar in pillars)
            {
                if (pillar != null)
                {
                    pillar.transform.position = Vector3.Lerp(riseStartPositions[pillar], riseTargetPositions[pillar], curvedT);
                }
            }
            yield return null;
        }

        foreach (var pillar in pillars)
        {
            if (pillar != null)
            {
                pillar.transform.position = riseTargetPositions[pillar];
                pillar.ResetPillarInstant();
                yield return null;
            }
        }

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
            pillarTriggered[i] = false;
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