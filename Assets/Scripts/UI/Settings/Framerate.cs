using System;
using Loopie;

class Framerate : Component
{
    private int framerate = 60;

    void OnCreate()
    {
        framerate = GlobalDatabase.Data.Settings.Framerate;
        Window.TargetFramerate = GlobalDatabase.Data.Settings.Framerate;
    }

    public void ApplyFramerate()
    {
        GlobalDatabase.Data.Settings.Framerate = framerate;
        Window.TargetFramerate = GlobalDatabase.Data.Settings.Framerate;
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