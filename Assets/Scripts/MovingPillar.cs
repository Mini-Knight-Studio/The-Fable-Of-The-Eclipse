using System;
using Loopie;

class MovingPillar : Component
{
    private BoxCollider zPlusCollider;
    private BoxCollider zMinusCollider;
    private BoxCollider xPlusCollider;
    private BoxCollider xMinusCollider;

    private BoxCollider placeCheckerCollider;

    public string zPlusColliderName;
    public string zMinusColliderName;
    public string xPlusColliderName;
    public string xMinusColliderName;

    public string placeCheckerColliderName;

    public float collisionCooldown = 2.0f;
    private float collisionTimer = 0.0f;

    public float movementSpeed = 2.0f;
    public float movementDistance = 3.0f;

    private Vector3 startPosition;
    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;

    private bool isMoving = false;
    private Vector3 targetPosition;

    private bool onCorrectPlace = false;

    void OnCreate()
    {
        zPlusCollider = Entity.FindEntityByName(zPlusColliderName).GetComponent<BoxCollider>();
        zMinusCollider = Entity.FindEntityByName(zMinusColliderName).GetComponent<BoxCollider>();
        xPlusCollider = Entity.FindEntityByName(xPlusColliderName).GetComponent<BoxCollider>();
        xMinusCollider = Entity.FindEntityByName(xMinusColliderName).GetComponent<BoxCollider>();

        placeCheckerCollider = Entity.FindEntityByName(placeCheckerColliderName).GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (onCorrectPlace) return;

        if (isMoving)
        {
            MoveTowardsTarget();
        }
        else
        {
            HandleCollision();
        }
    }

    void HandleCollision()
    {
        if (collisionTimer < collisionCooldown)
        {
            collisionTimer += Time.deltaTime;
            return;
        }

        Vector3 pos = entity.transform.position;

        if (zPlusCollider.HasCollided)
            StartMovement(pos + new Vector3(0, 0, movementDistance));
        else if (zMinusCollider.HasCollided)
            StartMovement(pos + new Vector3(0, 0, -movementDistance));
        else if (xPlusCollider.HasCollided)
            StartMovement(pos + new Vector3(movementDistance, 0, 0));
        else if (xMinusCollider.HasCollided)
            StartMovement(pos + new Vector3(-movementDistance, 0, 0));

        collisionTimer += Time.deltaTime;
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
        collisionTimer = 0;
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
};