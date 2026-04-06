using System;
using Loopie;

public class OrbHUD : Component
{
    public string playerName = "Player";
    public string orbPrefix = "HUD_Orb_";
    public int maxOrbs = 3;

    private OrbInventory playerInventory;
    private Entity[] orbIcons;
    private int lastKnownOrbs = -1;

    void OnCreate()
    {
        Entity playerEntity = Entity.FindEntityByName(playerName);
        if (playerEntity != null)
        {
            playerInventory = playerEntity.GetComponent<OrbInventory>();
        }

        orbIcons = new Entity[maxOrbs];

        for (int i = 0; i < maxOrbs; i++)
        {
            string searchName = orbPrefix + (i + 1).ToString();
            orbIcons[i] = Entity.FindEntityByName(searchName);

            if (orbIcons[i] != null)
            {
                orbIcons[i].SetActive(false);
            }
        }
    }

    void OnUpdate()
    {
        if (playerInventory == null) return;

        int currentOrbs = playerInventory.orbsCollected;

        if (currentOrbs != lastKnownOrbs)
        {
            UpdateIcons(currentOrbs);
            lastKnownOrbs = currentOrbs;
        }
    }

    private void UpdateIcons(int currentOrbs)
    {
        for (int i = 0; i < maxOrbs; i++)
        {
            if (orbIcons[i] == null) continue;

            if (i < currentOrbs)
            {
                orbIcons[i].SetActive(true);
            }
            else
            {
                orbIcons[i].SetActive(false);
            }
        }
    }
}; 