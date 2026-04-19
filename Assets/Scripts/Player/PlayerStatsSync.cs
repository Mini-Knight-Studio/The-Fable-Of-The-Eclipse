using System;
using Loopie;

public class PlayerStatsSync : Component
{
    private Health playerHealth;

    void OnCreate()
    {
        playerHealth = entity.GetComponent<Health>();

        if (!PlayerStats.isInitialized)
        {
            if (playerHealth != null)
            {
                PlayerStats.savedHealth = playerHealth.GetMaxHealth();
            }
            PlayerStats.isInitialized = true;
        }
        else
        {
            if (playerHealth != null)
            {
                playerHealth.actualHealth = PlayerStats.savedHealth;
            }
        }
    }

    void OnUpdate()
    {
        if (playerHealth != null)
        {
            PlayerStats.savedHealth = playerHealth.GetActualHealth();
        }
    }
};