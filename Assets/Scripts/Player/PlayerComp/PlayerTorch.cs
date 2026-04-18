using System;
using Loopie;

public class PlayerTorch : Component
{
    public Entity firePrefab;
    public float burnDuration = 2.0f;

    void OnUpdate()
    {
        if (Input.IsKeyPressed(KeyCode.O) && DatabaseRegistry.playerDB.Player.hasBurner)
        {
            SpawnFire();
        }
    }

    private void SpawnFire()
    {
        if (firePrefab == null) return;

        Entity newFire = firePrefab.Clone(true);

        Vector3 spawnOffset = entity.transform.Forward * 1.5f;
        newFire.transform.position = entity.transform.position + spawnOffset;

        StartCoroutine(ExtinguishFire(newFire));
    }

    private System.Collections.IEnumerator ExtinguishFire(Entity fire)
    {
        float timer = 0.0f;
        while (timer < burnDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        fire.SetActive(false);
    }
}