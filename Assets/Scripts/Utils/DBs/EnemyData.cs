using System;
using Loopie;

public class EnemyData
{

    // Scenes and position
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

    public float enemyPositionX = 0.0f;
    public float enemyPositionY = 0.0f;
    public float enemyPositionZ = 0.0f;


}