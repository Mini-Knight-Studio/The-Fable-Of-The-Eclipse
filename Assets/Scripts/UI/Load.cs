using System;
using Loopie;

class Load : Component
{
    bool isLoading = false;

    public void LoadPreviousSave()
    {
        isLoading = true;
        
        GlobalDatabase.Data.Load();
        SceneManager.LoadSceneByID(GlobalDatabase.Data.Player.currentSceneUUID);

        Debug.Log("Load Success");
    }
};