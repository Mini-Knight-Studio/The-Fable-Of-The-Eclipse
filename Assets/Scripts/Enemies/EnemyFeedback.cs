using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

public class EnemyFeedback : Component
{
    public Entity FeedbackEntity;
    private ParticleComponent Particles;
    private Dictionary<string, AudioSource> audioDatas;

    void OnCreate()
    {
        audioDatas = new Dictionary<string, AudioSource> ();
        Particles = FeedbackEntity.GetComponent<ParticleComponent>();
        foreach (Entity child in FeedbackEntity.Children)
        {
            AudioSource sound_source = child.GetComponent<AudioSource>();
            if (sound_source == null) continue;
            audioDatas.Add(child.Name, sound_source);
        }
    }

    #region Sound
    public void PlaySound(string sound_type)
    {
        AudioSource source = audioDatas[sound_type];
        if(source == null) return;
        source.Looping = false;
        source.Play();
    }
    #endregion

    #region Particles
    public void TickParticles(string emitter_name, float tick_duration)
    {
        int index = Particles.GetEmitterIndex(emitter_name);
        if (index != -1)
            StartCoroutine(PlayParticles(index, tick_duration));
    }

    private IEnumerator PlayParticles(int emitter_index, float duration)
    {
        Particles.SetEmitterState(emitter_index, true);
        yield return new WaitForSeconds(duration);
        Particles.SetEmitterState(emitter_index, false);
    }
    #endregion

    public void ShakeCamera(float amount, float duration)
    {
        Player.Instance.Camera.SetIsShaking(true, duration, amount);
    }

    public void SetParticlesState(string emitter_name, bool state)
    {
        int index = Particles.GetEmitterIndex(emitter_name);
        if (index != -1)
            Particles.SetEmitterState(index, state);
    }
};