using System;
using Loopie;

public class LoadSettings : Component
{
    public void ImportSettings()
    {
        // Visual
        Window.Fullscreen = GlobalDatabase.GlobalData.settingsDB.Settings.Fullscreen;
        Window.TargetFramerate = GlobalDatabase.GlobalData.settingsDB.Settings.Framerate;
        Window.VSync = GlobalDatabase.GlobalData.settingsDB.Settings.VSync;

        // Audio
        MasterVolume.volume = GlobalDatabase.GlobalData.settingsDB.Settings.MasterVolume;
        MasterVolume.ApplyToMixer();
        MusicVolume.volume = GlobalDatabase.GlobalData.settingsDB.Settings.MusicVolume;
        MusicVolume.ApplyToMixer();
        SfxVolume.volume = GlobalDatabase.GlobalData.settingsDB.Settings.SfxVolume;
        SfxVolume.ApplyToMixer();
    }
};