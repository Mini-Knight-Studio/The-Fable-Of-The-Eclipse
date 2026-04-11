using System;
using Loopie;

class ColliderSceneTransition : SceneTransition
{
    public string SceneUUID;
    private BoxCollider collision;

    void OnCreate()
    {
        collision = entity.GetComponent<BoxCollider>();
        collision.Trigger = true;
        UUID = SceneUUID;
    }

    void OnUpdate()
    {
        if(collision.HasCollided)
        {
            DatabaseRegistry.playerDB.Player.SetCurrentScene(UUID);
            StartTransition();
        }
    }
};
