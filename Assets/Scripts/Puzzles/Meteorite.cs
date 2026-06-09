using System;
using System.Collections;
using Loopie;

class Meteorite : Component
{
    [Header("Settings")]
    public float meteoriteTime = 2.0f;
    public float fallDistance = 25.0f;
    public int damage = 20;
    public bool goingUp = false;
    public bool useCurve = false;
    public bool autoStart = false;

    [Header("Curve Tweaks")]
    public float curveArcHeight = 15f;
    public float curveHorizontalOffset = 10f;

    public BoxCollider MeteoriteTrigger;
    public float shakeRadius = 12.0f;

    [Header("Wiggle Effect")]
    public float wiggleSpeed = 10.0f;
    public float wiggleIntensity = 15.0f;

    [Header("Juice / Feedback")]
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.3f;
    public float shakeRotation = 0.2f;

    [Header("Explosion VFX")]
    public Entity explosionPrefab;
    public float explosionDuration = 1.0f;
    public float growDuration = 0.2f;
    public float finalScale = 2.0f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 controlPos;
    private float timer = 0.0f;
    private float totalElapsed = 0.0f;
    private bool isFalling = false;
    private bool hasDealtDamage = false;
    private bool isInitialized = false;

    public bool inUse = false;

    void OnCreate()
    {
        if (MeteoriteTrigger == null)
            MeteoriteTrigger = entity.GetComponent<BoxCollider>();

        if (autoStart)
        {
            Fire(transform.position, transform.rotation);
        }
    }

    public void Fire(Vector3 spawnPosition, Vector3 spawnRotation)
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        inUse = true;
        entity.SetActive(true);

        if (MeteoriteTrigger != null)
        {
            MeteoriteTrigger.Trigger = true;
            MeteoriteTrigger.entity.SetActive(true);
        }

        isFalling = true;
        timer = 0.0f;
        totalElapsed = 0.0f;
        hasDealtDamage = false;
        isInitialized = false;
    }

    void OnUpdate()
    {
        if (!isFalling) return;

        if (!isInitialized)
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
            isInitialized = true;
        }

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
            Player.Instance.PlayerHealth.Damage(damage);
        hasDealtDamage = true;
    }

    void Impact()
    {
        if (Player.Instance != null)
        {
            float distanceToPlayer = (float)Vector3.Distance(transform.position, Player.Instance.transform.position);
            if (Player.Instance.Camera != null && distanceToPlayer <= shakeRadius)
                Player.Instance.Camera.SetIsShaking(true, shakeDuration, shakeAmount, shakeRotation);
        }

        if (explosionPrefab != null)
        {
            Entity explosionInstance = explosionPrefab.Clone(true);
            explosionInstance.transform.position = transform.position;
            explosionInstance.transform.scale = new Vector3(0.1f, 0.1f, 0.1f);
            explosionInstance.SetActive(true);

            StartCoroutine(ExplosionEffectRoutine(explosionInstance));
        }

        StartCoroutine(DeactivateRoutine());
    }

    private IEnumerator ExplosionEffectRoutine(Entity explosionEntity)
    {
        float elapsed = 0f;
        Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 endScale = new Vector3(finalScale, finalScale, finalScale);

        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / growDuration;
            explosionEntity.transform.scale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        yield return new WaitForSeconds(explosionDuration - growDuration);

        if (explosionEntity != null)
        {
            explosionEntity.Destroy();
        }
    }

    private IEnumerator DeactivateRoutine()
    {
        if (MeteoriteTrigger != null) MeteoriteTrigger.entity.SetActive(false);
        //transform.position = new Vector3(0, 0, 0);

        yield return null;

        inUse = false;
        entity.SetActive(false);
    }
}