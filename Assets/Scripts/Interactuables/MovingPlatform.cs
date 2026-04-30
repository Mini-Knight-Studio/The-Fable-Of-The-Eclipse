using System;
using Loopie;

class MovingPlatform : Component
{
    public enum SinkState
    {
        Normal,
        Sinking,
        Sunken,
        Rising
    }

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

    [Header("Activate on Collision")]
    public bool activateOnCollision = false;
    private bool isWaitingForPlayer = false;

    [Header("Sinking Platform")]
    public bool canSink = false;
    public float timeBeforeSink = 2.0f;
    public float timeSunken = 3.0f;
    public float sinkDistance = 5.0f;
    public float sinkSpeed = 2.0f;

    private SinkState sinkState = SinkState.Normal;
    private float stepTimer = 0.0f;
    private float sunkenTimer = 0.0f;
    private Vector3 positionBeforeSink;

    private Vector3 logicalPosition;
    private float currentSinkOffset = 0.0f;

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

        logicalPosition = platformModel.transform.position;

        if (activateOnCollision)
        {
            isWaitingForPlayer = true;
            goingToPoint = false;
        }
        else if (moveOnStart)
        {
            GoToPoint(0);
        }
    }

    void OnUpdate()
    {
        Vector3 previousActualPos = platformModel.transform.position;

        if (canSink)
        {
            ManageSinking();
        }

        if (activateOnCollision && isWaitingForPlayer)
        {
            if (collider.IsColliding && sinkState == SinkState.Normal)
            {
                isWaitingForPlayer = false;
                GoToPoint((currentPoint + 1) % numOfPathPoints);
            }
        }
        else if (goingToPoint)
        {
            ManageMovement();
        }

        Vector3 newActualPos = logicalPosition - new Vector3(0, currentSinkOffset, 0);
        platformModel.transform.position = newActualPos;

        Vector3 platformDelta = newActualPos - previousActualPos;
        if (collider.IsColliding && platformDelta != Vector3.Zero && sinkState == SinkState.Normal)
        {
            Player.Instance.transform.position += platformDelta;
        }
    }

    public void GoToPoint(int pointNum)
    {
        if (pointNum < 0 || pointNum >= numOfPathPoints) return;

        goingToPoint = true;
        targetPoint = pointNum;
        movingParticles.Play();
    }

    void ManageMovement()
    {
        if (pathPoints[targetPoint] == null) return;

        Vector3 targetPos = pathPoints[targetPoint].transform.position;

        Vector3 directionVec = targetPos - logicalPosition;
        float distance = (float)directionVec.magnitude;

        if (distance < 0.2f)
        {
            logicalPosition = targetPos;
            currentPoint = targetPoint;

            if (activateOnCollision && currentPoint == 0)
            {
                goingToPoint = false;
                isWaitingForPlayer = true;
                movingParticles.Stop();
                return;
            }

            if (looping || activateOnCollision)
            {
                GoToPoint((currentPoint + 1) % numOfPathPoints);
            }
            else
            {
                goingToPoint = false;
                movingParticles.Stop();
            }

            return;
        }

        Vector3 moveDir = directionVec.normalized;
        logicalPosition += moveDir * movementSpeed * Time.deltaTime;
    }

    void ManageSinking()
    {
        switch (sinkState)
        {
            case SinkState.Normal:
                if (collider.IsColliding)
                {
                    Player.Instance.Camera.SetIsShaking(true, 0.1f, 0.05f, 0.05f);
                    stepTimer += Time.deltaTime;
                    if (stepTimer >= timeBeforeSink)
                    {
                        sinkState = SinkState.Sinking;
                    }
                }
                else
                {
                    stepTimer = 0.0f;
                }
                break;

            case SinkState.Sinking:
                currentSinkOffset += sinkSpeed * Time.deltaTime;
                if (currentSinkOffset >= sinkDistance)
                {
                    currentSinkOffset = sinkDistance;
                    sinkState = SinkState.Sunken;
                }
                break;

            case SinkState.Sunken:
                sunkenTimer += Time.deltaTime;
                if (sunkenTimer >= timeSunken)
                {
                    sinkState = SinkState.Rising;
                    sunkenTimer = 0.0f;
                }
                break;

            case SinkState.Rising:
                currentSinkOffset -= sinkSpeed * Time.deltaTime;
                if (currentSinkOffset <= 0.0f)
                {
                    currentSinkOffset = 0.0f;
                    sinkState = SinkState.Normal;
                    stepTimer = 0.0f;
                }

                //if (collider.IsColliding)
                //{
                //    Player.Instance.Movement.gravityActive = false;
                //}
                break;
        }
    }
}