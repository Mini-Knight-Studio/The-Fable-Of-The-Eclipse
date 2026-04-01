using System;
using Loopie;

public class PlayerData
{
    // Items, Abilities and Gems
    public bool gemEarthCollected = false;
    public bool gemFireCollected = false;
    public bool gemWaterCollected = false;

    public bool hasGrappling = false;
    public bool hasBurner = false;

    // Scenes and position
    public string currentSceneUUID = "";
    public string previousSceneUUID = "";
    public void SetCurrentScene(string newSceneUUID)
    {
        if (currentSceneUUID == newSceneUUID)   return;
        previousSceneUUID = currentSceneUUID;
        currentSceneUUID = newSceneUUID;
    }

    public Vector3 playerPosition = Vector3.Zero;
}