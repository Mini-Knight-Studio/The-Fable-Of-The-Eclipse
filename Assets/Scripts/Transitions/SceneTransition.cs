using System;
using Loopie;

public class SceneTransition : Component
{
	public string UUID;
    public bool Blocked;
    
    public void StartTransition()
    {
    	if(!Blocked)
        {
            GlobalDatabase.Data.Player.SetCurrentScene(UUID);
            SceneManager.LoadSceneByID(UUID);
        }
    }
};



