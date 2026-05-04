using System;
using Loopie;

public class PlayerData
{
    // Health
    public int maxHealth = 6;
    public int currentHealth = 6;

    // Items, Abilities and Gems
    public bool gemAirCollected = false;
    public bool gemFireCollected = false;
    public bool gemWaterCollected = false;

    public bool hasGrappling = false;
    public bool hasBurner = false;

    // Scenes and position
    public string currentSceneUUID = "";
    public string previousSceneUUID = "";
    public void SetCurrentScene(string newSceneUUID)
    {
        if (currentSceneUUID == newSceneUUID) return;
        previousSceneUUID = currentSceneUUID;
        currentSceneUUID = newSceneUUID;
    }

    public void ResetScenes()
    {
        currentSceneUUID = "";
        previousSceneUUID = "";
    }

    public float playerPositionX = 0.0f;
    public float playerPositionY = 0.0f;
    public float playerPositionZ = 0.0f;
}