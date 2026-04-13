using System;
using Loopie;

public class Fullscreen : Component
{
    private bool isFullscreen = false;

    void OnCreate()
    {
        isFullscreen = GlobalDatabase.GlobalData.settingsDB.Settings.Fullscreen;
        Window.Fullscreen = GlobalDatabase.GlobalData.settingsDB.Settings.Fullscreen;
    }

    public void ApplyFullscreen()
    {
        GlobalDatabase.GlobalData.settingsDB.Settings.Fullscreen = isFullscreen;
        Window.Fullscreen = GlobalDatabase.GlobalData.settingsDB.Settings.Fullscreen;
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
    }

    public bool IsFullscreen()
    {
        return isFullscreen;
    } 
};