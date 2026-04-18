using System;
using Loopie;

class CheckpointCollider : Component
{
    private BoxCollider collider;
    private bool hasSaved = false;
    public Entity player;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (!hasSaved && collider.IsColliding)
        {
            hasSaved = true;
            if (DatabaseRegistry.playerDB.Exists())
            {
                DatabaseRegistry.playerDB.Player.playerPositionX = player.transform.position.x;
                DatabaseRegistry.playerDB.Player.playerPositionY = player.transform.position.y;
                DatabaseRegistry.playerDB.Player.playerPositionZ = player.transform.position.z;
                DatabaseRegistry.playerDB.Save();
                Debug.Log("Player Data Saved");
            }
            if (DatabaseRegistry.enemiesDB.Exists())
            {
                //Making sure list is empty before looking for all existing enemies at the time the player has saved
                DatabaseRegistry.enemiesDB.Enemies.enemies.Clear(); //(Inside the DB, there is data type Enemies, and then vector enemies)

                Entity enemiesRoot = Entity.FindEntityByName("Enemies");

                foreach (Entity child in enemiesRoot.GetChildren())
                {

                    EnemiesData.EnemyData data = new EnemiesData.EnemyData();
                    data.enemyPositionX = child.transform.position.x;
                    data.enemyPositionY = child.transform.position.y;
                    data.enemyPositionZ = child.transform.position.z;

                    Health health = child.GetComponent<Health>();
                    if (health != null)
                    {
                        data.hp = health.actualHealth;
                    }

                    //Specific data for only some types of enemy
                    Golem golem = child.GetComponent<Golem>();
                    if (golem != null)
                    {
                        data.enemyType = "Golem";
                        data.shieldHP = golem.ShieldLife;
                    }
                    else
                    {
                        Blob blob = child.GetComponent<Blob>();
                        if (blob != null)
                        {
                            data.enemyType = "Blob";
                            data.blobStage = blob.BlobStage;
                        }

                    }

                    DatabaseRegistry.enemiesDB.Enemies.enemies.Add(data);
                }
                DatabaseRegistry.enemiesDB.Save();
                Debug.Log("Enemy Data Saved");
            }
            if (DatabaseRegistry.puzzlesDB.Exists())
            {
                DatabaseRegistry.puzzlesDB.Save();
            }
        }
    }
};