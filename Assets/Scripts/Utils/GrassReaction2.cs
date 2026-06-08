using System;
using Loopie;

public class GrassReaction2 : Component
{
    private float bendIntensity = 14;
    private float recoverySpeed = 5.0f;
    private float detectionRadius = 2.0f;

    private bool enableWind = true;
    private float windSpeed = .5f;
    private float windStrength = 3.5f;
    private float windGustiness = 0.15f;
    private float windSpatialScale = 0.08f;

    private float grassVibrationIntensity = 0.02f;
    private float grassVibrationDuration = 0.02f;

    private Vector3 originalRotation;
    private Vector3 currentBendOffset = Vector3.Zero;
    private bool isPlayerInside = false;
    private float windTimer = 0.0f;

    void OnCreate()
    {
        originalRotation = transform.rotation;
    }

    void OnUpdate()
    {
        Vector3 targetPush = Vector3.Zero;
        bool beingTouched = false;

        if (Player.Instance != null)
        {
            float dist = (float)Vector3.Distance(transform.position, Player.Instance.transform.position);

            if (dist < detectionRadius && dist > 0.01f)
            {
                beingTouched = true;
                Vector3 pushDirection = (transform.position - Player.Instance.transform.position).normalized;
                float distanceFactor = 1.0f - (dist / detectionRadius);
                targetPush = pushDirection * distanceFactor;

                if (!isPlayerInside)
                {
                    isPlayerInside = true;
                    Input.StartShake(grassVibrationIntensity, grassVibrationDuration);
                }
            }
            else
            {
                isPlayerInside = false;
            }
        }

        float worldTargetBendX = 0f;
        float worldTargetBendZ = 0f;

        if (beingTouched)
        {
            worldTargetBendX = targetPush.z * bendIntensity;
            worldTargetBendZ = -targetPush.x * bendIntensity;
        }
        else if (enableWind)
        {
            windTimer += Time.deltaTime * windSpeed;

            float spatialOffset = (transform.position.x + transform.position.z) * windSpatialScale;
            float localWindTime = windTimer + spatialOffset;

            float primaryWave = MathF.Sin(localWindTime);
            float secondaryGust = MathF.Sin(localWindTime * 2.5f) * windGustiness;
            float windEffect = (primaryWave + secondaryGust + 1.0f) * 0.5f;

            worldTargetBendX = 0f;
            worldTargetBendZ = -windEffect * windStrength;
        }

        float angleRad = originalRotation.y * (MathF.PI / 180.0f);
        float cos = MathF.Cos(angleRad);
        float sin = MathF.Sin(angleRad);

        Vector3 localTargetBendOffset = new Vector3(worldTargetBendX * cos - worldTargetBendZ * sin, 0, worldTargetBendX * sin + worldTargetBendZ * cos);

        currentBendOffset = Vector3.Lerp(currentBendOffset, localTargetBendOffset, Time.deltaTime * recoverySpeed);

        transform.rotation = new Vector3(originalRotation.x + currentBendOffset.x, originalRotation.y, originalRotation.z + currentBendOffset.z);
    }
}