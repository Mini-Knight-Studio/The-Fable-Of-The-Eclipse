using System;
using Loopie;

public class HealthHUD : Component
{
    public int maxHealthIcons = 5;

    private HealthSlot[] healthIcons;
    private int lastKnownHealth = -1;

    void OnCreate()
    {
        healthIcons = new HealthSlot[maxHealthIcons];

        int index = 0;
        foreach (var icon in entity.Children)
        {
            if (index >= maxHealthIcons) 
                break;
            healthIcons[index] = icon.GetComponent<HealthSlot>();
            index++;
        }
    }

    void OnUpdate()
    {
        if (Player.Instance.PlayerHealth == null) return;

        int currentHealth = Player.Instance.PlayerHealth.GetActualHealth();

        if (currentHealth != lastKnownHealth)
        {
            UpdateIcons(currentHealth);
            lastKnownHealth = currentHealth;
        }
    }

    private void UpdateIcons(int currentHealth)
    {
        foreach (var icon in healthIcons)
        {
            if (currentHealth >= 2)
            {
                icon.UpdateVisuals(2);
            }
            else if (currentHealth == 1)
            {
                icon.UpdateVisuals(1);
            }
            else
            {
                icon.UpdateVisuals(0);
            }

            currentHealth -= 2;
        }
    }
};