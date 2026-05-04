using System;
using System.Collections.Generic;
using System.Numerics;
using Loopie;

public class SpawnersData
{
    public string currentSceneUUID = "";
    public string previousSceneUUID = "";

    public void SetCurrentScene(string newSceneUUID)
    {
        if (currentSceneUUID == newSceneUUID) return;
        previousSceneUUID = currentSceneUUID;
        currentSceneUUID = newSceneUUID;
    }

    public void ResetScenes()
    {
        currentSceneUUID = "";
        previousSceneUUID = "";
    }
    public List<SpawnerData> spawners = new List<SpawnerData>();


}

public class SpawnerData
{
    public int spawnerID;
    public bool alreadySpawned;

}