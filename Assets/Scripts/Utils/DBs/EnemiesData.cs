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

    public List<EnemyData> enemies = new List<EnemyData>();

    public struct EnemyData 
    {
        public string enemyType;


        public float enemyPositionX;
        public float enemyPositionY;
        public float enemyPositionZ;

        public int hp;

        public int shieldHP;
        public int blobStage;
    }

}