using Loopie;
using System;
using System.Collections.Generic;

public class PlayerData
{
    // Health
    public int maxHealth = 8;
    public int currentHealth = 8;

    // Items, Abilities and Gems
    public bool gemAirCollected = false;
    public bool gemFireCollected = false;
    public bool gemWaterCollected = false;

    public bool hasGrappling = false;
    public bool hasBurner = false;

    // Scenes and position
    public string currentSceneUUID = "";
    public string previousSceneUUID = "";

    public Dictionary<string, PlayerPosition> positions = new Dictionary<string, PlayerPosition>();

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

    public void GetPosition(ref Vector3 pos)
    {
        if(positions.ContainsKey(currentSceneUUID))
        {
            PlayerPosition position = positions[currentSceneUUID];
            pos.x = position.x;
            pos.y = position.y;
            pos.z = position.z;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        if (positions.ContainsKey(currentSceneUUID))
        {
            PlayerPosition position = new PlayerPosition();
            position.x = pos.x;
            position.y = pos.y;
            position.z = pos.z;
            positions[currentSceneUUID] = position;
        }
        else
        {
            PlayerPosition position = new PlayerPosition();
            position.x = pos.x;
            position.y = pos.y;
            position.z = pos.z;
            positions.Add(currentSceneUUID, position);
        }
    }
}

public class PlayerPosition
{
    public float x;
    public float y;
    public float z;
}