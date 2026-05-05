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
            DatabaseRegistry.LoadAll();
            RestoreEnemies();
            RestoreSpawners();
            
        }
    }

    void RestoreEnemies() 
    {

        string path = "enemiesDB.json";

        string rawJson = System.IO.File.ReadAllText(path);
        JObject root = JObject.Parse(rawJson);
        string enemiesJson = root["Enemies"].ToString();
        EnemiesData testData = JsonConvert.DeserializeObject<EnemiesData>(enemiesJson);
        Debug.Log("Direct count: " + testData.enemies.Count);

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            //Debug.Log("RAW JSON: " + json);
        }
        else
        {
            Debug.LogError("FILE NOT FOUND: " + path);
        }

        if (!DatabaseRegistry.enemiesDB.Exists())
        {
            Debug.LogError("EnemiesDB not Found");
            return;
        }

        List<EnemyData> savedEnemies = DatabaseRegistry.enemiesDB.Enemies.enemies;
        if (savedEnemies == null || savedEnemies.Count == 0)
        {
            Debug.Log("Saved Enemies Count: " + savedEnemies.Count);
            Debug.Log("No enemies in save");
            return;
        }
        foreach (EnemyData data in savedEnemies)
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

    void RestoreSpawners() 
    {

        string path = "spawnersDB.json";

        string rawJson = System.IO.File.ReadAllText(path);
        JObject root = JObject.Parse(rawJson);
        string spawnersJson = root["Spawners"].ToString();
        SpawnersData testData = JsonConvert.DeserializeObject<SpawnersData>(spawnersJson);
        Debug.Log("Direct count: " + testData.spawners.Count);

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            //Debug.Log("RAW JSON: " + json);
        }
        else
        {
            Debug.LogError("FILE NOT FOUND: " + path);
        }

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