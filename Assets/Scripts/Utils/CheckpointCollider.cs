using System;
using Loopie;

class CheckpointCollider : Component
{
    private BoxCollider collider;
    private bool hasSaved = false;
    public Entity player;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (!hasSaved)
        {
            GlobalDatabase.Data.Player.playerPosition = player.transform.position;
            GlobalDatabase.Data.Save();
            hasSaved = true;
        }
    }
};