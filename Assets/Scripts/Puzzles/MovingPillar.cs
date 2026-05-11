using System;
using Loopie;

class MovingPillar : Component
{
    public float movementSpeed = 2.0f;
    public float tileSize = 1.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;

    private bool isMoving = false;

    public Entity goalEntity;
    public float onGoalMovementDistance = 1.0f;
    public bool onGoalPosition = false;
    public bool onGoalCalled = false;
    private bool stopedOnGoal = false;

    private BoxCollider myCollider;
    private BoxCollider goalCollider;

    [HideInInspector]
    public AudioSource slideSFX;

    public float cameraShakeDuration = 0.5f;
    public float cameraShakeAmount = 0.3f;
    public float cameraShakeRotation = 0.3f;

    // Side colliders
    public Entity pushForwardEntity;
    public Entity pushBackEntity;
    public Entity pushLeftEntity;
    public Entity pushRightEntity;

    private BoxCollider pushForward;
    private BoxCollider pushBack;
    private BoxCollider pushLeft;
    private BoxCollider pushRight;

    private float pushTimerForward = 0.0f;
    private float pushTimerBack = 0.0f;
    private float pushTimerLeft = 0.0f;
    private float pushTimerRight = 0.0f;

    public float pushTimeRequired = 1.25f;

    // Particles
    private ParticleComponent goalParticles;
    public Entity movingParticlesEntity;
    private ParticleComponent movingParticles;

    // For reseting
    private Vector3 initialPosition;


    void OnCreate()
    {
        myCollider = entity.GetComponent<BoxCollider>();
        goalCollider = goalEntity.GetComponent<BoxCollider>();
        slideSFX = entity.GetComponent<AudioSource>();
        goalParticles = goalEntity.GetComponent<ParticleComponent>();
        goalParticles.Stop();
        movingParticles = movingParticlesEntity.GetComponent<ParticleComponent>();
        movingParticles.Stop();

        pushForward = pushForwardEntity.GetComponent<BoxCollider>();
        pushBack = pushBackEntity.GetComponent<BoxCollider>();
        pushLeft = pushLeftEntity.GetComponent<BoxCollider>();
        pushRight = pushRightEntity.GetComponent<BoxCollider>();

        initialPosition = entity.transform.position;
    }

    void OnUpdate()
    {
        HandleGoal();

        if (isMoving)
        {
            MoveTowardsTarget();
        }
        else
        {
            HandlePushColliders();
            if (onGoalCalled && !stopedOnGoal)
            {
                stopedOnGoal = true;
                goalParticles.Stop();
            }
        }
    }

    void HandlePushColliders()
    {
        if (pushForward.IsColliding)
        {
            HandlePush(pushForward, transform.Forward, ref pushTimerForward);
        }
        else if (pushBack.IsColliding)
        {
            HandlePush(pushBack, transform.Back, ref pushTimerBack);
        }
        else if (pushLeft.IsColliding)
        {
            HandlePush(pushLeft, transform.Left, ref pushTimerLeft);
        }
        else if (pushRight.IsColliding)
        {
            HandlePush(pushRight, transform.Right, ref pushTimerRight);
        }
        else
        {
            pushTimerForward = 0.0f;
            pushTimerBack = 0.0f;
            pushTimerLeft = 0.0f;
            pushTimerRight = 0.0f;
        }
    }

    void HandlePush(BoxCollider col, Vector3 direction, ref float timer)
    {
        timer += Time.deltaTime;

        if (timer >= pushTimeRequired)
        {
            if (CanMove(direction))
            {
                TryMove(direction);
            }

            timer = 0.0f;
        }
    }

    public void TryMove(Vector3 direction)
    {
        if (isMoving || onGoalPosition) return;

        direction.y = 0;
        direction = direction.normalized;

        Vector3 target = entity.transform.position + direction * tileSize;
        target = SnapToGrid(target);

        StartMovement(target);
    }

    bool CanMove(Vector3 direction)
    {
        pushForward.SetActive(false);
        pushBack.SetActive(false);
        pushLeft.SetActive(false);
        pushRight.SetActive(false);

        direction = direction.normalized;

        Vector3 extents = myCollider.LocalExtents;
        Vector3 scale = entity.transform.scale;

        Vector3 worldExtents = new Vector3(extents.x * scale.x, extents.y * scale.y, extents.z * scale.z);

        Vector3 origin = entity.transform.position + myCollider.LocalCenter;
        origin += new Vector3(direction.x * worldExtents.x, direction.y * worldExtents.y, direction.z * worldExtents.z);

        origin += direction * 0.05f;

        RaycastHit hit;
        bool blocked = Collisions.Raycast(origin, direction, tileSize, out hit, Collisions.GetLayerBit("PillarLimits")| Collisions.GetLayerBit("WorldLimits") | Collisions.GetLayerBit("Pillars"));

        pushForward.SetActive(true);
        pushBack.SetActive(true);
        pushLeft.SetActive(true);
        pushRight.SetActive(true);

        return !blocked;
    }

    void StartMovement(Vector3 newTarget)
    {
        startPosition = entity.transform.position;
        targetPosition = newTarget;

        float distance = (float)(targetPosition - startPosition).magnitude;

        moveDuration = distance / movementSpeed;
        moveTimer = 0.0f;

        isMoving = true;

        slideSFX.Play();
        movingParticles.Play();
        Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount, cameraShakeRotation);
    }

    void MoveTowardsTarget()
    {
        moveTimer += Time.deltaTime;

        float t = moveTimer / moveDuration;

        if (t >= 1.0f)
        {
            entity.transform.position = targetPosition;
            isMoving = false;
            movingParticles.Stop();
            return;
        }

        entity.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    }

    void HandleGoal()
    {
        if (onGoalCalled) return;

        Vector3 origin = entity.transform.position + myCollider.LocalCenter;
        origin.y -= ((myCollider.LocalExtents.y) * entity.transform.scale.y) + 0.01f;

        RaycastHit hit;

        if (Collisions.Raycast(origin, entity.transform.Down, 2.0f, out hit))
        {
            if (hit.collider.ID == goalCollider.ID)
            {
                StartGoalPosition();
            }
        }
    }

    void StartGoalPosition()
    {
        onGoalPosition = true;
        onGoalCalled = true;

        Vector3 finalPos = goalCollider.transform.position;
        finalPos.y = entity.transform.position.y - onGoalMovementDistance;

        StartMovement(finalPos);

        myCollider.Static = true;

        Debug.LogWarning("The pillar has reached its goal");

        goalParticles.Play();
    }

    public void CompletePillarAuto()
    {
        Vector3 finalPos = goalCollider.transform.position;
        finalPos.y = entity.transform.position.y;

        entity.transform.position = finalPos;
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(
            (float)Math.Round(pos.x / tileSize) * tileSize,
            pos.y,
            (float)Math.Round(pos.z / tileSize) * tileSize
        );
    }

    public void ResetPillar()
    {
        StartMovement(initialPosition);
        onGoalPosition = false;
        onGoalCalled = false;
        stopedOnGoal = false;
}
}