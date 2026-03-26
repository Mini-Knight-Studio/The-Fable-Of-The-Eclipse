using System;
using System.IO;
using Loopie;
using Newtonsoft.Json;

class MovingPillar : Component
{
    public float movementSpeed = 2.0f;
    public float movementDistance = 3.0f;

    private Vector3 startPosition;
    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;

    private bool isMoving = false;
    private Vector3 targetPosition;

    public Entity goalEntity;
    public float onGoalMovementDistance = 1.0f;
    public bool onGoalPosition = false;
    public bool onGoalCalled = false;

    private float rayDistance = 2.0f;

    private BoxCollider myCollider;
    private BoxCollider goalCollider;

    public AudioSource slideSFX;

    public Entity playerCamera;
    public float cameraShakeDuration = 0.5f;
    public float cameraShakeAmount = 0.3f;
    public float cameraShakeRotation = 0.3f;

    void OnCreate()
    {
        myCollider = entity.GetComponent<BoxCollider>();
        goalCollider = goalEntity.GetComponent<BoxCollider>();

        slideSFX = entity.GetComponent<AudioSource>();
    }

    void OnUpdate()
    {
        HandleGoal();

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void HandleGoal()
    {
        if (onGoalCalled) return;

        Vector3 origin = entity.transform.position + myCollider.LocalCenter;
        origin.y -= ((myCollider.LocalExtents.y) * entity.transform.scale.y) + 0.01f;

        RaycastHit hit;

        if (Collisions.Raycast(origin, entity.transform.Down, rayDistance, out hit))
        {
            if(hit.collider.ID == goalCollider.ID)
            {
                StartGoalPosition();
            }
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

        slideSFX.Play();
        playerCamera.GetComponent<PlayerCamera>().SetIsShaking(true, cameraShakeDuration, cameraShakeAmount, cameraShakeRotation);
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

        Vector3 finalPos = goalCollider.transform.position;
        finalPos.y = entity.transform.position.y -onGoalMovementDistance;

        StartMovement(finalPos);

        myCollider.Static = true;

        Debug.LogWarning("The pillar has reached its goal");
    }

    public void CompletePillarAuto()
    {
        Vector3 finalPos = goalCollider.transform.position;
        finalPos.y = entity.transform.position.y;

        transform.position = finalPos;
    }

    void OnDrawGizmo()
    {
        Vector3 origin = entity.transform.position + myCollider.LocalCenter;
        origin.y -= ((myCollider.LocalExtents.y) * entity.transform.scale.y) - 0.01f;

        Vector3 lineEnd = origin;
        lineEnd.y -= rayDistance;

        Gizmo.DrawLine(origin, lineEnd, Color.Magenta);
    }
};