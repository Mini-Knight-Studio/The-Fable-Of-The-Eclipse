using System;
using Loopie;

class MasterVolume : Component
{
    private float volume = 1.0f;

    void OnCreate()
    {
        volume = ClampAndRound(GlobalDatabase.GlobalData.settingsDB.Settings.MasterVolume);
        ApplyToMixer();
    }

    public void ApplyMasterVolume()
    {
        volume = ClampAndRound(volume);
        GlobalDatabase.GlobalData.settingsDB.Settings.MasterVolume = volume;
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
        AudioMixer.SetVolume("Master", AudioMixer.LinearToEngineVolume(volume));
    }

    private float ClampAndRound(float value)
    {
        value = Mathf.Clamp(value, 0f, 1f);
        return Mathf.Round(value * 10f) / 10f;
    }
}