using System;
using Loopie;

class SceneTransition : Component
{
    public string SceneUUID;
    public bool Blocked;
    private BoxCollider transitionArea;

    void OnCreate()
    {
        transitionArea = entity.GetComponent<BoxCollider>();
        transitionArea.Trigger = true;
    }

    void OnUpdate()
    {
        if(!Blocked && transitionArea.HasCollided)
        {
            SceneManager.LoadSceneByID(SceneUUID);
        }
    }
};