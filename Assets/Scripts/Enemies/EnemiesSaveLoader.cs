using System;
using System.Collections.Generic;
using Loopie;


class EnemyRestorer : Component
{
    public Entity WaterBlobReference;
    public Entity GroundBlobReference;
    public Entity FireBlobReference;
    public Entity GolemReference;

    void OnCreate()
    {
        if (!DatabaseRegistry.enemiesDB.Exists())
        {
            Debug.Log("No save found");
            return;
        }

        List<EnemiesData.EnemyData> savedEnemies = DatabaseRegistry.enemiesDB.Enemies.enemies;
        if (savedEnemies == null || savedEnemies.Count == 0)
        {
            Debug.Log("No enemies in save");
            return;
        }

        foreach (EnemiesData.EnemyData data in savedEnemies)
        {
            Entity reference = GetReferenceForType(data.enemyType);
            if (reference == null)
            {
                Debug.LogWarning("No reference found for type " + data.enemyType);
                continue;
            }

            Entity spawned = Spawner.Spawn(reference, new Vector3(data.enemyPositionX, data.enemyPositionY, data.enemyPositionZ));

            Health health = spawned.GetComponent<Health>();
            if (health != null)
            {
                health.actualHealth = data.hp;
            }

            if (data.enemyType == "Golem")
            {
                Golem golem = spawned.GetComponent<Golem>();
                if (golem != null)
                {
                    golem.ShieldLife = data.shieldHP;
                }

            }
            else
            {
                Blob blob = spawned.GetComponent<Blob>();
                if (blob != null) 
                {
                    blob.SetStage(data.blobStage);
                }

            }

            Debug.Log("EnemyRestorer: spawned " + data.enemyType + " at " + data.enemyPositionX + ", " + data.enemyPositionY + ", " + data.enemyPositionZ);
        }

        Debug.Log("EnemyRestorer: restored " + savedEnemies.Count + " enemies");
    }

    Entity GetReferenceForType(string enemyType)
    {
        if (enemyType == "WaterBlob") return WaterBlobReference;
        if (enemyType == "GroundBlob") return GroundBlobReference;
        if (enemyType == "FireBlob") return FireBlobReference;
        if (enemyType == "Golem") return GolemReference;
        return null;
    }
}