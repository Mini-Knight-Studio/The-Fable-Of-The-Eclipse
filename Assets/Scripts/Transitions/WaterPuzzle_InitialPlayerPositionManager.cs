using System;
using Loopie;

class WaterPuzzle_InitialPlayerPositionManager : Component
{
    public string waterpuzzleUUID;
    public string mechanicSceneUUID;
    public string waterpathUUID;

    public Entity fromMechanicReference;
    public Entity fromWaterpathReference;

    void OnPostCreate()
    {
        DatabaseRegistry.playerDB.Player.SetCurrentScene(waterpuzzleUUID);

        float previousY = Player.Instance.transform.position.y;

        if (DatabaseRegistry.playerDB.Player.previousSceneUUID == mechanicSceneUUID)
        {
            var pos = fromMechanicReference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromMechanicReference.transform.rotation;
        }
        else if (DatabaseRegistry.playerDB.Player.previousSceneUUID == waterpathUUID)
        {
            var pos = fromWaterpathReference.transform.position;
            pos.y = previousY;

            Player.Instance.transform.local_position = pos;
            Player.Instance.transform.local_rotation = fromWaterpathReference.transform.rotation;
        }
    }

};