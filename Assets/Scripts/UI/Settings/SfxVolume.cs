using System;
using Loopie;

class SfxVolume : Component
{
    private float volume = 1.0f;

    void OnCreate()
    {
        volume = GlobalDatabase.GlobalData.settingsDB.Settings.SfxVolume;
        AudioMixer.SetVolume("Master/Sfx", AudioMixer.LinearToEngineVolume(volume));
    }

    public void ApplySfxVolume()
    {
        GlobalDatabase.GlobalData.settingsDB.Settings.SfxVolume = volume;
        AudioMixer.SetVolume("Master/Sfx", AudioMixer.LinearToEngineVolume(volume));
    }

    public void IncreaseVolume()
    {
        if (volume < 1.0f)
        {
            volume += 0.1f;
        }
    }

    public void DecreaseVolume()
    {
        if (volume > 0.0f)
        {
            volume -= 0.1f;
        }
    }

    public float GetVolume()
    {
        return volume;
    }
};