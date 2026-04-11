using System;
using Loopie;

class Exit : Component
{
    public bool Blocked;

    public void ExitGame()
    {
        if (!Blocked)
        {
            Debug.Log("Exit Success");
            Application.Quit();
        }
    }
};