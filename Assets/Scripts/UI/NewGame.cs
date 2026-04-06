using System;
using Loopie;

class NewGame : Component
{
    public string UUID;
    public bool Blocked;

    public void StartNewGame()
    {
        if (!Blocked)
        {
            GlobalDatabase.Data.Player.ResetScenes();
            SceneManager.LoadSceneByID(UUID);
        }
    }
};