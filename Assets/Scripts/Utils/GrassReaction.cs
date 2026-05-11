using System;
using Loopie;

public class GrassReaction : Component
{
    [Header("Settings")]
    public float bendIntensity = 30.0f;
    public float recoverySpeed = 5.0f;
    public float detectionRadius = 2.0f;

    private Vector3 originalRotation;
    private Vector3 pushDirection = Vector3.Zero;
    private bool isPlayerInside = false;

    void OnCreate()
    {
        originalRotation = transform.rotation;

    }

    void OnUpdate()
    {
        float dist = (float)Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (dist < detectionRadius)
        {
            isPlayerInside = true;
            pushDirection = (transform.position - Player.Instance.transform.position).normalized;
        }
        else
        {
            isPlayerInside = false;
        }

        Vector3 targetPush = isPlayerInside ? pushDirection : Vector3.Zero;

        Vector3 targetRot = new Vector3(
            originalRotation.x + (targetPush.z * bendIntensity),
            originalRotation.y,
            originalRotation.z + (-targetPush.x * bendIntensity)
        );

        transform.rotation = Vector3.Lerp(transform.rotation, targetRot, Time.deltaTime * recoverySpeed);
    }
}