using System;
using Loopie;

public class GrassReaction : Component
{
    [Header("Settings")]
    public float bendIntensity = 30.0f;
    public float recoverySpeed = 5.0f;
    public float detectionRadius = 2.0f;

    private Vector3 originalRotation;
    private Vector3 currentBendOffset = Vector3.Zero;

    void OnCreate()
    {
        originalRotation = transform.rotation;
    }

    void OnUpdate()
    {
        Vector3 targetPush = Vector3.Zero;

        if (Player.Instance != null)
        {
            float dist = (float)Vector3.Distance(transform.position, Player.Instance.transform.position);

            if (dist < detectionRadius && dist > 0.01f)
            {
                Vector3 pushDirection = (transform.position - Player.Instance.transform.position).normalized;

                float distanceFactor = 1.0f - (dist / detectionRadius);

                targetPush = pushDirection * distanceFactor;
            }
        }

        Vector3 targetBendOffset = new Vector3(
            targetPush.z * bendIntensity,
            0,
            -targetPush.x * bendIntensity
        );

        currentBendOffset = Vector3.Lerp(currentBendOffset, targetBendOffset, Time.deltaTime * recoverySpeed);

        transform.rotation = new Vector3(
            originalRotation.x + currentBendOffset.x,
            originalRotation.y,
            originalRotation.z + currentBendOffset.z
        );
    }
}