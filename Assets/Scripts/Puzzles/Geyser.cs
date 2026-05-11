using System;
using Loopie;

class Geyser : Component
{
    public int damage = 1;
    public float damageCD = 2.0f;
    private float damageTimer = 0.0f;

    private BoxCollider collider;
    private ParticleComponent particles;
    public AudioSource riseSFX;

    public float frequency = 3.0f;
    public float delay = 0.0f;
    private float frequencyTimer = 0.0f;

    private bool isActive = false;

    public float knockbackForce = 0.0f;
    public float knockbackDuration = 0.0f;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
        particles = entity.GetComponent<ParticleComponent>();
        riseSFX = entity.GetComponent<AudioSource>();

        frequencyTimer = -delay;

        SetActiveState(false);
    }

    void OnUpdate()
    {
        frequencyTimer += Time.deltaTime;
        damageTimer += Time.deltaTime;

        if (frequencyTimer >= frequency)
        {
            frequencyTimer = 0.0f;

            isActive = !isActive;
            SetActiveState(isActive);
        }

        if (isActive && collider.IsColliding && damageTimer >= damageCD)
        {
            damageTimer = 0.0f;

            Player.Instance.PlayerHealth.Damage(damage);
            Player.Instance.Movement.ApplyKnockback(
                knockbackForce,
                knockbackDuration,
                Player.Instance.transform.position - transform.position
            );
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