using System;
using Loopie;

class VSync : Component
{
    private bool isVSync = false;

    void OnCreate()
    {
        isVSync = GlobalDatabase.Data.Settings.VSync;
        Window.VSync = GlobalDatabase.Data.Settings.VSync;
    }

    public void ApplyVSync()
    {
        GlobalDatabase.Data.Settings.VSync = isVSync;
        Window.VSync = GlobalDatabase.Data.Settings.VSync;
    }

    public void ToggleVSync()
    {
        isVSync = !isVSync;
    }

    public bool IsVSync()
    {
        return isVSync;
    }
};