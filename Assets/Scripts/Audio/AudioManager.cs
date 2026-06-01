using System;
using System.Collections.Generic;
using Loopie;

public class AudioManager : Component
{
    public static AudioManager Instance { get; private set; }


    private Dictionary<string, AudioEntity> audios = new Dictionary<string, AudioEntity>();

    void OnCreate()
    {
        if (Instance != null)
        {
            entity.Destroy();
            return;
        }
        DontDestroyOnLoad();

    }

    public void Register(AudioEntity audioEntity)
    {
        string key = audioEntity.GetKey();
        if (audios.ContainsKey(key))
            return;
        audios.Add(key, audioEntity);
    }

    public void Unregister(AudioEntity audioEntity)
    {
        string key = audioEntity.GetKey();
        if (!audios.ContainsKey(key))
            return;
        audios.Remove(key);
    }
};