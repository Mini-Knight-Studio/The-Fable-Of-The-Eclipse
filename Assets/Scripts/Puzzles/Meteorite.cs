using System;
using Loopie;

class Meteorite : Component
{
    [Header("Settings")]
    public float meteoriteTime = 2.0f;
    public float fallDistance = 25.0f;

    [Header("Wiggle Effect")]
    public float wiggleSpeed = 10.0f;
    public float wiggleIntensity = 15.0f;

    [Header("Juice / Feedback")]
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.3f;
    public float shakeRotation = 0.2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float timer = 0.0f;
    private float totalElapsed = 0.0f;
    private bool isFalling = false;

    void OnCreate()
    {
        startPos = transform.position;
        targetPos = new Vector3(startPos.x, startPos.y - fallDistance, startPos.z);

        isFalling = true;
        timer = 0.0f;
        totalElapsed = 0.0f;
    }

    void OnUpdate()
    {
        if (!isFalling) return;

        totalElapsed += Time.deltaTime;

        if (timer < meteoriteTime)
        {
            timer += Time.deltaTime;
            float t = timer / meteoriteTime;

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            float angle = Mathf.Sin(totalElapsed * wiggleSpeed) * wiggleIntensity;

            transform.rotation = new Vector3(0, 0, angle);
        }
        else
        {
            isFalling = false;
            Impact();
        }
    }

    void Impact()
    {
        if (Player.Instance != null && Player.Instance.Camera != null)
        {
            Player.Instance.Camera.SetIsShaking(true, shakeDuration, shakeAmount, shakeRotation);
        }

        entity.Destroy();
    }
};