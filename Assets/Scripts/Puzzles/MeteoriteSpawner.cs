using System;
using Loopie;

class MeteoriteSpawner : Component
{
    [Header("Spawning Settings")]
    public float spawnCooldown = 5.0f;

    [Header("References")]
    public Entity meteoritePrefab;

    [Header("Spawn Points")]
    public Entity spawnPointEntity0;
    public Entity spawnPointEntity1;
    public Entity spawnPointEntity2;
    public Entity spawnPointEntity3;
    public Entity spawnPointEntity4;
    public Entity spawnPointEntity5;
    public Entity spawnPointEntity6;
    public Entity spawnPointEntity7;
    public Entity spawnPointEntity8;
    public Entity spawnPointEntity9;

    private Entity[] validSpawnPoints;
    private int realSpawnCount = 0;
    private float timer;

    void OnCreate()
    {
        timer = spawnCooldown;

        if (meteoritePrefab != null)
        {
            meteoritePrefab.SetActive(false);
        }

        Entity[] tempPoints = new Entity[10] {
            spawnPointEntity0, spawnPointEntity1, spawnPointEntity2, spawnPointEntity3, spawnPointEntity4,
            spawnPointEntity5, spawnPointEntity6, spawnPointEntity7, spawnPointEntity8, spawnPointEntity9
        };

        realSpawnCount = 0;
        for (int i = 0; i < 10; i++)
        {
            if (tempPoints[i] != null)
            {
                realSpawnCount++;
            }
        }

        validSpawnPoints = new Entity[realSpawnCount];
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            if (tempPoints[i] != null)
            {
                validSpawnPoints[index] = tempPoints[i];
                index++;
            }
        }
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) return;
        if (meteoritePrefab == null || realSpawnCount == 0) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnMeteoriteWave();
            timer = spawnCooldown;
        }
    }

    private void SpawnMeteoriteWave()
    {
        for (int i = 0; i < realSpawnCount; i++)
        {
            Entity currentPoint = validSpawnPoints[i];

            if (currentPoint != null)
            {
                Entity newClone = meteoritePrefab.Clone(true);

                if (newClone != null)
                {
                    //copiada historica del spawn de Ana
                    newClone.Name = newClone.Name.Replace("_Reference_", "").Replace("_Reference", "");

                    newClone.transform.position = currentPoint.transform.position;
                    newClone.transform.rotation = currentPoint.transform.rotation;

                    newClone.SetActive(true);
                }
            }
        }
    }
}