using System;
using System.Collections;
using Loopie;

class MovingPlatform : Component
{
    [Header("Model")]
    public Entity platformModel;

    public Entity doublePlatformGroup;
    public Entity doublePlatformModel1;
    public Entity doublePlatformModel2;

    private Entity activeMovingEntity;
    private BoxCollider collider1;
    private BoxCollider collider2;

    [Header("Path Points")]
    public bool followPathRotations = false;
    public int numOfPathPoints = 5;
    public Entity pathPointEntity0;
    public Entity pathPointEntity1;
    public Entity pathPointEntity2;
    public Entity pathPointEntity3;
    public Entity pathPointEntity4;
    public Entity pathPointEntity5;
    public Entity pathPointEntity6;
    public Entity pathPointEntity7;
    public Entity pathPointEntity8;
    public Entity pathPointEntity9;

    private Entity[] pathPoints;

    [Header("Point Pause Times")]
    public float pauseInPoint0 = 0.0f;
    public float pauseInPoint1 = 0.0f;
    public float pauseInPoint2 = 0.0f;
    public float pauseInPoint3 = 0.0f;
    public float pauseInPoint4 = 0.0f;
    public float pauseInPoint5 = 0.0f;
    public float pauseInPoint6 = 0.0f;
    public float pauseInPoint7 = 0.0f;
    public float pauseInPoint8 = 0.0f;
    public float pauseInPoint9 = 0.0f;

    private float[] pointPauseTimes;

    [Header("Feedback")]
    public Entity movingParticlesEntity;
    private ParticleComponent movingParticles;
    public Entity movingParticlesEntity2;
    private ParticleComponent movingParticles2;

    [Header("Settings")]
    public float movementSpeed = 2.0f;

    public bool moveOnStart = false;
    public bool looping = false;

    private bool goingToPoint = false;
    public int targetPoint = 0;

    private int currentPoint = 0;

    private bool movementRoutineRunning = false;

    [Header("Start moving on Collision")]
    public bool activateOnCollision = false;
    private bool isWaitingForPlayer = false;

    [Header("Start moving after Burned")]
    public bool activateAfterBurn = false;
    public string burnableID = "UNASSIGNED_BURNABLE";

    [Header("Sinking Platforms")]
    public bool canSink = false;
    public float timeBeforeSink = 2.0f;
    public float timeSunken = 3.0f;
    public float sinkDistance = 5.0f;
    public float sinkSpeed = 2.0f;

    private bool isSinking = false;
    private bool sinkRoutineRunning = false;
    private Vector3 positionBeforeSink;

    void OnCreate()
    {
        if (doublePlatformGroup != null)
        {
            activeMovingEntity = doublePlatformGroup;
            if (doublePlatformModel1 != null) collider1 = doublePlatformModel1.GetComponent<BoxCollider>();
            if (doublePlatformModel2 != null) collider2 = doublePlatformModel2.GetComponent<BoxCollider>();
        }
        else
        {
            activeMovingEntity = platformModel;
            if (platformModel != null) collider1 = platformModel.GetComponent<BoxCollider>();
        }

        movingParticles = movingParticlesEntity.GetComponent<ParticleComponent>();
        movingParticles.Stop();

        if (movingParticlesEntity2 != null)
        {
            movingParticles2 = movingParticlesEntity2.GetComponent<ParticleComponent>();
            movingParticles2.Stop();
        }

        pathPoints = new Entity[10];

        pathPoints[0] = pathPointEntity0;
        pathPoints[1] = pathPointEntity1;
        pathPoints[2] = pathPointEntity2;
        pathPoints[3] = pathPointEntity3;
        pathPoints[4] = pathPointEntity4;
        pathPoints[5] = pathPointEntity5;
        pathPoints[6] = pathPointEntity6;
        pathPoints[7] = pathPointEntity7;
        pathPoints[8] = pathPointEntity8;
        pathPoints[9] = pathPointEntity9;

        pointPauseTimes = new float[10];

        pointPauseTimes[0] = pauseInPoint0;
        pointPauseTimes[1] = pauseInPoint1;
        pointPauseTimes[2] = pauseInPoint2;
        pointPauseTimes[3] = pauseInPoint3;
        pointPauseTimes[4] = pauseInPoint4;
        pointPauseTimes[5] = pauseInPoint5;
        pointPauseTimes[6] = pauseInPoint6;
        pointPauseTimes[7] = pauseInPoint7;
        pointPauseTimes[8] = pauseInPoint8;
        pointPauseTimes[9] = pauseInPoint9;

        if (activateOnCollision)
        {
            isWaitingForPlayer = true;
            goingToPoint = false;
        }
        else if (moveOnStart || (activateAfterBurn && DatabaseRegistry.levelsDB.Levels.IsBurnableBurned(burnableID)))
        {
            GoToPoint(targetPoint);
        }
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        if (canSink && !sinkRoutineRunning)
        {
            if (CheckCollision())
            {
                StartCoroutine(SinkRoutine());
            }
        }

        if (!isSinking)
        {
            if (activateOnCollision && isWaitingForPlayer)
            {
                if (CheckCollision())
                {
                    isWaitingForPlayer = false;
                    GoToPoint(targetPoint);
                }
            }
            else if (activateAfterBurn && DatabaseRegistry.levelsDB.Levels.IsBurnableBurned(burnableID))
            {
                GoToPoint(targetPoint);
            }
        }
    }

    private bool CheckCollision()
    {
        bool hit = false;
        if (collider1 != null && collider1.IsColliding) hit = true;
        if (collider2 != null && collider2.IsColliding) hit = true;
        return hit;
    }

    public void GoToPoint(int pointNum)
    {
        if (pointNum < 0 || pointNum >= numOfPathPoints) return;

        if (movementRoutineRunning) return;

        targetPoint = pointNum;

        StartCoroutine(MoveToPointRoutine());
    }

    IEnumerator MoveToPointRoutine()
    {
        movementRoutineRunning = true;
        goingToPoint = true;

        movingParticles.Play();
        if (movingParticlesEntity2 != null) movingParticles2.Play();

        while (true)
        {
            if (Pause.isPaused)
            {
                yield return null;
                continue;
            }

            if (isSinking)
            {
                yield return null;
                continue;
            }

            if (pathPoints[targetPoint] == null)
            {
                break;
            }

            Vector3 startPos = activeMovingEntity.transform.position;
            Vector3 startRot = activeMovingEntity.transform.rotation;
            Vector3 targetPos = pathPoints[targetPoint].transform.position;
            Vector3 targetRot = pathPoints[targetPoint].transform.rotation;

            float totalDistance = (float)(targetPos - startPos).magnitude;

            while (true)
            {
                if (Pause.isPaused || isSinking)
                {
                    yield return null;
                    continue;
                }

                Vector3 currentPos = activeMovingEntity.transform.position;
                Vector3 directionVec = targetPos - currentPos;
                float remainingDistance = (float)directionVec.magnitude;

                if (remainingDistance < 0.1f)
                {
                    break;
                }

                float distanceCovered = totalDistance - remainingDistance;
                float interpolationFactor = distanceCovered / totalDistance;

                Vector3 moveDir = directionVec.normalized;
                Vector3 velocity = moveDir * movementSpeed * Time.deltaTime;

                activeMovingEntity.transform.position += velocity;

                activeMovingEntity.transform.rotation = Vector3.Lerp(
                    startRot,
                    targetRot,
                    interpolationFactor
                );

                if (CheckCollision())
                {
                    Player.Instance.transform.position += velocity;
                }

                yield return null;
            }

            currentPoint = targetPoint;
            activeMovingEntity.transform.position = targetPos;
            activeMovingEntity.transform.rotation = targetRot;

            goingToPoint = false;
            movingParticles.Stop();
            if (movingParticlesEntity2 != null) movingParticles2.Stop();

            if (pointPauseTimes[currentPoint] > 0.0f)
            {
                yield return new WaitForSeconds(pointPauseTimes[currentPoint]);
            }

            if (activateOnCollision && currentPoint == 0)
            {
                isWaitingForPlayer = true;
                break;
            }

            if (looping || activateOnCollision)
            {
                targetPoint = (currentPoint + 1) % numOfPathPoints;
                goingToPoint = true;
                movingParticles.Play();
                if (movingParticlesEntity2 != null) movingParticles2.Play();
                yield return null;
                continue;
            }

            break;
        }

        movementRoutineRunning = false;
    }

    IEnumerator SinkRoutine()
    {
        sinkRoutineRunning = true;

        Player.Instance.Camera.SetIsShaking(true, timeBeforeSink * 1.5f, 0.05f, 0.05f, 50f, 50f);

        yield return new WaitForSeconds(timeBeforeSink);

        if (!CheckCollision())
        {
            Player.Instance.Camera.SetIsShaking(false);
            sinkRoutineRunning = false;
            yield break;
        }

        isSinking = true;

        positionBeforeSink = activeMovingEntity.transform.position;

        Vector3 sinkTarget = positionBeforeSink - new Vector3(0, sinkDistance, 0);

        while (true)
        {
            if (Pause.isPaused)
            {
                yield return null;
                continue;
            }

            Vector3 directionVec = sinkTarget - activeMovingEntity.transform.position;
            float distance = (float)directionVec.magnitude;

            if (distance < 0.1f)
            {
                activeMovingEntity.transform.position = sinkTarget;
                break;
            }

            Vector3 moveDir = directionVec.normalized;
            Vector3 velocity = moveDir * sinkSpeed * Time.deltaTime;

            activeMovingEntity.transform.position += velocity;

            yield return null;
        }

        yield return new WaitForSeconds(timeSunken);

        while (true)
        {
            if (Pause.isPaused)
            {
                yield return null;
                continue;
            }

            Vector3 directionVec = positionBeforeSink - activeMovingEntity.transform.position;
            float distance = (float)directionVec.magnitude;

            if (distance < 0.1f)
            {
                activeMovingEntity.transform.position = positionBeforeSink;
                break;
            }

            Vector3 moveDir = directionVec.normalized;
            Vector3 velocity = moveDir * sinkSpeed * Time.deltaTime;

            activeMovingEntity.transform.position += velocity;

            yield return null;
        }

        isSinking = false;
        sinkRoutineRunning = false;

        if (goingToPoint)
        {
            movingParticles.Play();
            if (movingParticlesEntity2 != null) movingParticles2.Play();
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}