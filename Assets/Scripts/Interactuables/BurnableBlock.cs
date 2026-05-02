using System;
using System.Collections;
using Loopie;

public class BurnableBlock : Component
{
    [Header("Visuals")]
    public Entity visuals;

    [Header("References")]
    public Entity colliderEntity;
    public Entity audioSourceEntity;
    public Entity particlesEntity;

    private BoxCollider collider;
    private AudioSource source;
    private ParticleComponent particles;

    private bool isBurning = false;

    void OnCreate()
    {
        collider = colliderEntity.GetComponent<BoxCollider>();
        source = audioSourceEntity.GetComponent<AudioSource>();
        particles = particlesEntity.GetComponent<ParticleComponent>();
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        if (collider == null || isBurning) return;

        if (collider.IsColliding)
        {
            StartCoroutine(BurnSequence());
        }
    }

    private IEnumerator BurnSequence()
    {
        isBurning = true;
        if(source != null)
            source.Play();

        if(particles != null)
            particles.Play();

        float timer = 0.0f;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (particles != null)
            particles.Stop();

        collider.SetActive(false);
        visuals.SetActive(false);

        yield return new WaitForSeconds(3);
        entity.SetActive(false);
    }
}