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
    public bool counterClockwise = false;

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
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

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

    private Entity GetCollidedModel()
    {
        if (collider1 != null && collider1.IsColliding) return doublePlatformModel1 != null ? doublePlatformModel1 : platformModel;
        if (collider2 != null && collider2.IsColliding) return doublePlatformModel2;
        return null;
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

        float overflowTime = 0f;

        while (true)
        {
            if (GameManager.state == GameManager.GameState.PAUSE || isSinking)
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
            Vector3 targetRot = followPathRotations ? pathPoints[targetPoint].transform.rotation : startRot;

            float totalDistance = (float)(targetPos - startPos).magnitude;

            float totalTravelTime = movementSpeed > 0f ? totalDistance / movementSpeed : 0f;

            float elapsedTravelTime = overflowTime;

            if (totalTravelTime > 0f)
            {
                while (elapsedTravelTime < totalTravelTime)
                {
                    if (GameManager.state == GameManager.GameState.PAUSE || isSinking)
                    {
                        yield return null;
                        continue;
                    }

                    elapsedTravelTime += Time.deltaTime;

                    float t = elapsedTravelTime / totalTravelTime;
                    if (t > 1f) t = 1f;

                    Vector3 previousGroupPos = activeMovingEntity.transform.position;
                    Vector3 previousGroupRot = activeMovingEntity.transform.rotation;

                    Vector3 newGroupPos = Vector3.Lerp(startPos, targetPos, t);
                    Vector3 newGroupRot = followPathRotations ? Vector3.Lerp(startRot, targetRot, t) : startRot;

                    Entity collidedModel = GetCollidedModel();
                    Vector3 playerLocalPos = Vector3.Zero;

                    if (collidedModel != null)
                    {
                        Vector3 worldPivot = collidedModel.transform.position;
                        Vector3 toPlayer = Player.Instance.transform.position - worldPivot;

                        Vector3 groupForward = activeMovingEntity.transform.Forward;
                        Vector3 groupRight = activeMovingEntity.transform.Left;
                        Vector3 groupUp = activeMovingEntity.transform.Up;

                        playerLocalPos = new Vector3(
                            Vector3.Dot(toPlayer, groupRight),
                            Vector3.Dot(toPlayer, groupUp),
                            Vector3.Dot(toPlayer, groupForward)
                        );
                    }

                    activeMovingEntity.transform.position = newGroupPos;
                    activeMovingEntity.transform.rotation = newGroupRot;

                    if (collidedModel != null)
                    {
                        Vector3 newWorldPivot = collidedModel.transform.position;

                        Vector3 newGroupForward = activeMovingEntity.transform.Forward;
                        Vector3 newGroupRight = activeMovingEntity.transform.Left;
                        Vector3 newGroupUp = activeMovingEntity.transform.Up;

                        Vector3 newPlayerWorldPos = newWorldPivot +
                            (newGroupRight * playerLocalPos.x) +
                            (newGroupUp * playerLocalPos.y) +
                            (newGroupForward * playerLocalPos.z);

                        Player.Instance.transform.position = newPlayerWorldPos;

                        if (followPathRotations)
                        {
                            Vector3 deltaRot = newGroupRot - previousGroupRot;
                            Player.Instance.transform.rotation -= deltaRot;
                        }
                    }

                    if (elapsedTravelTime >= totalTravelTime)
                    {
                        break;
                    }

                    yield return null;
                }

                overflowTime = elapsedTravelTime - totalTravelTime;
            }
            else
            {
                overflowTime = 0f;
            }

            currentPoint = targetPoint;
            activeMovingEntity.transform.position = targetPos;
            activeMovingEntity.transform.rotation = targetRot;

            goingToPoint = false;
            movingParticles.Stop();
            if (movingParticlesEntity2 != null) movingParticles2.Stop();

            float pauseTime = pointPauseTimes[currentPoint];
            if (pauseTime > 0.0f)
            {
                float elapsedPauseTime = overflowTime;

                while (elapsedPauseTime < pauseTime)
                {
                    if (GameManager.state == GameManager.GameState.PAUSE || isSinking)
                    {
                        yield return null;
                        continue;
                    }

                    elapsedPauseTime += Time.deltaTime;
                    if (elapsedPauseTime >= pauseTime) break;

                    yield return null;
                }

                overflowTime = elapsedPauseTime - pauseTime;
                if (overflowTime < 0f) overflowTime = 0f;
            }

            if (activateOnCollision && currentPoint == 0)
            {
                isWaitingForPlayer = true;
                break;
            }

            if (looping || activateOnCollision)
            {
                if (counterClockwise)
                {
                    targetPoint = (currentPoint - 1 + numOfPathPoints) % numOfPathPoints;
                }
                else
                {
                    targetPoint = (currentPoint + 1) % numOfPathPoints;
                }

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
            if (GameManager.state == GameManager.GameState.PAUSE)
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

            if (CheckCollision())
            {
                Player.Instance.transform.position += velocity;
            }

            yield return null;
        }

        yield return new WaitForSeconds(timeSunken);

        while (true)
        {
            if (GameManager.state == GameManager.GameState.PAUSE)
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

            if (CheckCollision())
            {
                Player.Instance.transform.position += velocity;
            }

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