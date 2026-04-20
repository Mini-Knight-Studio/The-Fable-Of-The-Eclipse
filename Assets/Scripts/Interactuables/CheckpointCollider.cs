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
            if (DatabaseRegistry.playerDB.Exists())
            {
                DatabaseRegistry.playerDB.Player.playerPositionX = player.transform.position.x;
                DatabaseRegistry.playerDB.Player.playerPositionY = player.transform.position.y;
                DatabaseRegistry.playerDB.Player.playerPositionZ = player.transform.position.z;
                DatabaseRegistry.playerDB.Save();
            }
            if (DatabaseRegistry.puzzlesDB.Exists())
            {
                DatabaseRegistry.puzzlesDB.Save();
            }
        }
    }
};