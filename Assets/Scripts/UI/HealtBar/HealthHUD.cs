using Loopie;
using System;

public class HealthHUD : Component
{
    public int maxHealthIcons = 5;

    private HealthSlot[] healthIcons;
    private int lastKnownHealth = -1;
    private int lastKnownMaxHealth = -1;

    void OnCreate()
    {
        healthIcons = new HealthSlot[maxHealthIcons];

        int childCount = entity.GetChildren().Count;
        for (int i = 0; i < childCount; i++)
        {
            if (i >= maxHealthIcons)
                break;
            Entity healthSlotEntity = entity.GetChildByName("Life_" + (i));
            if(healthSlotEntity != null)
            {
                HealthSlot healthSlot = healthSlotEntity.GetComponent<HealthSlot>();
                if (healthSlot != null)
                {
                    healthIcons[i] = healthSlot;
                }
            }
        }
    }

    void OnUpdate()
    {
        if (Player.Instance.PlayerHealth == null) return;

        int currentHealth = Player.Instance.PlayerHealth.GetActualHealth();
        int maxHealth = Player.Instance.PlayerHealth.GetMaxHealth();

        if (currentHealth != lastKnownHealth || maxHealth != lastKnownMaxHealth)
        {
            UpdateIcons(currentHealth, maxHealth);
            lastKnownHealth = currentHealth;
            lastKnownMaxHealth = maxHealth;
        }
    }

    private void UpdateIcons(int currentHealth, int maxHealth)
    {
        int numActiveSlots = (maxHealth + 3) / 4;

        if (numActiveSlots > maxHealthIcons)
            numActiveSlots = maxHealthIcons;

        for (int i = 0; i < maxHealthIcons; i++)
        {
            var icon = healthIcons[i];
            if (icon == null) continue;

            if (i < numActiveSlots)
            {
                icon.entity.SetActive(true);

                if (currentHealth >= 3)
                {
                    icon.UpdateVisuals(2); 
                }
                else if (currentHealth >= 1)
                {
                    icon.UpdateVisuals(1); 
                }
                else
                {
                    icon.UpdateVisuals(0);
                }

                icon.Unlock();

                currentHealth -= 4;
            }
            else
            {
                icon.entity.SetActive(true);
                icon.Lock();
            }
        }
    }
};