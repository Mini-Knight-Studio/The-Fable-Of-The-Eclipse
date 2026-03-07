using System;
using Loopie;

class MovingPillar : Component
{
    private BoxCollider zPlusCollider;
    private BoxCollider zMinusCollider;
    private BoxCollider xPlusCollider;
    private BoxCollider xMinusCollider;

    private BoxCollider zPlusTargetCheck;
    private BoxCollider zMinusTargetCheck;
    private BoxCollider xPlusTargetCheck;
    private BoxCollider xMinusTargetCheck;

    public string zPlusColliderName;
    public string zMinusColliderName;
    public string xPlusColliderName;
    public string xMinusColliderName;

    public string zPlusTargetName;
    public string zMinusTargetName;
    public string xPlusTargetName;
    public string xMinusTargetName;

    public float collisionCooldown = 2.0f;
    private float collisionTimer = 0.0f;

    public float movementSpeed = 2.0f;
    public float movementDistance = 3.0f;

    private Vector3 startPosition;
    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;

    private bool isMoving = false;
    private Vector3 targetPosition;

    public string goalPositionName;
    public float onGoalMovementDistance = 1.0f;
    public bool onGoalPosition = false;
    public bool onGoalCalled = false;
    private Vector3 goalPosition;

    public AudioSource slideSFX;

    void OnCreate()
    {
        zPlusCollider = Entity.FindEntityByName(zPlusColliderName).GetComponent<BoxCollider>();
        zMinusCollider = Entity.FindEntityByName(zMinusColliderName).GetComponent<BoxCollider>();
        xPlusCollider = Entity.FindEntityByName(xPlusColliderName).GetComponent<BoxCollider>();
        xMinusCollider = Entity.FindEntityByName(xMinusColliderName).GetComponent<BoxCollider>();

        zPlusTargetCheck = Entity.FindEntityByName(zPlusTargetName).GetComponent<BoxCollider>();
        zMinusTargetCheck = Entity.FindEntityByName(zMinusTargetName).GetComponent<BoxCollider>();
        xPlusTargetCheck = Entity.FindEntityByName(xPlusTargetName).GetComponent<BoxCollider>();
        xMinusTargetCheck = Entity.FindEntityByName(xMinusTargetName).GetComponent<BoxCollider>();

        goalPosition = Entity.FindEntityByName(goalPositionName).transform.position;

        slideSFX = entity.GetComponent<AudioSource>();
    }

    void OnUpdate()
    {
        if (entity.transform.position.x == goalPosition.x && entity.transform.position.z == goalPosition.z && !onGoalCalled)
        {
            StartGoalPosition();
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
        else
        {
            if (onGoalPosition) return;
            HandleCollision();
        }
    }

    void HandleCollision()
    {
        //if (collisionTimer < collisionCooldown)
        //{
        //    collisionTimer += Time.deltaTime;
        //    return;
        //}

        Vector3 pos = entity.transform.position;

        if (zPlusCollider.HasCollided && CanMove(zPlusTargetCheck))
        {
            StartMovement(pos + new Vector3(0, 0, movementDistance));
        }
        else if (zMinusCollider.HasCollided && CanMove(zMinusTargetCheck))
        {
            StartMovement(pos + new Vector3(0, 0, -movementDistance));
        }
        else if (xPlusCollider.HasCollided && CanMove(xPlusTargetCheck))
        {
            StartMovement(pos + new Vector3(movementDistance, 0, 0));
        }
        else if (xMinusCollider.HasCollided && CanMove(xMinusTargetCheck))
        {
            StartMovement(pos + new Vector3(-movementDistance, 0, 0));
        }

        //collisionTimer += Time.deltaTime;
    }

    bool CanMove(BoxCollider targetCollider)
    {
        targetCollider.entity.SetActive(true);
        bool blocked = targetCollider.HasCollided;
        targetCollider.entity.SetActive(false);
        return !blocked;
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
        //collisionTimer = 0;

        slideSFX.Play();
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

    void StartGoalPosition()
    {
        onGoalPosition = true;
        onGoalCalled = true;

        Vector3 pos = entity.transform.position;

        StartMovement(pos + new Vector3(0, -onGoalMovementDistance, 0));

        Debug.LogWarning("The pillar has reached its goal");
    }
};