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
            DatabaseRegistry.playerDB.Player.ResetScenes();
            SceneManager.LoadSceneByID(UUID);
        }
    }
};