using System;
using System.Collections;
using Loopie;

public class PlayerFeedback : PlayerComponent
{
    [Header("Health - Audio")]
    [ShowInInspector] Entity onHurtAudioEntity;
    [ShowInInspector] Entity onHealAudioEntity;
    [ShowInInspector] Entity onDeathAudioEntity;

    [Header("Health - Particles")]
    [ShowInInspector] Entity onHurtParticleEntity;
    [ShowInInspector] Entity onHealParticleEntity;
    [ShowInInspector] Entity onDeathParticleEntity;

    [Header("Movement - Audio")]
    [ShowInInspector] Entity walkingAudioEntity;
    [ShowInInspector] Entity dashAudioEntity;
    [ShowInInspector] Entity idleAudioEntity;

    [Header("Movement - Particles")]
    [ShowInInspector] Entity walkingParticleEntity;
    [ShowInInspector] Entity dashParticleEntity;

    [Header("Combat - Audio")]
    [ShowInInspector] Entity attackAudioEntity;

    [Header("Combat - Particles")]
    [ShowInInspector] Entity attackParticleEntity;

    [Header("Flint&Steel - Audio")]
    [ShowInInspector] Entity flintSteelAudioEntity;

    [Header("Flint&Steel - Particles")]
    [ShowInInspector] Entity flintSteelParticleEntity;

    [Header("Grapple - Audio")]
    [ShowInInspector] Entity grappleAudioEntity;

    [Header("Grapple - Particles")]
    [ShowInInspector] Entity grappleParticleEntity;


    AudioSource hurtAudio, healAudio, deathAudio;
    AudioSource walkAudio, dashAudio, idleAudio;
    AudioSource attackAudio;
    AudioSource flintSteelAudio;
    AudioSource grappleAudio;

    ParticleComponent hurtParticle, healParticle, deathParticle;
    ParticleComponent walkParticle, dashParticle;
    ParticleComponent attackParticle;
    ParticleComponent flintSteelParticle;
    ParticleComponent grappleParticle;


    private bool initialized = false;

    void OnCreate()
    {

        hurtAudio = onHurtAudioEntity.GetComponent<AudioSource>();
        healAudio = onHealAudioEntity.GetComponent<AudioSource>();
        deathAudio = onDeathAudioEntity.GetComponent<AudioSource>();

        hurtParticle = onHurtParticleEntity.GetComponent<ParticleComponent>();
        healParticle = onHealParticleEntity.GetComponent<ParticleComponent>();
        deathParticle = onDeathParticleEntity.GetComponent<ParticleComponent>();


        walkAudio = walkingAudioEntity.GetComponent<AudioSource>();
        dashAudio = dashAudioEntity.GetComponent<AudioSource>();
        idleAudio = idleAudioEntity.GetComponent<AudioSource>();

        walkParticle = walkingParticleEntity.GetComponent<ParticleComponent>();
        dashParticle = dashParticleEntity.GetComponent<ParticleComponent>();


        attackAudio = attackAudioEntity.GetComponent<AudioSource>();  
        
        attackParticle = attackParticleEntity.GetComponent<ParticleComponent>();


        flintSteelAudio = flintSteelAudioEntity.GetComponent<AudioSource>();  
        
        flintSteelParticle = flintSteelParticleEntity.GetComponent<ParticleComponent>();


        grappleAudio = grappleAudioEntity.GetComponent<AudioSource>();  
        
        grappleParticle = grappleParticleEntity.GetComponent<ParticleComponent>();
    }

    public void ProcessFeedback()
    {
       
    }

    public void Initialize()
    {
        initialized = true;

        player.PlayerHealth.OnDeath += PlayDeath;
        player.PlayerHealth.OnHeal += PlayHeal;
        player.PlayerHealth.OnHit += PlayHurt;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();

        if (!initialized)
            return;

        player.PlayerHealth.OnDeath -= PlayDeath;
        player.PlayerHealth.OnHeal -= PlayHeal;
        player.PlayerHealth.OnHit -= PlayHurt;

        initialized = false;
    }

    // --- HEALTH ---

    public void PlayDeath()
    {
        PlayFeedback(deathAudio, deathParticle);
    }

    public void PlayHurt()
    {
        PlayFeedback(hurtAudio, hurtParticle,0.1f);
    }

    public void PlayHeal()
    {
        PlayFeedback(healAudio, healParticle,0.5f);
    }

    // --- MOVEMENT ---

    public void PlayDash()
    {
        PlayFeedback(dashAudio, dashParticle,0.3f);
    }

    public void PlayIdle()
    {
        int randomValue = Loopie.Random.Range(0, 100) + 1;
        if (randomValue < 30)
            PlayFeedback(idleAudio);
    }

    public void PlayWalk()
    {
        PlayFeedback(walkAudio, walkParticle,0.2f);
    }

    public void PlayAttack()
    {
        PlayFeedback(attackAudio, attackParticle);
    }

    public void PlayFlintSteel(){
        PlayFeedback(flintSteelAudio,flintSteelParticle,0.3f);
    }

    public void PlayGrapple()
    {
        PlayFeedback(grappleAudio, grappleParticle, 0.3f);
    }

    // --- CORE ---

    void PlayFeedback(AudioSource audio)
    {
        if (audio != null)
            audio.Play();
    }

    void PlayFeedback(ParticleComponent particle, float duration = 1f)
    {
        if (particle != null)
            StartCoroutine(PlayParticles(particle, duration));
    }

    void PlayFeedback(AudioSource audio, ParticleComponent particle, float duration = 1f)
    {
        if (audio != null)
            PlayFeedback(audio);

        if (particle != null)
        {
            PlayFeedback(particle, duration);
        }
    }

    IEnumerator PlayParticles(ParticleComponent component, float duration)
    {
        component.Play();
        yield return new WaitForSeconds(duration);
        component.Stop();
    }

    void ShakeCamera(float amount, float time){
        
    }
}