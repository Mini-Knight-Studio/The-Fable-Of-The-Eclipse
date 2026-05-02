using System;
using Loopie;

public class PillarTrigger : Component
{
    public Entity interactionPrompt;
    public Entity hookParticlesEntity;

    private ParticleComponent hookParticles;

    public float reachDistance = 10.0f;
    public float hookTravelTime = 0.5f;

    void OnCreate()
    {
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (hookParticlesEntity != null) hookParticles = hookParticlesEntity.GetComponent<ParticleComponent>();

        PlayerGrapple.RegisterPillar(this);
    }

    void OnDestroy()
    {
        PlayerGrapple.UnregisterPillar(this);
    }

    public void SetTargetedState(bool isTargeted)
    {
        if (interactionPrompt != null) 
            interactionPrompt.SetActive(isTargeted);
    }

    public void PlayHookParticles()
    {
        if (hookParticles != null) 
            hookParticles.Play();
    }

    public void ResetParticles()
    {
        if (hookParticles != null) 
            hookParticles.Stop();
    }

    public bool CheckLineOfSight(Vector3 playerPos)
    {
        Vector3 pillarPos = entity.transform.position;
        Vector3 dirToPlayer = (playerPos - pillarPos).normalized;
        float distance = (float)Vector3.Distance(pillarPos, playerPos);

        int layer = Collisions.GetLayerBit("WorldLimits") | Collisions.GetLayerBit("Enemy");

        Collisions.Raycast(pillarPos, dirToPlayer, distance, out RaycastHit hit, layer);
        return hit.entity == null;
    }

    void OnDrawGizmo()
    {
        Gizmo.DrawCircle(transform.position, reachDistance, Vector3.Up, 32, Color.Red);
    }
}