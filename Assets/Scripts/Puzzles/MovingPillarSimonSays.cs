using System;
using Loopie;

class MovingPillarSimonSays : Component
{
    private bool enabled = false;

    public bool active = false;
    public bool wasPressed = false;

    private bool activated = false;
    private bool locked = true;

    public Entity torch;
    private ParticleComponent torchParticles;
    private BoxCollider torchCollider;

    private MovingPillar movingPillar;

    void OnCreate()
    {
        movingPillar = entity.GetComponent<MovingPillar>();

        if (torch != null)
        {
            torchParticles = torch.GetComponent<ParticleComponent>();
            torchCollider = torch.GetComponent<BoxCollider>();
        }
    }

    void OnUpdate()
    {
        wasPressed = false;

        if (movingPillar == null) return;
        if (!movingPillar.onGoalPosition) return;

        if (!enabled)
        {
            if (torch != null) torch.SetActive(true);
            if (torchParticles != null) torchParticles.SetActive(false);
            enabled = true;
        }

        HandleActivation();
    }

    void HandleActivation()
    {
        if (torchCollider != null && torchCollider.IsColliding && !locked && Input.IsKeyDown(KeyCode.E))
        {
            wasPressed = true;
            active = true;
        }

        if (active && !activated)
        {
            if (torchParticles != null) torchParticles.SetActive(true);
            activated = true;
        }
        else if (!active && activated)
        {
            if (torchParticles != null) torchParticles.SetActive(false);
            activated = false;
        }
    }

    public void ForceActive()
    {
        active = true;

        if (!activated)
        {
            if (torchParticles != null) torchParticles.SetActive(true);
            activated = true;
        }
    }

    public void ResetState()
    {
        active = false;
        wasPressed = false;

        if (activated)
        {
            if (torchParticles != null) torchParticles.SetActive(false);
            activated = false;
        }
    }

    public void Unlock()
    {
        locked = false;
    }

    public void Lock()
    {
        locked = true;
    }
}