using System;
using Loopie;

public class PillarTrigger : Component
{
    private BoxCollider triggerZone;
    private PlayerGrapple playerGrapple;

    public Entity interactionPrompt;
    public Entity hookParticles;

    public float hookTravelTime = 0.25f;
    private bool isWaitingForHook = false;
    private float hookTimer = 0.0f;

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
            if (interactionPrompt != null) interactionPrompt.SetActive(true);
        }
        else if (triggerZone.HasEndedCollision)
        {
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
            isWaitingForHook = false;
        }

        if (interactionPrompt != null && interactionPrompt.Active && Input.IsKeyPressed(KeyCode.I) && !isWaitingForHook && DatabaseRegistry.playerDB.Player.hasGrappling)
        {
            isWaitingForHook = true;
            hookTimer = 0.0f;
            playerGrapple.RotateToTarget(entity.transform.position);
        }

        if (isWaitingForHook)
        {
            hookTimer += Time.deltaTime;
            if (hookTimer >= hookTravelTime)
            {
                isWaitingForHook = false;
                if (hookParticles != null) hookParticles.SetActive(true);
                playerGrapple.ExecuteGrapple(this);
            }
        }
    }

    public void ResetParticles()
    {
        if (hookParticles != null) hookParticles.SetActive(false);
    }
}