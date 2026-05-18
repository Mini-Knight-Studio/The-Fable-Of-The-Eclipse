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

    private ParticleComponent unactiveParticlesPriv;
    private ParticleComponent topParticlesPriv;
    private ParticleComponent dispersionParticlesPriv;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
        particles = entity.GetComponent<ParticleComponent>();
        riseSFX = entity.GetComponent<AudioSource>();

        if(unactiveParticles!=null)
            unactiveParticlesPriv = unactiveParticles.GetComponent<ParticleComponent>();
        if(topParticles != null)
            topParticlesPriv = topParticles.GetComponent<ParticleComponent>();
        if(dispersionParticles!=null)
            dispersionParticlesPriv = dispersionParticles.GetComponent<ParticleComponent>();

        SetActiveState(false);
        StartCoroutine(GeyserCycleRoutine());
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

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

            if (unactiveParticlesPriv != null) unactiveParticlesPriv.Stop();
            if (dispersionParticlesPriv != null) dispersionParticlesPriv.Play();

            StartCoroutine(TopParticlesRoutine());
        }
        else
        {
            particles.Stop();

            if (unactiveParticlesPriv != null) unactiveParticlesPriv.Play();
            if (dispersionParticlesPriv != null) dispersionParticlesPriv.Stop();
            if (topParticlesPriv != null) topParticlesPriv.Stop();
        }
    }

    IEnumerator TopParticlesRoutine()
    {
        yield return new WaitForSeconds(topParticlesTiming);

        if (isActive && topParticles != null)
        {
            topParticlesPriv.Play();
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}