using System;
using Loopie;

class Geyser : Component
{
    public float movementSpeed = 2.0f;

    public float frequency = 3.0f;
    private float frequencyTimer = 0.0f;

    public float height = 5.0f;

    private Vector3 startPosition;
    private Vector3 upPosition;

    private Vector3 moveStart;
    private Vector3 moveTarget;

    private float moveTimer = 0.0f;
    private float moveDuration = 0.0f;

    private bool isMoving = false;
    private bool isUp = false;

    public AudioSource riseSFX;

    void OnCreate()
    {
        riseSFX = entity.GetComponent<AudioSource>();

        startPosition = entity.transform.position;
        upPosition = startPosition + new Vector3(0, height, 0);
    }

    void OnUpdate()
    {
        frequencyTimer += Time.deltaTime;

        if (frequencyTimer >= frequency && !isMoving)
        {
            frequencyTimer = 0.0f;

            if (isUp)
            {
                StartMovement(startPosition);
            }
            else
            {
                StartMovement(upPosition);
            }

            isUp = !isUp;
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void StartMovement(Vector3 newTarget)
    {
        moveStart = entity.transform.position;
        moveTarget = newTarget;

        float distance = (float)(moveTarget - moveStart).magnitude;

        moveDuration = distance / movementSpeed;
        moveTimer = 0.0f;

        isMoving = true;

        riseSFX.Play();
    }

    void MoveTowardsTarget()
    {
        moveTimer += Time.deltaTime;

        float t = moveTimer / moveDuration;

        if (t >= 1.0f)
        {
            entity.transform.position = moveTarget;
            isMoving = false;
            return;
        }

        float smoothT = t * t * (3f - 2f * t);

        entity.transform.position = Vector3.Lerp(moveStart, moveTarget, smoothT);
    }
}