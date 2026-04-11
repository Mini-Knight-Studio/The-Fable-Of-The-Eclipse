using System;
using Loopie;

class MusicVolume : Component
{
    private float volume = 1.0f;

    void OnCreate()
    {
        volume = GlobalDatabase.GlobalData.settingsDB.Settings.MusicVolume;
        // Set in engine the volume.
    }

    public void ApplyMusicVolume()
    {
        GlobalDatabase.GlobalData.settingsDB.Settings.MusicVolume = volume;
        // Set in engine the volume.
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