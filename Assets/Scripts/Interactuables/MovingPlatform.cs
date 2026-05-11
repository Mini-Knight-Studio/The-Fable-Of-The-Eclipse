using System;
using System.Collections;
using Loopie;

class MovingPlatform : Component
{
    [Header("Model")]
    public Entity platformModel;
    private BoxCollider collider;

    [Header("Path Points")]
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
        collider = platformModel.GetComponent<BoxCollider>();
        movingParticles = movingParticlesEntity.GetComponent<ParticleComponent>();
        movingParticles.Stop();

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
        else if (moveOnStart)
        {
            GoToPoint(targetPoint);
        }
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        if (canSink && !sinkRoutineRunning)
        {
            if (collider.IsColliding)
            {
                StartCoroutine(SinkRoutine());
            }
        }

        if (!isSinking)
        {
            if (activateOnCollision && isWaitingForPlayer)
            {
                if (collider.IsColliding)
                {
                    isWaitingForPlayer = false;
                    GoToPoint((currentPoint + 1) % numOfPathPoints);
                }
            }
        }
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

            Vector3 currentPos = platformModel.transform.position;
            Vector3 targetPos = pathPoints[targetPoint].transform.position;

            Vector3 directionVec = targetPos - currentPos;
            float distance = (float)directionVec.magnitude;

            if (distance < 0.1f)
            {
                currentPoint = targetPoint;

                platformModel.transform.position = targetPos;

                goingToPoint = false;

                movingParticles.Stop();

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

                    yield return null;
                    continue;
                }

                break;
            }

            Vector3 moveDir = directionVec.normalized;
            Vector3 velocity = moveDir * movementSpeed * Time.deltaTime;

            platformModel.transform.position += velocity;

            if (collider.IsColliding)
            {
                Player.Instance.transform.position += velocity;
            }

            yield return null;
        }

        movementRoutineRunning = false;
    }

    IEnumerator SinkRoutine()
    {
        sinkRoutineRunning = true;

        Player.Instance.Camera.SetIsShaking(true, timeBeforeSink*1.5f, 0.05f, 0.05f, 50f, 50f);

        yield return new WaitForSeconds(timeBeforeSink);

        if (!collider.IsColliding)
        {
            Player.Instance.Camera.SetIsShaking(false);
            sinkRoutineRunning = false;
            yield break;
        }

        isSinking = true;

        positionBeforeSink = platformModel.transform.position;

        Vector3 sinkTarget = positionBeforeSink - new Vector3(0, sinkDistance, 0);

        while (true)
        {
            if (Pause.isPaused)
            {
                yield return null;
                continue;
            }

            Vector3 directionVec = sinkTarget - platformModel.transform.position;
            float distance = (float)directionVec.magnitude;

            if (distance < 0.1f)
            {
                platformModel.transform.position = sinkTarget;
                break;
            }

            Vector3 moveDir = directionVec.normalized;
            Vector3 velocity = moveDir * sinkSpeed * Time.deltaTime;

            platformModel.transform.position += velocity;

            //if (collider.IsColliding)
            //{
            //    Player.Instance.Movement.gravityActive = false;
            //}

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

            Vector3 directionVec = positionBeforeSink - platformModel.transform.position;
            float distance = (float)directionVec.magnitude;

            if (distance < 0.1f)
            {
                platformModel.transform.position = positionBeforeSink;
                break;
            }

            Vector3 moveDir = directionVec.normalized;
            Vector3 velocity = moveDir * sinkSpeed * Time.deltaTime;

            platformModel.transform.position += velocity;

            yield return null;
        }

        isSinking = false;
        sinkRoutineRunning = false;

        if (goingToPoint)
        {
            movingParticles.Play();
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}