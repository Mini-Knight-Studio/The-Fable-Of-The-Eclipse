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
            SceneManager.LoadSceneByID(UUID);
        }
    }
};



