using System;
using Loopie;

class SfxVolume : Component
{
    private float volume = 1.0f;

    void OnCreate()
    {
        volume = GlobalDatabase.Data.Settings.SfxVolume;
        // Set in engine the volume.
    }

    public void ApplySfxVolume()
    {
        GlobalDatabase.Data.Settings.SfxVolume = volume;
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