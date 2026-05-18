using System;
using System.Collections.Generic;
using Loopie;

class CheckpointCollider : Component
{
    private BoxCollider collider;
    private bool hasSaved = false;

    void OnPostCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
        

        if (collider.IsColliding || collider.HasCollided)
            Save();
    }

    void OnUpdate()
    {
        if (!hasSaved && collider.IsColliding)
        {
            Save();
        }
    }

    public void Save()
    {
        Player player = Player.Instance;
        hasSaved = true;
        if (DatabaseRegistry.playerDB != null)
        {
            DatabaseRegistry.playerDB.Player.SetPosition(player.entity.transform.position);
            DatabaseRegistry.playerDB.Save();
            Debug.Log("Player Data Saved");
        }
        if (DatabaseRegistry.enemiesDB != null)
        {
            if (!DatabaseRegistry.enemiesDB.Enemies.enemiesDictionary.ContainsKey(DatabaseRegistry.playerDB.Player.currentSceneUUID))
            {
                DatabaseRegistry.enemiesDB.Enemies.enemiesDictionary.Add(DatabaseRegistry.playerDB.Player.currentSceneUUID, new List<EnemyData>());
            }


            DatabaseRegistry.enemiesDB.Enemies.enemiesDictionary[DatabaseRegistry.playerDB.Player.currentSceneUUID].Clear();
            Entity enemiesReferences = Entity.FindEntityByName("EnemiesReferences");
            if (enemiesReferences == null)
            {
                Debug.LogWarning("EnemiesReferences not found");
            }
            else
            {
                foreach (Entity child in enemiesReferences.GetChildren())
                {
                    if (child.Name.Contains("_Reference"))
                    {
                        continue;
                    }

                    EnemyData data = new EnemyData();
                    data.entityID = child.ID;
                    data.enemyPositionX = child.transform.position.x;
                    data.enemyPositionY = child.transform.position.y;
                    data.enemyPositionZ = child.transform.position.z;

                    Health health = child.GetComponent<Health>();
                    if (health != null)
                    {
                        data.hp = health.GetActualHealth();
                    }

                    switch (child.Name)
                    {
                        default:
                            break;
                    }

                    if (child.Name.Contains("Golem"))
                    {
                        Golem golem = child.GetComponent<Golem>();
                        data.enemyType = golem.type;
                        data.shieldHP = 0;
                    }
                    else if (child.Name.Contains("Blob"))
                    {
                        Blob blob = child.GetComponent<Blob>();
                        data.blobStage = blob.Stage;

                        if (child.Name.Contains("WaterBlob"))
                        {
                            data.enemyType = "WaterBlob";
                        }
                        else if (child.Name.Contains("GroundBlob"))
                        {
                            data.enemyType = "GroundBlob";
                        }
                        else if (child.Name.Contains("FireBlob"))
                        {
                            data.enemyType = "FireBlob";
                        }
                        else
                        {
                            data.enemyType = "Blob";
                        }


                    }

                    DatabaseRegistry.enemiesDB.Enemies.enemiesDictionary[DatabaseRegistry.playerDB.Player.currentSceneUUID].Add(data);
                }
                DatabaseRegistry.enemiesDB.Save();
                Debug.Log("Enemy Data Saved");
            }

        }
        if (DatabaseRegistry.puzzlesDB.Exists())
        {
            DatabaseRegistry.puzzlesDB.Save();
        }
        if (DatabaseRegistry.spawnersDB != null)
        {
            DatabaseRegistry.spawnersDB.Spawners.spawners.Clear();
            Entity enemySpawners = Entity.FindEntityByName("EnemySpawners");
            if (enemySpawners == null)
            {
                Debug.LogWarning("EnemySpawners not found");
            }
            else
            {
                foreach (Entity child in enemySpawners.GetChildren())
                {
                    Spawner spawner = child.GetComponent<Spawner>();
                    if (spawner == null)
                    {
                        continue;
                    }

                    SpawnerData sData = new SpawnerData();
                    sData.spawnerID = spawner.spawnerID;
                    sData.alreadySpawned = spawner.alreadySpawned;

                    DatabaseRegistry.spawnersDB.Spawners.spawners.Add(sData);
                }
                Debug.Log(DatabaseRegistry.spawnersDB.Spawners.spawners.Count + " spawners saved");
                DatabaseRegistry.spawnersDB.Save();
            }
        }
    }
};