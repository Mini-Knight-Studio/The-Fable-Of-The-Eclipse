using Loopie;
using System;

public class HealthHUD : Component
{
    public int maxHealthIcons = 10;
    public Entity healthIconsParent;

    private HealthSlot[] healthIcons;
    private int lastKnownHealth = -1;
    private int lastKnownMaxHealth = -1;

    public Entity bg4;
    public Entity bg6;
    public Entity bg8;
    public Entity bg10;

    void OnCreate()
    {
        healthIcons = new HealthSlot[maxHealthIcons];

        int childCount = entity.GetChildren().Count;
        for (int i = 0; i < childCount; i++)
        {
            if (i >= maxHealthIcons)
                break;
            Entity healthSlotEntity = healthIconsParent.GetChildByName("Life_" + (i));
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
        // Toggle the background layouts based on maxHealth
        if (bg4 != null) bg4.SetActive(maxHealth <= 8);
        if (bg6 != null) bg6.SetActive(maxHealth > 8 && maxHealth <= 12);
        if (bg8 != null) bg8.SetActive(maxHealth > 12 && maxHealth <= 16);
        if (bg10 != null) bg10.SetActive(maxHealth > 16);

        int numActiveSlots = (maxHealth + 1) / 2;

        if (numActiveSlots > maxHealthIcons)
            numActiveSlots = maxHealthIcons;

        for (int i = 0; i < maxHealthIcons; i++)
        {
            var icon = healthIcons[i];
            if (icon == null) continue;

            if (i < numActiveSlots)
            {
                icon.entity.SetActive(true);
                icon.Unlock();

                if (currentHealth >= 2)
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

                currentHealth -= 2;
            }
            else
            {
                icon.entity.SetActive(true);
                icon.Lock();
            }
        }
    }
};