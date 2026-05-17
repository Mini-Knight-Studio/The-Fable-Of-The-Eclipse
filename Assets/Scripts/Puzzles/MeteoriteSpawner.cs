using System;
using Loopie;

class MeteoriteSpawner : Component
{
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

    [Header("Audio Feedback")]
    public Entity spawnWaveSFX;

    private Entity[] validSpawnPoints;
    private int realSpawnCount = 0;
    private float timer;

    // Tracks the previous frame's state to catch the exact moment the sequence ends
    private GameManager.GameState lastState;

    void OnCreate()
    {
        timer = spawnCooldown;
        lastState = GameManager.state;

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
        // If the game just transitioned out of PAUSE and back into DEFAULT, 
        // it means the volcano sequence just finished up.
        if (lastState == GameManager.GameState.PAUSE && GameManager.state == GameManager.GameState.DEFAULT)
        {
            timer = spawnCooldown; // Reset timer to give the player a fair head start
            Console.WriteLine("[MeteoriteSpawner] Cutscene finished. Resetting spawn timer.");
        }

        // Always record the current state for the next frame's comparison
        lastState = GameManager.state;

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
        if (spawnWaveSFX != null)
        {
            spawnWaveSFX.GetComponent<AudioSource>().Play();
        }

        for (int i = 0; i < realSpawnCount; i++)
        {
            Entity currentPoint = validSpawnPoints[i];

            if (currentPoint != null)
            {
                Entity newClone = meteoritePrefab.Clone(true);

                if (newClone != null)
                {
                    newClone.Name = newClone.Name.Replace("_Reference_", "").Replace("_Reference", "");

                    newClone.transform.position = currentPoint.transform.position;
                    newClone.transform.rotation = currentPoint.transform.rotation;

                    newClone.SetActive(true);
                }
            }
        }
    }
}