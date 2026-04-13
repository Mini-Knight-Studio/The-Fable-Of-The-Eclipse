using System;
using Loopie;

public class PlayerStatsSync : Component
{
    public Player player;
    public OrbInventory orbInventory;

    void OnCreate()
    {
        player = entity.GetComponent<Player>();
        orbInventory = entity.GetComponent<OrbInventory>();

        
        if (!PlayerStats.isInitialized)
        {
            if (player != null)
            {
                PlayerStats.savedHealth = player.PlayerHealth.GetMaxHealth();
            }
            PlayerStats.savedOrbs = 0;
            PlayerStats.isInitialized = true;
        }
        else
        {
           
            if (player != null)
            {
                player.PlayerHealth.ModifyActualHealth(PlayerStats.savedHealth);
            }

            if (orbInventory != null)
            {
                orbInventory.orbsCollected = PlayerStats.savedOrbs;
            }
        }
    }

    void OnUpdate()
    {
     
        if (player.PlayerHealth != null)
        {
            PlayerStats.savedHealth = player.PlayerHealth.GetActualHealth();
        }

        if (orbInventory != null)
        {
            PlayerStats.savedOrbs = orbInventory.orbsCollected;
        }
    }
};