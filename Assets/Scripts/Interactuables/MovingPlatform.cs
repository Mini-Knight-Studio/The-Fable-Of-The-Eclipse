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

    public Entity platformModel;
    private BoxCollider collider;

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

    public Entity movingParticlesEntity;
    private ParticleComponent movingParticles;

    public float movementSpeed = 2.0f;

    public bool moveOnStart = false;
    public bool looping = false;

    private bool goingToPoint = false;
    public int targetPoint = 0;

    private int currentPoint = 0;

    // Activate with player col
    public bool activateOnCollision = false;
    private bool isWaitingForPlayer = false;

    // Sinking platforms
    public bool canSink = false;
    public float timeBeforeSink = 2.0f;
    public float timeSunken = 3.0f;
    public float sinkDistance = 5.0f;
    public float sinkSpeed = 2.0f;

    private SinkState sinkState = SinkState.Normal;
    private float stepTimer = 0.0f;
    private float sunkenTimer = 0.0f;
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
        if (canSink)
        {
            ManageSinking();
        }

        if (sinkState == SinkState.Normal)
        {
            if (activateOnCollision && isWaitingForPlayer)
            {
                if (collider.IsColliding)
                {
                    isWaitingForPlayer = false;
                    GoToPoint((currentPoint + 1) % numOfPathPoints);
                }
            }
            else if (goingToPoint)
            {
                ManageMovement();
            }
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

        Vector3 currentPos = platformModel.transform.position;
        Vector3 targetPos = pathPoints[targetPoint].transform.position;

        Vector3 directionVec = targetPos - currentPos;
        float distance = (float)directionVec.magnitude;

        if (distance < 0.1f)
        {
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
        platformModel.transform.position += moveDir * movementSpeed * Time.deltaTime;

        if (collider.IsColliding)
        {
            Player.Instance.transform.position += moveDir * movementSpeed * Time.deltaTime;
        }
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
                        positionBeforeSink = platformModel.transform.position;
                        //if (goingToPoint) movingParticles.Stop();
                    }
                }
                else
                {
                    stepTimer = 0.0f;
                }
                break;

            case SinkState.Sinking:
                Vector3 sinkTarget = positionBeforeSink - new Vector3(0, sinkDistance, 0);
                HandleVerticalMovement(sinkTarget, sinkSpeed, SinkState.Sunken);
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
                HandleVerticalMovement(positionBeforeSink, sinkSpeed, SinkState.Normal);
                break;
        }
    }

    void HandleVerticalMovement(Vector3 targetPos, float speed, SinkState nextState)
    {
        Vector3 directionVec = targetPos - platformModel.transform.position;
        float distance = (float)directionVec.magnitude;

        if (distance < 0.1f)
        {
            platformModel.transform.position = targetPos;
            sinkState = nextState;

            if (nextState == SinkState.Normal)
            {
                stepTimer = 0.0f;
                if (goingToPoint) movingParticles.Play();
            }
        }
        else
        {
            Vector3 moveDir = directionVec.normalized;
            Vector3 velocity = moveDir * speed * Time.deltaTime;
            platformModel.transform.position += velocity;

            //if (collider.IsColliding)
            //{
            //    Player.Instance.Movement.gravityActive = false;
            //}
        }
    }
};