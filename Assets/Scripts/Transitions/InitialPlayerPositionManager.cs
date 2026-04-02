using System;
using Loopie;

class InitialPlayerPositionManager : Component
{
    public string level1SceneUUID;
    public string puzzle1SceneUUID;
    public string level2SceneUUID;

    public Vector3 fromPuzzlePlayerPos;
    public Vector3 fromLvl2PlayerPos;

    public Entity player;
    void OnCreate()
    {
        SceneStatesManager.SetCurrentScene(level1SceneUUID);

        if(SceneStatesManager.GetPreviousSceneUUID() == puzzle1SceneUUID)
        {
            fromPuzzlePlayerPos.y = player.transform.position.y;
            player.transform.position = fromPuzzlePlayerPos;
        }
        else if (SceneStatesManager.GetPreviousSceneUUID() == level2SceneUUID)
        {
            fromLvl2PlayerPos.y = player.transform.position.y;
            player.transform.position = fromLvl2PlayerPos;
        }
    }
};