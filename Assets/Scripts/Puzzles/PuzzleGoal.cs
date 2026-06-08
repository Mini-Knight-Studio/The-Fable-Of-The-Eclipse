using System;
using System.Collections;
using Loopie;

class PuzzleGoal : Component
{
    private MovingPillar[] pillars;
    private bool[] pillarTriggered;

    public Entity Pillar1;
    public Entity Pillar2;
    public Entity Pillar3;
    public Entity Pillar4;

    public Entity Gem;

    public float movementSpeed = 2.0f;
    public float movementDistance = 1.0f;

    private bool isMoving = false;
    private int pendingMoves = 0;

    private bool puzzle1Completed;
    private bool isCollecting = false;

    private float cameraShakeDuration = 1f;
    private float cameraShakeAmount = 0.1f;
    private float cameraShakeRotation = 0.1f;
    private float cameraShakeAmountVel = 10f;
    private float cameraShakeRotationVel = 10f;

    public float collectTime = 5f;

    // Particles
    private ParticleComponent goalParticles;
    private bool particlesSwitched = true;

    // Sounds
    private AudioSource moveSFX;
    public Entity completeSFX;
    public Entity collectGemSFX;

    public string popupName = "Popup_GemAir";

    [Header("Mechanic door")]
    public Entity doorEntity;
    public Entity doorFocusPoint;
    public float finalDoorHeight;
    public float lowerDuration = 2.0f;
    public float pauseBeforeLowering = 0.5f;
    public float easeIntensity = 1.5f;
    public float doorCameraZoom = 20f;
    public float doorFocusDuration = 1.5f;

    private float initialDoorHeight;

    public float doorShakeDuration = 2.0f;
    public float doorShakeAmount = 0.4f;
    public float doorShakeRotation = 0.4f;
    public float doorShakeAmountVel = 10f;
    public float doorShakeRotationVel = 10f;

    public Entity mechanichDoorParticlesEntity;
    private ParticleComponent mechanichDoorParticles;

    public Entity mechanichDoorSFXEntity;
    private AudioSource mechanichDoorSFX;
    public Entity mechanichDoorSFXEntity2;
    private AudioSource mechanichDoorSFX2;
    public Entity mechanichDoorSFXEntity3;
    private AudioSource mechanichDoorSFX3;

    public Entity mechanichDoorThumpSFXEntity;
    private AudioSource mechanichDoorThumpSFX;

    void OnCreate()
    {
        pillars = new MovingPillar[4];
        pillarTriggered = new bool[4];

        pillars[0] = Pillar1.GetComponent<MovingPillar>();
        pillars[1] = Pillar2.GetComponent<MovingPillar>();
        pillars[2] = Pillar3.GetComponent<MovingPillar>();
        pillars[3] = Pillar4.GetComponent<MovingPillar>();

        if (doorEntity != null)
        {
            initialDoorHeight = doorEntity.transform.position.y;
        }

        Gem.GetComponent<BoxCollider>().SetActive(false);

        moveSFX = entity.GetComponent<AudioSource>();

        if (mechanichDoorParticlesEntity != null)
            mechanichDoorParticles = mechanichDoorParticlesEntity.GetComponent<ParticleComponent>();

        if (mechanichDoorSFXEntity != null)
            mechanichDoorSFX = mechanichDoorSFXEntity.GetComponent<AudioSource>();
        if (mechanichDoorSFXEntity2 != null)
            mechanichDoorSFX2 = mechanichDoorSFXEntity2.GetComponent<AudioSource>();
        if (mechanichDoorSFXEntity3 != null)
            mechanichDoorSFX3 = mechanichDoorSFXEntity3.GetComponent<AudioSource>();

        if (mechanichDoorThumpSFXEntity != null)
            mechanichDoorThumpSFX = mechanichDoorThumpSFXEntity.GetComponent<AudioSource>();

        goalParticles = entity.GetComponent<ParticleComponent>();
        goalParticles.Stop();
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        CheckPillars();

        if (pendingMoves > 0 && !isMoving)
        {
            StartCoroutine(ProcessMovementQueue());
        }
    }

    void CheckPillars()
    {
        bool allOnGoal = true;

        for (int i = 0; i < pillars.Length; i++)
        {
            if (pillars[i] == null) continue;

            if (pillars[i].stopedOnGoal && !pillarTriggered[i])
            {
                pillarTriggered[i] = true;
                pendingMoves++;
            }

            if (!pillars[i].stopedOnGoal)
            {
                allOnGoal = false;
            }
        }

        if (DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed && !puzzle1Completed)
        {
            puzzle1Completed = true;

            CompletePuzzleAuto();

            if (!isCollecting)
            {
                Gem.SetActive(!DatabaseRegistry.playerDB.Player.gemAirCollected);
                Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(!DatabaseRegistry.playerDB.Player.gemAirCollected);
                Gem.GetComponent<BoxCollider>().SetActive(!DatabaseRegistry.playerDB.Player.gemAirCollected);
                if (doorEntity != null && DatabaseRegistry.playerDB.Player.gemAirCollected)
                {
                    doorEntity.transform.position = new Vector3(doorEntity.transform.position.x, finalDoorHeight, doorEntity.transform.position.z);
                }
            }
        }

        if (allOnGoal && !puzzle1Completed)
        {
            puzzle1Completed = true;
            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed = true;

            Gem.GetComponent<BoxCollider>().SetActive(true);
            Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(true);

            completeSFX.GetComponent<AudioSource>().Play();
        }

        if (!isCollecting && Gem.GetComponent<BoxCollider>().IsColliding && Player.Instance.Input.interactKeyPressed)
        {
            StartCoroutine(Collect());
        }
    }

    IEnumerator PuzzleCompleteCinematic()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        if (doorEntity != null)
        {
            Player.Instance.Camera.FocusOnPoint(doorFocusPoint.transform.position, doorCameraZoom, 4);
            yield return new WaitForSeconds(doorFocusDuration);

            if (mechanichDoorParticlesEntity != null) mechanichDoorParticles.Play();

            if (mechanichDoorSFXEntity != null) mechanichDoorSFX.Play();
            if (mechanichDoorSFXEntity2 != null) mechanichDoorSFX2.Play();
            if (mechanichDoorSFXEntity3 != null) mechanichDoorSFX3.Play();

            yield return new WaitForSeconds(pauseBeforeLowering);

            Player.Instance.Camera.SetIsShaking(true, doorShakeDuration + pauseBeforeLowering, doorShakeAmount, doorShakeRotation, doorShakeAmountVel, doorShakeRotationVel);

            float elapsedTime = 0f;
            while (elapsedTime < lowerDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / lowerDuration;
                float curvedT = Mathf.Pow(t, easeIntensity);

                float currentHeight = Mathf.Lerp(initialDoorHeight, finalDoorHeight, curvedT);
                doorEntity.transform.position = new Vector3(doorEntity.transform.position.x, currentHeight, doorEntity.transform.position.z);

                yield return null;
            }

            doorEntity.transform.position = new Vector3(doorEntity.transform.position.x, finalDoorHeight, doorEntity.transform.position.z);

            if (mechanichDoorThumpSFXEntity != null) mechanichDoorThumpSFX.Play();

            if (mechanichDoorParticlesEntity != null) mechanichDoorParticles.Stop();
            if (mechanichDoorSFXEntity2 != null) mechanichDoorSFX2.Stop();
            if (mechanichDoorSFXEntity3 != null) mechanichDoorSFX3.Stop();
        }

        yield return new WaitForSeconds(1.0f);
        Player.Instance.Camera.StopFocus();
        yield return new WaitForSeconds(0.5f);

        GameManager.SetState(GameManager.GameState.DEFAULT);
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
        Pillar4.GetComponent<MovingPillar>().CompletePillarAuto();

        float totalDropDistance = 4f * movementDistance;
        entity.transform.position = new Vector3(entity.transform.position.x, entity.transform.position.y - totalDropDistance, entity.transform.position.z);

        for (int i = 0; i < pillarTriggered.Length; i++)
        {
            pillarTriggered[i] = true;
        }
        pendingMoves = 0;
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

        DatabaseRegistry.playerDB.Player.gemAirCollected = true;

        //if (UIPopupManager.Instance != null)
        //{
        //    UIPopupManager.Instance.ShowPopup(popupName);
        //}

        StartCoroutine(PuzzleCompleteCinematic());

        isCollecting = false;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}