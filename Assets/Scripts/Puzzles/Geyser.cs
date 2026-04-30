using System;
using Loopie;

class Geyser : Component
{
    public enum TriggerState
    {
        Waiting,
        Delaying,
        Active
    }

    [Header("Settings")]
    public int damage = 1;
    public float damageCD = 2.0f;
    private float damageTimer = 0.0f;

    private BoxCollider collider;
    private ParticleComponent particles;
    private AudioSource riseSFX;

    public float frequency = 3.0f;
    public float delay = 0.0f;
    private float frequencyTimer = 0.0f;

    private bool isActive = false;

    public float knockbackForce = 0.0f;
    public float knockbackDuration = 0.0f;

    [Header("Activate on Trigger")]
    public bool activateOnTrigger = false;
    public Entity triggerColliderEntity;
    private BoxCollider triggerCollider;
    public float delayAfterColliding = 0.0f;

    private TriggerState triggerState = TriggerState.Waiting;
    private float triggerTimer = 0.0f;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
        particles = entity.GetComponent<ParticleComponent>();
        riseSFX = entity.GetComponent<AudioSource>();

        if (activateOnTrigger && triggerColliderEntity != null)
        {
            triggerCollider = triggerColliderEntity.GetComponent<BoxCollider>();
        }

        frequencyTimer = -delay;

        SetActiveState(false);
    }

    void OnUpdate()
    {
        damageTimer += Time.deltaTime;

        if (activateOnTrigger)
        {
            ManageTriggerMode();
        }
        else
        {
            ManageLoopingMode();
        }

        if (isActive && collider.IsColliding && damageTimer >= damageCD)
        {
            damageTimer = 0.0f;

            Player.Instance.PlayerHealth.Damage(damage);
            Player.Instance.Movement.ApplyKnockback(knockbackForce, knockbackDuration, Player.Instance.transform.position - transform.position);
        }
    }

    void ManageLoopingMode()
    {
        frequencyTimer += Time.deltaTime;

        if (frequencyTimer >= frequency)
        {
            frequencyTimer = 0.0f;
            isActive = !isActive;
            SetActiveState(isActive);
        }
    }

    void ManageTriggerMode()
    {
        if (triggerCollider == null) return;

        switch (triggerState)
        {
            case TriggerState.Waiting:
                if (triggerCollider.IsColliding)
                {
                    triggerState = TriggerState.Delaying;
                    triggerTimer = 0.0f;
                }
                break;

            case TriggerState.Delaying:
                triggerTimer += Time.deltaTime;
                if (triggerTimer >= delayAfterColliding)
                {
                    triggerState = TriggerState.Active;
                    triggerTimer = 0.0f;
                    isActive = true;
                    SetActiveState(true);
                }
                break;

            case TriggerState.Active:
                triggerTimer += Time.deltaTime;
                if (triggerTimer >= frequency)
                {
                    triggerState = TriggerState.Waiting;
                    triggerTimer = 0.0f;
                    isActive = false;
                    SetActiveState(false);
                }
                break;
        }
    }

    void SetActiveState(bool state)
    {
        collider.Enabled = state;

        if (state)
        {
            particles.Play();
            riseSFX.Play();
        }
        else
        {
            particles.Stop();
        }
    }
}