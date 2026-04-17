using System;
using Loopie;

public class OrbHUD : Component
{
    public string playerName = "Player";
    public string orbPrefix = "HUD_Orb_";
    public int maxOrbs = 3;

    private Entity[] orbIcons;
    private int lastKnownOrbs = -1;

    void OnCreate()
    {
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
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        for (int i = 0; i < maxOrbs; i++)
        {
            if (orbIcons[i] == null) continue;

            switch (i)
            {
                case 0:
                    orbIcons[i].SetActive(DatabaseRegistry.playerDB.Player.gemAirCollected);
                    break;
                case 1:
                    orbIcons[i].SetActive(DatabaseRegistry.playerDB.Player.gemWaterCollected);
                    break;
                case 2:
                    orbIcons[i].SetActive(DatabaseRegistry.playerDB.Player.gemFireCollected);
                    break;
                default:
                    break;
            }
        }

        
    }
}; 