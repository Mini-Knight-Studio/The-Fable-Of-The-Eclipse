using System;
using Loopie;

class Hub_InitialPlayerPositionManager : Component
{
    public string level1SceneUUID;
    public string level2SceneUUID;
    public string waterpathUUID;
    public string firepathUUID;

    public Vector3 FromLvl1PlayerPos;
    public Vector3 FromLvl1PlayerRot;
    public Vector3 FromWaterpathPlayerPos;
    public Vector3 FromWaterpathPlayerRot;
    public Vector3 FromFirepathPlayerPos;
    public Vector3 FromFirepathPlayerRot;

    public Entity player;
    void OnCreate()
    {
        DatabaseRegistry.playerDB.Player.SetCurrentScene(level2SceneUUID);

        if(DatabaseRegistry.playerDB.Player.previousSceneUUID == level1SceneUUID)
        {
            FromLvl1PlayerPos.y = player.transform.local_position.y;

            player.transform.local_position = FromLvl1PlayerPos;
            player.transform.local_rotation = FromLvl1PlayerRot;
        }
        else if (DatabaseRegistry.playerDB.Player.previousSceneUUID == waterpathUUID)
        {
            FromWaterpathPlayerPos.y = player.transform.local_position.y;

            player.transform.local_position = FromWaterpathPlayerPos;
            player.transform.local_rotation = FromWaterpathPlayerRot;
        }
        else if (DatabaseRegistry.playerDB.Player.previousSceneUUID == firepathUUID)
        {
            FromFirepathPlayerPos.y = player.transform.local_position.y;

            player.transform.local_position = FromFirepathPlayerPos;
            player.transform.local_rotation = FromFirepathPlayerRot;
        }
    }
};