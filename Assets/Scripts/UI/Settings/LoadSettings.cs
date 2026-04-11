using System;
using Loopie;

public class LoadSettings : Component
{
    public void ImportSettings()
    {
        Window.Fullscreen = GlobalDatabase.GlobalData.settingsDB.Settings.Fullscreen;
        Window.TargetFramerate = GlobalDatabase.GlobalData.settingsDB.Settings.Framerate;
        Window.VSync = GlobalDatabase.GlobalData.settingsDB.Settings.VSync;
        // Master Volume
        // Music Volume
        // Sfx Volume
    }
};