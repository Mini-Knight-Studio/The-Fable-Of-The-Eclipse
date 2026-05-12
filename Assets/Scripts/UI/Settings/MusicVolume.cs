using System;
using Loopie;

public class MusicVolume : Component
{
    public static float volume = 1.0f;
    void OnCreate()
    {
        volume = ClampAndRound(GlobalDatabase.GlobalData.settingsDB.Settings.MusicVolume);
        ApplyToMixer();
    }

    public static void ApplyMusicVolume()
    {
        volume = ClampAndRound(volume);
        GlobalDatabase.GlobalData.settingsDB.Settings.MusicVolume = volume;
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

    public static void ApplyToMixer()
    {
        AudioMixer.SetVolume("Master/Music", AudioMixer.LinearToEngineVolume(volume));
    }

    private static float ClampAndRound(float value)
    {
        value = Mathf.Clamp(value, 0f, 1f);
        return Mathf.Round(value * 10f) / 10f;
    }
}