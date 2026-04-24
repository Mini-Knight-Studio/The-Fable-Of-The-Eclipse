using System;
using Loopie;

public class EnemyFeedback : Component
{
    public Entity FeedbackEntity;
    private ParticleComponent Particles;
    private AudioSource SFX;

    void OnCreate()
    {
        Particles = FeedbackEntity.GetComponent<ParticleComponent>();
        SFX = FeedbackEntity.GetComponent<AudioSource>();
    }
};