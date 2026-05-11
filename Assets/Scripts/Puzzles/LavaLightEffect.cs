using System;
using Loopie;

class LavaLightEffect : Component
{
    public float maxRotation = 45.0f;
    public float minRotation = -45.0f;
    public float velocity = 30.0f;

    private float startRotation;
    private float targetRotation;

    private float timer = 0.0f;
    private float duration = 0.0f;

    private bool isMoving = false;
    private bool goingToMax = true;

    void OnCreate()
    {
        startRotation = minRotation;
        targetRotation = maxRotation;
        StartMovement(targetRotation);
    }

    void OnUpdate()
    {
        if (isMoving)
        {
            MoveRotation();
        }
    }

    void StartMovement(float newTarget)
    {
        Vector3 rot = entity.transform.rotation;
        startRotation = rot.x;
        targetRotation = newTarget;

        float distance = Math.Abs(targetRotation - startRotation);
        duration = distance / velocity;

        timer = 0.0f;
        isMoving = true;
    }

    void MoveRotation()
    {
        timer += Time.deltaTime;

        float t = timer / duration;

        if (t >= 1.0f)
        {
            Vector3 rot = entity.transform.rotation;
            rot.x = targetRotation;
            entity.transform.rotation = rot;

            isMoving = false;

            goingToMax = !goingToMax;

            if (goingToMax)
                StartMovement(maxRotation);
            else
                StartMovement(minRotation);

            return;
        }

        float smoothT = t * t * (3f - 2f * t);

        float current = startRotation + (targetRotation - startRotation) * smoothT;

        Vector3 r = entity.transform.rotation;
        r.x = current;
        entity.transform.rotation = r;
    }
}