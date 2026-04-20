using System;
using Loopie;

class Load : Component
{
    bool isLoading = false;

    public void LoadPreviousSave()
    {
        isLoading = true;

        DatabaseRegistry.LoadAll();
        SceneManager.LoadSceneByID(DatabaseRegistry.playerDB.Player.currentSceneUUID);

        Debug.Log("Load Success");
        Debug.Log(DatabaseRegistry.enemiesDB.Enemies.enemies.Count);
    }
};