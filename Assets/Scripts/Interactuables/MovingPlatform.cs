using System;
using Loopie;

class MovingPlatform : Component
{
    // Have a collider, if the player is colliding, make the player movement the same as the platform (or sum them or smthn)
    // PlatformObj --> script, have a minimum of points and an int public to set how many you are sing
    //  > model (collider)
    //  > points (from path, entities)
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

    public float movementSpeed = 2.0f;

    public bool moveOnStart = false;
    public bool looping = false;

    private bool goingToPoint = false;
    public int targetPoint = 0;

    private int currentPoint = 0;

    void OnCreate()
    {
        collider = platformModel.GetComponent<BoxCollider>();

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

        if (moveOnStart)
        {
            GoToPoint(0);
        }
    }

    void OnUpdate()
    {
        if (goingToPoint)
        {
            ManageMovement();
        }
    }

    public void GoToPoint(int pointNum)
    {
        if (pointNum < 0 || pointNum >= numOfPathPoints) return;

        goingToPoint = true;
        targetPoint = pointNum;
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

            if (looping)
            {
                GoToPoint((currentPoint + 1) % numOfPathPoints);
            }
            else
            {
                goingToPoint = false;
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
};