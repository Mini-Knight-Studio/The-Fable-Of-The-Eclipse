using System;
using Loopie;

public class Fullscreen : Component
{
    private bool isFullscreen = false;

    void OnCreate()
    {
        isFullscreen = GlobalDatabase.Data.Settings.Fullscreen;
        Window.Fullscreen = GlobalDatabase.Data.Settings.Fullscreen;
    }

    public void ApplyFullscreen()
    {
        GlobalDatabase.Data.Settings.Fullscreen = isFullscreen;
        Window.Fullscreen = GlobalDatabase.Data.Settings.Fullscreen;
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