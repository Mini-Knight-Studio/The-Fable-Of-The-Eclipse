using System;
using Loopie;

class VSync : Component
{
    private bool isVSync = false;

    void OnCreate()
    {
        isVSync = GlobalDatabase.GlobalData.settingsDB.Settings.VSync;
        Window.VSync = GlobalDatabase.GlobalData.settingsDB.Settings.VSync;
    }

    public void ApplyVSync()
    {
        GlobalDatabase.GlobalData.settingsDB.Settings.VSync = isVSync;
        Window.VSync = GlobalDatabase.GlobalData.settingsDB.Settings.VSync;
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