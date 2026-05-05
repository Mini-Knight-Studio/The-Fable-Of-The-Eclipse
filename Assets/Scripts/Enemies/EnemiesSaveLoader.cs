using System;
using System.Collections.Generic;
using Loopie;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class EnemyRestorer : Component
{
    public Entity WaterBlobReference;
    public Entity GroundBlobReference;
    public Entity FireBlobReference;
    public Entity GolemReference;


    private bool hasRestored = false;


    void OnUpdate()
    {
        if (!hasRestored)
        {

            hasRestored = true;
            RestoreEnemies();
            RestoreSpawners();
            
        }
    }

    void RestoreEnemies() 
    {

        if (!DatabaseRegistry.enemiesDB.Exists())
        {
            Debug.LogError("EnemiesDB not Found");
            return;
        }

        Dictionary<string, List<EnemyData>> savedEnemies = DatabaseRegistry.enemiesDB.Enemies.enemiesDictionary;
        if (savedEnemies == null || savedEnemies.Count == 0)
        {
            Debug.Log("Saved Enemies Count: " + savedEnemies.Count);
            Debug.Log("No enemies in save");
            return;
        }

        if (!savedEnemies.ContainsKey(DatabaseRegistry.playerDB.Player.currentSceneUUID))
            return;

        foreach (EnemyData data in savedEnemies[DatabaseRegistry.playerDB.Player.currentSceneUUID])
        {
            Entity reference = GetReferenceForType(data.enemyType);
            if (reference == null)
            {
                Debug.LogWarning("No reference found for type " + data.enemyType);
                continue;
            }

            Entity spawned = reference.Clone(true);
            spawned.Name = spawned.Name.Replace("_Reference_", "").Replace("_Reference", "");
            spawned.transform.position = new Vector3(data.enemyPositionX, data.enemyPositionY, data.enemyPositionZ);
            spawned.SetActive(true);

            Health health = spawned.GetComponent<Health>();
            if (health != null)
            {
                health.ModifyActualHealth(data.hp);
            }



            if (data.enemyType == "Golem")
            {
                Golem golem = spawned.GetComponent<Golem>();
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

    void RestoreSpawners() 
    {

        if (!DatabaseRegistry.spawnersDB.Exists())
        {
            Debug.LogError("SpawnersDB not Found");
            return;
        }

        List<SpawnerData> savedSpawners = DatabaseRegistry.spawnersDB.Spawners.spawners;
        if (savedSpawners == null || savedSpawners.Count == 0)
        {
            Debug.Log("Saved Spawners Count: " + savedSpawners.Count);
            Debug.Log("No spawners in save");
            return;
        }

        Entity SpawnersParent = Entity.FindEntityByName("EnemySpawners");
        if (SpawnersParent == null)
        {
            Debug.LogError("EnemySpawners container not found in scene");
            return;
        }

        foreach (SpawnerData data in savedSpawners)
        {
            foreach (Entity child in SpawnersParent.GetChildren())
            {
                Spawner spawner = child.GetComponent<Spawner>();

                if (spawner != null && spawner.spawnerID == data.spawnerID)
                {
                    spawner.ModifySpawnerStatus(data.alreadySpawned);

                }
            }

        }
        Debug.Log("EnemyRestorer: restored " + savedSpawners.Count + " spawners");
    }

    
    Entity GetReferenceForType(string enemyType)
    {
        if (enemyType == "WaterBlob") return WaterBlobReference;
        if (enemyType == "GroundBlob") return GroundBlobReference;
        if (enemyType == "FireBlob") return FireBlobReference;
        if (enemyType == "Blob") return GroundBlobReference;
        if (enemyType == "Golem") return GolemReference;
        return null;
    }
}