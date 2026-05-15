using System;
using Loopie;

class Meteorite : Component
{
    [Header("Settings")]
    public float meteoriteTime = 2.0f;
    public float fallDistance = 25.0f;
    public int damage = 20;
    public bool goingUp = false;
    public bool useCurve = false;

    [Header("Curve Tweaks")]
    public float curveArcHeight = 15f;
    public float curveHorizontalOffset = 10f;

    [Header("Collision & Radius")]
    public BoxCollider MeteoriteTrigger;
    public float shakeRadius = 12.0f;

    [Header("Wiggle Effect")]
    public float wiggleSpeed = 10.0f;
    public float wiggleIntensity = 15.0f;

    [Header("Juice / Feedback")]
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.3f;
    public float shakeRotation = 0.2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 controlPos;
    private float timer = 0.0f;
    private float totalElapsed = 0.0f;
    private bool isFalling = false;
    private bool hasDealtDamage = false;

    void OnCreate()
    {
        startPos = transform.position;

        float finalY = goingUp ? startPos.y + fallDistance : startPos.y - fallDistance;

        if (goingUp && useCurve)
        {
            targetPos = new Vector3(startPos.x + curveHorizontalOffset, finalY, startPos.z);
            controlPos = new Vector3(startPos.x, startPos.y + fallDistance + curveArcHeight, startPos.z);
        }
        else
        {
            targetPos = new Vector3(startPos.x, finalY, startPos.z);
        }

        if (MeteoriteTrigger == null)
            MeteoriteTrigger = entity.GetComponent<BoxCollider>();

        if (MeteoriteTrigger != null)
        {
            MeteoriteTrigger.Trigger = true;
            MeteoriteTrigger.entity.SetActive(true);
        }

        isFalling = true;
        timer = 0.0f;
        totalElapsed = 0.0f;
        hasDealtDamage = false;
    }

    void OnUpdate()
    {
        if (!isFalling) return;

        totalElapsed += Time.deltaTime;

        if (timer < meteoriteTime)
        {
            timer += Time.deltaTime;
            float t = timer / meteoriteTime;

            if (goingUp && useCurve)
            {
                float u = 1f - t;
                transform.position = (startPos * (u * u)) + (controlPos * (2f * u * t)) + (targetPos * (t * t));
            }
            else
            {
                transform.position = Vector3.Lerp(startPos, targetPos, t);
            }

            if (!hasDealtDamage && MeteoriteTrigger != null && MeteoriteTrigger.HasCollided)
            {
                ApplyDamageOnce();
            }

            float angle = Mathf.Sin(totalElapsed * wiggleSpeed) * wiggleIntensity;
            transform.rotation = new Vector3(0, 0, angle);
        }
        else
        {
            isFalling = false;
            Impact();
        }
    }

    private void ApplyDamageOnce()
    {
        if (Player.Instance != null && Player.Instance.PlayerHealth != null)
        {
            Player.Instance.PlayerHealth.Damage(damage);
        }

        hasDealtDamage = true;
    }

    void Impact()
    {
        if (Player.Instance != null)
        {
            float distanceToPlayer = (float)Vector3.Distance(transform.position, Player.Instance.transform.position);

            if (Player.Instance.Camera != null && distanceToPlayer <= shakeRadius)
            {
                Player.Instance.Camera.SetIsShaking(true, shakeDuration, shakeAmount, shakeRotation);
            }
        }

        entity.Destroy();
    }

    void OnDrawGizmo()
    {
        float finalY = goingUp ? startPos.y + fallDistance : startPos.y - fallDistance;
        Vector3 groundPos = (goingUp && useCurve) ? new Vector3(startPos.x + curveHorizontalOffset, finalY, startPos.z) : new Vector3(startPos.x, finalY, startPos.z);
        Gizmo.DrawCircle(groundPos, shakeRadius, Vector3.Up, 32, Color.Green);

        if (goingUp && useCurve)
        {
            Vector3 peakPos = new Vector3(startPos.x, startPos.y + fallDistance + curveArcHeight, startPos.z);
            Gizmo.DrawLine(startPos, peakPos, Color.Orange);
            Gizmo.DrawLine(peakPos, groundPos, Color.Orange);
        }
    }
}