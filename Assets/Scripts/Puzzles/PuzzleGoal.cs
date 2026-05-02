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

    public Entity Gem;

    public float movementSpeed = 2.0f;
    public float movementDistance = 1.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;

    private bool isMoving = false;
    private int pendingMoves = 0;

    private bool puzzle1Completed;

    // Particles
    private ParticleComponent goalParticles;
    private bool particlesSwitched = true;

    // Sounds
    private AudioSource moveSFX;
    public Entity completeSFX;
    public Entity collectGemSFX;

    void OnCreate()
    {
        pillars = new MovingPillar[4];
        pillarTriggered = new bool[4];

        pillars[0] = Pillar1.GetComponent<MovingPillar>();
        pillars[1] = Pillar2.GetComponent<MovingPillar>();
        pillars[2] = Pillar3.GetComponent<MovingPillar>();
        pillars[3] = Pillar4.GetComponent<MovingPillar>();

        Gem.GetComponent<BoxCollider>().SetActive(false);

        moveSFX = entity.GetComponent<AudioSource>();

        goalParticles = entity.GetComponent<ParticleComponent>();
        goalParticles.Stop();
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

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

        if (!isMoving && !particlesSwitched)
        {
            goalParticles.Stop();
            particlesSwitched = true;
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

        if (DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed && !puzzle1Completed)
        {
            puzzle1Completed = true;
            
            CompletePuzzleAuto();

            Gem.SetActive(!DatabaseRegistry.playerDB.Player.gemAirCollected);
            Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(!DatabaseRegistry.playerDB.Player.gemAirCollected);
            Gem.GetComponent<BoxCollider>().SetActive(!DatabaseRegistry.playerDB.Player.gemAirCollected);
        }

        if (allOnGoal && !puzzle1Completed)
        {
            puzzle1Completed = true;

            DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed = true;

            completeSFX.GetComponent<AudioSource>().Play();

            Gem.GetComponent<BoxCollider>().SetActive(true);
            Gem.GetComponent<Gem_Idle>().interactionPrompt.SetActive(true);
        }

        if (Gem.GetComponent<BoxCollider>().IsColliding && Player.Instance.Input.interactKeyPressed)
        {
            Gem.SetActive(false);

            collectGemSFX.GetComponent<AudioSource>().Play();

            DatabaseRegistry.playerDB.Player.gemAirCollected = true;
            DatabaseRegistry.playerDB.Player.hasBurner = true;
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

        goalParticles.Play();
        moveSFX.Play();
        particlesSwitched = false;
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

        completeSFX.GetComponent<AudioSource>().Play();
    }
};