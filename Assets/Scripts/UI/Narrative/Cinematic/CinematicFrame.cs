using Loopie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class CinematicFrame : Component
{
    public float Duration;
    public bool UsePage;
    public bool FadeTextGroup;
    public bool FadeInText;
    public bool FadeOutText;

    public bool WaitsForInput { get { return Duration <= 0f; } }

    [HideInInspector] public Entity FrameEntity;
    [HideInInspector]public CanvasGroup FrameCanvasGroup;

    public Entity FrameGroupEntity;
    [HideInInspector] public CanvasGroup TextGroupCanvasGroup;

    public Entity TextEntity;
    [HideInInspector] public CanvasGroup TextCanvasGroup;

    public Entity AudioEntity;
    [HideInInspector] public AudioSource AudioSource;

    void OnCreate()
    {
        FrameEntity = entity;
        FrameCanvasGroup = entity.GetComponent<CanvasGroup>();

        if (FrameGroupEntity != null)
            TextGroupCanvasGroup = FrameGroupEntity.GetComponent<CanvasGroup>();

        if (TextEntity != null)
            TextCanvasGroup = TextEntity.GetComponent<CanvasGroup>();

        if (AudioEntity != null)
            AudioSource = AudioEntity.GetComponent<AudioSource>();
    }
}

