using System;
using Loopie;

class InitialPlayerPositionManager : Component
{
    public string level1SceneUUID;
    public string puzzle1SceneUUID;
    public string level2SceneUUID;

    public Vector3 puzzleToLvl1PlayerPos;
    public Vector3 Lvl2ToLvl1PlayerPos;

    public Entity player;
    void OnCreate()
    {
        GlobalDatabase.Data.Player.SetCurrentScene(level1SceneUUID);

        if(GlobalDatabase.Data.Player.previousSceneUUID == puzzle1SceneUUID)
        {
            puzzleToLvl1PlayerPos.y = player.transform.position.y;
            player.transform.position = puzzleToLvl1PlayerPos;
        }
        else if (GlobalDatabase.Data.Player.previousSceneUUID == level2SceneUUID)
        {
            Lvl2ToLvl1PlayerPos.y = player.transform.position.y;
            player.transform.position = Lvl2ToLvl1PlayerPos;
        }
    }
};