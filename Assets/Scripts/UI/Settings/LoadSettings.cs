using System;
using Loopie;

public class LoadSettings : Component
{
    public void ImportSettings()
    {
        Window.Fullscreen = GlobalDatabase.Data.Settings.Fullscreen;
        Window.TargetFramerate = GlobalDatabase.Data.Settings.Framerate;
        Window.VSync = GlobalDatabase.Data.Settings.VSync;
        // Master Volume
        // Music Volume
        // Sfx Volume
    }
};