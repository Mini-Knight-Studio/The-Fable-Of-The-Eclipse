using System;
using Loopie;

class Waterpath_InitialPlayerPositionManager : Component
{
    public string waterpathUUID;
    public string puzzleSceneUUID;
    public string level2SceneUUID;

    public Vector3 FromPuzzlePlayerPos;
    public Vector3 FromPuzzlePlayerRot;
    public Vector3 FromLvl2PlayerPos;
    public Vector3 FromLvl2PlayerRot;

    public Entity player;
    void OnCreate()
    {
        GlobalDatabase.Data.Player.SetCurrentScene(waterpathUUID);

        if(GlobalDatabase.Data.Player.previousSceneUUID == puzzleSceneUUID)
        {
            FromPuzzlePlayerPos.y = player.transform.local_position.y;

            player.transform.local_position = FromPuzzlePlayerPos;
            player.transform.local_rotation = FromPuzzlePlayerRot;
        }
        else if (GlobalDatabase.Data.Player.previousSceneUUID == level2SceneUUID)
        {
            FromLvl2PlayerPos.y = player.transform.local_position.y;

            player.transform.local_position = FromLvl2PlayerPos;
            player.transform.local_rotation = FromLvl2PlayerRot;
        }
    }
};