using System;
using Loopie;

class Exit : Component
{
    public bool Blocked;

    public void ExitGame()
    {
        if (!Blocked)
        {
            GlobalDatabase.GlobalData.mainMenuDB.MainMenu.hasPlayedIntro = false;
            GlobalDatabase.GlobalData.SaveAll();
            Debug.Log("Exit Success");
            Application.Quit();
        }
    }
};