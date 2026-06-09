using System;
using Loopie;

public class MusicStopper : Component
{

    public static MusicStopper Instance { get; private set; }

    public Entity musicEntity;
    private AudioSource musicSource;

    float currentPlayback = 0;

    void OnCreate()
    {
        if (Instance == null)
            Instance = this;
        else
            return;


        musicSource = musicEntity.GetComponent<AudioSource>();
    }

    public void PauseMusic()
    {
        if(musicSource==null)
            return;
        currentPlayback = musicSource.GetPlaybackTime();
        musicSource.Stop(0.5f);
    }

    public void ResumeMusic()
    {
        if (musicSource == null)
            return;
        musicSource.Play(currentPlayback);
    }

    public void StopMusic()
    {
        if (musicSource == null)
            return;
        currentPlayback = 0;
        musicSource.Stop(0.5f);
    }   

    public void DestroyMusic()
    {
        musicSource.entity.Destroy();
    }
};