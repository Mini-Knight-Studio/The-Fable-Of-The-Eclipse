using System;
using System.Collections.Generic;
using Loopie;

class MeteoriteSpawner : Component
{
    public float spawnCooldown = 5.0f;
    public Entity meteoritePrefab;

    [Header("Spawn Points")]
    public Entity spawnPointEntity0; public Entity spawnPointEntity1; public Entity spawnPointEntity2;
    public Entity spawnPointEntity3; public Entity spawnPointEntity4; public Entity spawnPointEntity5;
    public Entity spawnPointEntity6; public Entity spawnPointEntity7; public Entity spawnPointEntity8;
    public Entity spawnPointEntity9;

    [Header("Audio Feedback")]
    public Entity spawnWaveSFX;

    private List<Entity> meteoritePool = new List<Entity>();
    private float timer;
    private GameManager.GameState lastState;

    void OnCreate()
    {
        timer = spawnCooldown;
        lastState = GameManager.state;
        if (meteoritePrefab != null) meteoritePrefab.SetActive(false);
    }

    void OnUpdate()
    {
        if (lastState == GameManager.GameState.PAUSE && GameManager.state == GameManager.GameState.DEFAULT)
            timer = spawnCooldown;

        lastState = GameManager.state;
        if (GameManager.state != GameManager.GameState.DEFAULT) return;
        if (meteoritePrefab == null) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnMeteoriteWave();
            timer = spawnCooldown;
        }
    }

    private Entity GetPooledMeteorite()
    {
        foreach (var m in meteoritePool)
        {
            if (m != null && !m.GetComponent<Meteorite>().inUse) return m;
        }

        Entity newClone = meteoritePrefab.Clone(true);
        newClone.Name = "Meteorite_Pooled";
        meteoritePool.Add(newClone);
        return newClone;
    }

    private void SpawnMeteoriteWave()
    {
        if (spawnWaveSFX != null) spawnWaveSFX.GetComponent<AudioSource>().Play();

        Entity[] points = { spawnPointEntity0, spawnPointEntity1, spawnPointEntity2, spawnPointEntity3, spawnPointEntity4,
                            spawnPointEntity5, spawnPointEntity6, spawnPointEntity7, spawnPointEntity8, spawnPointEntity9 };

        foreach (var point in points)
        {
            if (point != null)
            {
                Entity m = GetPooledMeteorite();
                m.GetComponent<Meteorite>().Fire(point.transform.position, point.transform.rotation);
            }
        }
    }
}