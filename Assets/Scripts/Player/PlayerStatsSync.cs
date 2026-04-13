using System;
using Loopie;

public class PlayerStatsSync : Component
{
    public Health playerHealth;
    public OrbInventory orbInventory;

    void OnCreate()
    {
        playerHealth = entity.GetComponent<Health>();
        orbInventory = entity.GetComponent<OrbInventory>();

        
        if (!PlayerStats.isInitialized)
        {
            if (playerHealth != null)
            {
                PlayerStats.savedHealth = playerHealth.GetMaxHealth();
            }
            PlayerStats.savedOrbs = 0;
            PlayerStats.isInitialized = true;
        }
        else
        {
           
            if (playerHealth != null)
            {
                playerHealth.actualHealth = PlayerStats.savedHealth;
            }

            if (orbInventory != null)
            {
                orbInventory.orbsCollected = PlayerStats.savedOrbs;
            }
        }
    }

    void OnUpdate()
    {
     
        if (playerHealth != null)
        {
            PlayerStats.savedHealth = playerHealth.GetActualHealth();
        }

        if (orbInventory != null)
        {
            PlayerStats.savedOrbs = orbInventory.orbsCollected;
        }
    }
};