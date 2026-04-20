using System;
using Loopie;

public class PillarTrigger : Component
{
    private BoxCollider triggerZone;
    private PlayerGrapple playerGrapple;

    public Entity interactionPrompt;
    public Entity hookParticles;

    public float hookTravelTime = 0.5f;

    private bool isWaitingForHook = false;
    private float hookTimer = 0.0f;
    private bool isTargeted = false;

    void OnCreate()
    {
        triggerZone = entity.GetComponent<BoxCollider>();

        Entity playerEntity = Entity.FindEntityByName("Player");
        if (playerEntity != null)
        {
            playerGrapple = playerEntity.GetComponent<PlayerGrapple>();
        }

        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (hookParticles != null) hookParticles.SetActive(false);
    }

    void OnUpdate()
    {
        if (triggerZone == null || playerGrapple == null) return;

        if (triggerZone.HasCollided)
        {
            Entity sensor = Entity.FindEntityByName("PlayerGrappleBox");

            if (sensor != null && triggerZone.IsColliding)
            {
                isTargeted = true;
                if (interactionPrompt != null) interactionPrompt.SetActive(true);
            }
        }
        else if (triggerZone.HasEndedCollision)
        {
            isTargeted = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
            isWaitingForHook = false;
        }

        if (isTargeted && interactionPrompt != null && interactionPrompt.Active && Input.IsKeyPressed(KeyCode.I) && !isWaitingForHook)
        {
            isWaitingForHook = true;
            hookTimer = 0.0f;

            playerGrapple.RotateToTarget(entity.transform.position);
            playerGrapple.ExecuteGrapple(this, hookTravelTime);
        }

        if (isWaitingForHook)
        {
            hookTimer += Time.deltaTime;

            if (hookTimer >= hookTravelTime)
            {
                isWaitingForHook = false;
                if (hookParticles != null) hookParticles.SetActive(true);
            }
        }
    }

    public void ResetParticles()
    {
        if (hookParticles != null) hookParticles.SetActive(false);
    }
}