using System;
using Loopie;

class Load : Component
{
    bool isLoading = false;

    public void LoadPreviousSave()
    {
        isLoading = true;
        
        GlobalDatabase.Data.Load();
        SceneStatesManager.SetCurrentScene(UUID);
        SceneManager.LoadSceneByID(UUID);

        Debug.Log("Load Success");
    }
};