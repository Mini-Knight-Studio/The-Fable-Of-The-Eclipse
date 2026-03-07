using System;
using Loopie;

class MovingPillar : Component
{
    private BoxCollider zPlusCollider;
    private BoxCollider zMinusCollider;
    private BoxCollider xPlusCollider;
    private BoxCollider xMinusCollider;

    public string zPlusColliderName;
    public string zMinusColliderName;
    public string xPlusColliderName;
    public string xMinusColliderName;

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
        Vector3 pos = entity.transform.position;

        if (zPlusCollider.HasCollided && CanMove(entity.transform.Forward))
        {
            StartMovement(pos + new Vector3(0, 0, movementDistance));
        }
        else if (zMinusCollider.HasCollided && CanMove(entity.transform.Back))
        {
            StartMovement(pos + new Vector3(0, 0, -movementDistance));
        }
        else if (xPlusCollider.HasCollided && CanMove(entity.transform.Right))
        {
            StartMovement(pos + new Vector3(movementDistance, 0, 0));
        }
        else if (xMinusCollider.HasCollided && CanMove(entity.transform.Left))
        {
            StartMovement(pos + new Vector3(-movementDistance, 0, 0));
        }
    }

    bool CanMove(Vector3 direction)
    {
        BoxCollider collider = null;
        if (direction.x > 0) collider = xMinusCollider;
        else if (direction.x < 0) collider = xPlusCollider;
        else if (direction.z > 0) collider = zMinusCollider;
        else if (direction.z < 0) collider = zPlusCollider;

        if (collider == null) return false;

        Vector3 origin = entity.transform.position + collider.LocalCenter;
        if (direction.x > 0) origin += new Vector3(collider.LocalExtents.x, 0, 0);
        else if (direction.x < 0) origin -= new Vector3(collider.LocalExtents.x, 0, 0);
        else if (direction.z > 0) origin += new Vector3(0, 0, collider.LocalExtents.z);
        else if (direction.z < 0) origin -= new Vector3(0, 0, collider.LocalExtents.z);

        const float epsilon = 0.01f;
        origin += direction.normalized * epsilon;

        float rayDistance = movementDistance;
        if (direction.x != 0) rayDistance -= collider.LocalExtents.x;
        else if (direction.z != 0) rayDistance -= collider.LocalExtents.z;

        rayDistance += epsilon;

        RaycastHit hit;
        if (Collisions.Raycast(origin, direction.normalized, rayDistance, out hit))
        {
            return false;
        }

        return true;
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