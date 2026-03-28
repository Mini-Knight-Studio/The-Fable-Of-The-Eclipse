using System;
using Loopie;

class PillarTrigger : Component
{
    private BoxCollider triggerZone;
    private PlayerGrapple playerGrapple;

    private Entity interactionPrompt;

    void OnCreate()
    {
        triggerZone = entity.GetComponent<BoxCollider>();

        Entity playerEntity = Entity.FindEntityByName("Player");
        if (playerEntity != null)
        {
            playerGrapple = playerEntity.GetComponent<PlayerGrapple>();
        }

        interactionPrompt = entity.GetChild(0);

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        else
        {
            Debug.Log("PillarTrigger: No child entity found for interaction prompt!");
        }

    }

    void OnUpdate()
    {
        if (triggerZone == null || playerGrapple == null) return;

        if (triggerZone.HasCollided)
        {
            if (interactionPrompt != null) interactionPrompt.SetActive(true);
            playerGrapple.SetGrappleTarget(entity);

            Debug.Log("Interaction prompt visible.");
        }
        else if (triggerZone.HasEndedCollision)
        {
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
            playerGrapple.ClearGrappleTarget(entity);

            Debug.Log("Interaction prompt hidden.");
        }
    }
}