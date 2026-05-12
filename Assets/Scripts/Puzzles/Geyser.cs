using System;
using System.Collections;
using Loopie;

class Geyser : Component
{
    [Header("References")]
    public AudioSource riseSFX;

    private BoxCollider collider;
    private ParticleComponent particles;

    [Header("Settings")]
    public int damage = 1;
    public float damageCD = 2.0f;
    private float damageTimer = 0.0f;

    public bool alwaysActive = false;
    public float frequency = 3.0f;
    public float delay = 0.0f;

    private bool isActive = false;

    public float knockbackForce = 0.0f;
    public float knockbackDuration = 0.0f;

    [Header("Orientation")]
    public bool horizontal = false;

    [Header("Feedback")]
    public Entity unactiveParticles;
    public float topParticlesTiming = 0.0f;
    public Entity topParticles;
    public Entity dispersionParticles;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
        particles = entity.GetComponent<ParticleComponent>();
        riseSFX = entity.GetComponent<AudioSource>();

        SetActiveState(false);
        StartCoroutine(GeyserCycleRoutine());
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        damageTimer += Time.deltaTime;

        if (isActive && collider.IsColliding && damageTimer >= damageCD)
        {
            damageTimer = 0.0f;

            Player.Instance.PlayerHealth.Damage(damage);

            Vector3 knockbackDir = Player.Instance.transform.position - transform.position;
            if (horizontal)
            {
                knockbackDir = new Vector3(knockbackDir.x, 0, knockbackDir.z);
            }

            Player.Instance.Movement.ApplyKnockback(knockbackForce, knockbackDuration, knockbackDir);
        }
    }

    IEnumerator GeyserCycleRoutine()
    {
        if (delay > 0.0f)
        {
            yield return new WaitForSeconds(delay);
        }

        if (alwaysActive)
        {
            isActive = true;
            SetActiveState(true);
            yield break;
        }

        while (true)
        {
            yield return new WaitForSeconds(frequency);
            isActive = !isActive;
            SetActiveState(isActive);
        }
    }

    void SetActiveState(bool state)
    {
        collider.Enabled = state;

        if (state)
        {
            particles.Play();
            if (riseSFX != null) riseSFX.Play();

            if (unactiveParticles != null) unactiveParticles.GetComponent<ParticleComponent>().Stop();
            if (dispersionParticles != null) dispersionParticles.GetComponent<ParticleComponent>().Play();

            StartCoroutine(TopParticlesRoutine());
        }
        else
        {
            particles.Stop();

            if (unactiveParticles != null) unactiveParticles.GetComponent<ParticleComponent>().Play();
            if (dispersionParticles != null) dispersionParticles.GetComponent<ParticleComponent>().Stop();
            if (topParticles != null) topParticles.GetComponent<ParticleComponent>().Stop();
        }
    }

    IEnumerator TopParticlesRoutine()
    {
        yield return new WaitForSeconds(topParticlesTiming);

        if (isActive && topParticles != null)
        {
            topParticles.GetComponent<ParticleComponent>().Play();
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}