using System;
using Loopie;

class SfxVolume : Component
{
    private float volume = 1.0f;

    void OnCreate()
    {
        volume = ClampAndRound(GlobalDatabase.GlobalData.settingsDB.Settings.SfxVolume);
        ApplyToMixer();
    }

    public void ApplySfxVolume()
    {
        volume = ClampAndRound(volume);
        GlobalDatabase.GlobalData.settingsDB.Settings.SfxVolume = volume;
        ApplyToMixer();
    }

    public void IncreaseVolume()
    {
        volume = ClampAndRound(volume + 0.1f);
    }

    public void DecreaseVolume()
    {
        volume = ClampAndRound(volume - 0.1f);
    }

    public float GetVolume()
    {
        return volume;
    }

    private void ApplyToMixer()
    {
        AudioMixer.SetVolume("Master/Sfx", AudioMixer.LinearToEngineVolume(volume));
    }

    private float ClampAndRound(float value)
    {
        value = Mathf.Clamp(value, 0f, 1f);
        return Mathf.Round(value * 10f) / 10f;
    }
};