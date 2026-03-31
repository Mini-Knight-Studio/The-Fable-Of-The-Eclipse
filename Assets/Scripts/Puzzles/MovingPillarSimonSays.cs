using System;
using Loopie;

class MovingPillarSimonSays : Component
{
    private bool enabled = false;

    public bool active = false;
    private bool activated = false;

    private bool locked = true;

    public Entity torch;
    private ParticleComponent torchParticles;
    private BoxCollider torchCollider;

    void OnCreate()
    {
        if (torch != null)
        {
            torchParticles = torch.GetComponent<ParticleComponent>();
            torchCollider = torch.GetComponent<BoxCollider>();
        }
    }

    void OnUpdate()
    {
        var movingPillar = entity.GetComponent<MovingPillar>();

        if (movingPillar == null || !movingPillar.onGoalPosition)
        {
            return;
        }
        else if (!enabled)
        {
            if (torch != null) torch.SetActive(true);
            if (torchParticles != null) torchParticles.SetActive(false);
            enabled = true;
        }

        HandleActivation();
    }

    void HandleActivation()
    {
        if (torchCollider != null && torchCollider.IsColliding && !locked && Input.IsKeyPressed(KeyCode.E))
        {
            active = true;
        }

        if (active && !activated)
        {
            if (torchParticles != null) torchParticles.SetActive(true);
            activated = true;
        }
        else
        {
            if (!active && activated)
            {
                if (torchParticles != null) torchParticles.SetActive(false);
                activated = false;
            }
        }
    }

    public void ForceActive()
    {
        active = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    public void Lock()
    {
        locked = true;
    }
};