using System;
using System.Collections.Generic;
using Loopie;

public class AudioEntity : Component
{
    [ShowInInspector] private string Key;
    private AudioSource source;

    void OnCreate()
    {
        source = entity.GetComponent<AudioSource>();
    }
    void OnPostCreate()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Register(this);
        }
    }

    void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Unregister(this);
        }
    }

    public string GetKey() { return Key; }
};