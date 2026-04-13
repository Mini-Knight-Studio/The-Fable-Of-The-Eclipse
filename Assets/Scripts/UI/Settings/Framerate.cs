using System;
using Loopie;

class Framerate : Component
{
    private int framerate = 60;

    void OnCreate()
    {
        framerate = GlobalDatabase.GlobalData.settingsDB.Settings.Framerate;
        Window.TargetFramerate = GlobalDatabase.GlobalData.settingsDB.Settings.Framerate;
    }

    public void ApplyFramerate()
    {
        GlobalDatabase.GlobalData.settingsDB.Settings.Framerate = framerate;
        Window.TargetFramerate = GlobalDatabase.GlobalData.settingsDB.Settings.Framerate;
    }

    public void IncreaseFramerate()
    {
        if (framerate < 995)
        {
            framerate += 5;
        }
    }

    public void DecreaseFramerate()
    {
        if (framerate > 5)
        {
            framerate -= 5;
        }
    }

    public float GetFramerate()
    {
        return framerate;
    }
};