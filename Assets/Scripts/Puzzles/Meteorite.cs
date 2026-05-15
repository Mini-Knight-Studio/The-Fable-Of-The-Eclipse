using System;
using Loopie;

class Meteorite : Component
{
    [Header("Settings")]
    public float meteoriteTime = 2.0f;
    public float fallDistance = 25.0f;
    public int damage = 20;

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
    private float timer = 0.0f;
    private float totalElapsed = 0.0f;
    private bool isFalling = false;
    private bool hasDealtDamage = false;

    void OnCreate()
    {
        startPos = transform.position;
        targetPos = new Vector3(startPos.x, startPos.y - fallDistance, startPos.z);

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
            transform.position = Vector3.Lerp(startPos, targetPos, t);

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
        Vector3 groundPos = new Vector3(startPos.x, startPos.y - fallDistance, startPos.z);
        Gizmo.DrawCircle(groundPos, shakeRadius, Vector3.Up, 32, Color.Green);
    }
}