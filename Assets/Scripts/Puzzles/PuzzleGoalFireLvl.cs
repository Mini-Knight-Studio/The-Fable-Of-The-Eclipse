using System;
using System.Collections;
using Loopie;

class PuzzleGoalFireLvl : Component
{
    private MovingPillar[] pillars;
    private bool[] pillarTriggered;

    public Entity Pillar1;
    public Entity Pillar2;
    public Entity Pillar3;

    public Entity Gem;

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

    // Particles
    private ParticleComponent goalParticles;
    private bool particlesSwitched = true;

    // Sounds
    private AudioSource moveSFX;
    public Entity completeSFX;
    public Entity collectGemSFX;

    public string popupName = "Popup_GemFire";

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
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

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

        isCollecting = false;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}