using System;
using System.Collections.Generic;
using Loopie;

public class LevelsData
{
    // Key = Type of thing ID, Value = true if opened
    // Convention: Lvl(Name or Number)_(Type)(Name or Number) --> Example: Lvl1_Chest2 / LvlFire_AcornInTheEnd

    // -------------------------------- Chests --------------------------------
    // Use same ID for both chests and rewards cause there's one for each

    public Dictionary<string, bool> openedChests = new Dictionary<string, bool>();
    public Dictionary<string, bool> collectedRewards = new Dictionary<string, bool>();

    public bool IsChestOpen(string chestID)
    {
        if (openedChests.TryGetValue(chestID, out bool isOpen)) return isOpen;
        return false;
    }

    public void SetChestOpened(string chestID)
    {
        openedChests[chestID] = true;
    }

    public bool IsRewardCollected(string chestID)
    {
        if (collectedRewards.TryGetValue(chestID, out bool isCollected)) return isCollected;
        return false;
    }

    public void SetRewardCollected(string chestID)
    {
        collectedRewards[chestID] = true;
    }

    // -------------------------------- Puzzle Doors --------------------------------
    public Dictionary<string, bool> openedPuzzleDoors = new Dictionary<string, bool>();

    public bool IsPuzzleDoorOpened(string puzzledoorID)
    {
        if (openedPuzzleDoors.TryGetValue(puzzledoorID, out bool isOpen)) return isOpen;
        return false;
    }

    public void SetPuzzleDoorOpened(string puzzledoorID)
    {
        openedPuzzleDoors[puzzledoorID] = true;
    }

    // -------------------------------- Bridges --------------------------------
    public Dictionary<string, bool> pushedBridges = new Dictionary<string, bool>();

    public bool IsBridgePushed(string bridgeID)
    {
        if (pushedBridges.TryGetValue(bridgeID, out bool isPushed)) return isPushed;
        return false;
    }

    public void SetBridgePushed(string bridgeID)
    {
        pushedBridges[bridgeID] = true;
    }

    // -------------------------------- Vines / Burnables --------------------------------
    public Dictionary<string, bool> burnedBlocks = new Dictionary<string, bool>();

    public bool IsBurnableBurned(string burnableID)
    {
        if (burnedBlocks.TryGetValue(burnableID, out bool isBurned)) return isBurned;
        return false;
    }

    public void SetBurnableBurned(string burnableID)
    {
        burnedBlocks[burnableID] = true;
    }

    // -------------------------------- Acorns / Healing Item --------------------------------
    public Dictionary<string, bool> collectedAcorns = new Dictionary<string, bool>();

    public bool IsHealingItemCollected(string acornID)
    {
        if (collectedAcorns.TryGetValue(acornID, out bool isCollected)) return isCollected;
        return false;
    }

    public void SetHealingItemCollected(string acornID)
    {
        collectedAcorns[acornID] = true;
    }
}