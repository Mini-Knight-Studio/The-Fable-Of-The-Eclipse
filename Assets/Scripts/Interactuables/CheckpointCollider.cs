using System;
using Loopie;

class CheckpointCollider : Component
{
    private BoxCollider collider;
    private bool hasSaved = false;
    public Player player;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
        player = Player.Instance;
    }

    void OnUpdate()
    {
        if (!hasSaved && collider.IsColliding)
        {
            hasSaved = true;
            if (DatabaseRegistry.playerDB != null)
            {
                DatabaseRegistry.playerDB.Player.playerPositionX = player.transform.position.x;
                DatabaseRegistry.playerDB.Player.playerPositionY = player.transform.position.y;
                DatabaseRegistry.playerDB.Player.playerPositionZ = player.transform.position.z;
                DatabaseRegistry.playerDB.Save();
                Debug.Log("Player Data Saved");
            }
            if (DatabaseRegistry.enemiesDB != null)
            {
                DatabaseRegistry.enemiesDB.Enemies.enemies.Clear();
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

                        EnemiesData.EnemyData data = new EnemiesData.EnemyData();
                        data.entityID = child.ID;
                        data.enemyPositionX = child.transform.position.x;
                        data.enemyPositionY = child.transform.position.y;
                        data.enemyPositionZ = child.transform.position.z;

                        Health health = child.GetComponent<Health>();
                        if (health != null)
                        {
                            data.hp = health.actualHealth;
                        }
                        Debug.Log("DEBUG TYPE: " + child.GetComponent<Enemy>().type);
                        //if (child.GetComponent<Enemy>().type == "Golem")
                        //{
                        //    data.enemyType = "Golem";
                        //    data.shieldHP = child.GetComponent<Golem>().ShieldLife;
                        //}

                        //if (child.GetComponent<Enemy>().type == "Blob")
                        //{
                        //    data.enemyType = "Blob";
                        //    data.blobStage = child.GetComponent<Blob>().BlobStage;
                        //}
                        if (child.HasComponent<Golem>())
                        {
                            Golem golem = child.GetComponent<Golem>();
                            data.enemyType = golem.type;
                            data.shieldHP = golem.ShieldLife;
                        }
                        else if (child.HasComponent<Blob>())
                        {
                            Blob blob = child.GetComponent<Blob>();
                            data.enemyType = "Blob";//For now
                            data.blobStage = blob.BlobStage;
                        }

                        DatabaseRegistry.enemiesDB.Enemies.enemies.Add(data);
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
                           
                        SpawnersData.SpawnerData sData = new SpawnersData.SpawnerData();
                        sData.spawnerID = spawner.spawnerID;
                        sData.alreadySpawned = spawner.alreadySpawned;

                        if (spawner.alreadySpawned)
                        {
                            
                            //foreach (EnemiesData.EnemyData eData in DatabaseRegistry.enemiesDB.Enemies.enemies)
                            //{
                            //    if (/*I need a condition to determine that the enemy corresponds to the spawner*/)
                            //    {
                            //        sData.enemyType = eData.enemyType;
                            //        sData.enemyPositionX = eData.enemyPositionX;
                            //        sData.enemyPositionY = eData.enemyPositionY;
                            //        sData.enemyPositionZ = eData.enemyPositionZ;
                            //        sData.hp = eData.hp;
                            //        sData.shieldHP = eData.shieldHP;
                            //        sData.blobStage = eData.blobStage;
                                    
                            //    }
                            //}
                        }

                        DatabaseRegistry.spawnersDB.Spawners.spawners.Add(sData);
                    }
                    Debug.Log(DatabaseRegistry.spawnersDB.Spawners.spawners.Count + " spawners saved");
                }
            }
        }
    }
};