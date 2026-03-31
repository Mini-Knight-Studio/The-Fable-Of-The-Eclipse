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
            SceneStatesManager.SetCurrentScene(UUID);
            SceneManager.LoadSceneByID(UUID);
        }
    }
};



