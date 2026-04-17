using System;
using System.Collections.Generic;
using System.Numerics;
using Loopie;


public class EnemiesData
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

    List<EnemyData> enemies;

    struct EnemyData 
    {
        public float enemyPositionX = 0.0f;
        public float enemyPositionY = 0.0f;
        public float enemyPositionZ = 0.0f;

        public int hp = 0;

        public int shieldHP = 0;
    }

}