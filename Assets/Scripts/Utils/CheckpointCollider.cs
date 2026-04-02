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
        if (!hasSaved && collider.IsColliding)
        {
            hasSaved = true;
            if (GlobalDatabase.Data.Exists())
            {
                GlobalDatabase.Data.Player.playerPositionX = player.transform.position.x;
                GlobalDatabase.Data.Player.playerPositionY = player.transform.position.y;
                GlobalDatabase.Data.Player.playerPositionZ = player.transform.position.z;
                GlobalDatabase.Data.Save();
            }
        }
    }
};