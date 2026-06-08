using Loopie;
using System;
using System.Collections;

public class HealthHUD : Component
{
    public int maxHealthIcons = 10;

    private HealthSlot[] healthIcons;
    private int lastKnownHealth = -1;
    private int lastKnownMaxHealth = -1;

    // Background bar entities (found by name)
    private Entity barBG4;
    private Entity barBG6;
    private Entity barBG8;
    private Entity barBG10;

    // Unlock animation references
    public Entity unlockAnim1Entity;
    public Entity unlockAnim2Entity;
    public Entity unlockAnim3Entity;

    private SpriteAnimator unlockAnimator1;
    private SpriteAnimator unlockAnimator2;
    private SpriteAnimator unlockAnimator3;

    void OnCreate()
    {
        healthIcons = new HealthSlot[maxHealthIcons];

        int childCount = entity.GetChildren().Count;
        for (int i = 0; i < childCount; i++)
        {
            if (i >= maxHealthIcons)
                break;
            Entity healthSlotEntity = entity.GetChildByName("Life_" + (i));
            if (healthSlotEntity != null)
            {
                HealthSlot healthSlot = healthSlotEntity.GetComponent<HealthSlot>();
                if (healthSlot != null)
                {
                    healthIcons[i] = healthSlot;
                }
            }
        }

        barBG4  = entity.GetChildByName("BarBG_4");
        barBG6  = entity.GetChildByName("BarBG_6");
        barBG8  = entity.GetChildByName("BarBG_8");
        barBG10 = entity.GetChildByName("BarBG_10");

        if (unlockAnim1Entity == null) unlockAnim1Entity = entity.GetChildByName("UnlockAnim1");
        if (unlockAnim1Entity != null && unlockAnim1Entity.HasComponent<SpriteAnimator>())
        {
            unlockAnimator1 = unlockAnim1Entity.GetComponent<SpriteAnimator>();
            unlockAnim1Entity.SetActive(false);
        }

        if (unlockAnim2Entity == null) unlockAnim2Entity = entity.GetChildByName("UnlockAnim2");
        if (unlockAnim2Entity != null && unlockAnim2Entity.HasComponent<SpriteAnimator>())
        {
            unlockAnimator2 = unlockAnim2Entity.GetComponent<SpriteAnimator>();
            unlockAnim2Entity.SetActive(false);
        }

        if (unlockAnim3Entity == null) unlockAnim3Entity = entity.GetChildByName("UnlockAnim3");
        if (unlockAnim3Entity != null && unlockAnim3Entity.HasComponent<SpriteAnimator>())
        {
            unlockAnimator3 = unlockAnim3Entity.GetComponent<SpriteAnimator>();
            unlockAnim3Entity.SetActive(false);
        }
    }

    void OnUpdate()
    {
        if (Player.Instance == null) return;
        if (Player.Instance.PlayerHealth == null) return;

        int currentHealth = Player.Instance.PlayerHealth.GetActualHealth();
        int maxHealth = Player.Instance.PlayerHealth.GetMaxHealth();

        if (lastKnownMaxHealth == -1)
        {
            lastKnownHealth = currentHealth;
            lastKnownMaxHealth = maxHealth;
            Debug.Log("[HealthHUD] First frame init: lastKnownHealth=" + lastKnownHealth + ", lastKnownMaxHealth=" + lastKnownMaxHealth);
            UpdateIcons(currentHealth, maxHealth);
            UpdateBarBackground(maxHealth);
            return;
        }

        if (currentHealth != lastKnownHealth || maxHealth != lastKnownMaxHealth)
        {
            Debug.Log("[HealthHUD] Value changed! Health: " + lastKnownHealth + " -> " + currentHealth + ", MaxHealth: " + lastKnownMaxHealth + " -> " + maxHealth);
            if (maxHealth > lastKnownMaxHealth)
            {
                int oldSlots = lastKnownMaxHealth / 2;
                int newSlots = maxHealth / 2;
                Debug.Log("[HealthHUD] Unlock detected: oldSlots=" + oldSlots + ", newSlots=" + newSlots);
                StartCoroutine(FlashUnlockAnimation(oldSlots, newSlots));
            }

            UpdateIcons(currentHealth, maxHealth);
            UpdateBarBackground(maxHealth);
            lastKnownHealth = currentHealth;
            lastKnownMaxHealth = maxHealth;
        }
    }

    private void UpdateIcons(int currentHealth, int maxHealth)
    {
        int numActiveSlots = maxHealth / 2;

        if (numActiveSlots > maxHealthIcons)
            numActiveSlots = maxHealthIcons;

        int remainingHealth = currentHealth;

        for (int i = 0; i < maxHealthIcons; i++)
        {
            var icon = healthIcons[i];
            if (icon == null) continue;

            if (i < numActiveSlots)
            {
                icon.entity.SetActive(true);

                int slotHP = remainingHealth >= 2 ? 2 : (remainingHealth >= 1 ? 1 : 0);
                icon.UpdateVisuals(slotHP);
                icon.Unlock();
                remainingHealth -= 2;
            }
            else
            {
                icon.entity.SetActive(true);
                icon.Lock();
            }
        }
    }

    private void UpdateBarBackground(int maxHealth)
    {
        int slots = maxHealth / 2;
        if (barBG4  != null) barBG4.SetActive(slots <= 4);
        if (barBG6  != null) barBG6.SetActive(slots > 4 && slots <= 6);
        if (barBG8  != null) barBG8.SetActive(slots > 6 && slots <= 8);
        if (barBG10 != null) barBG10.SetActive(slots > 8);
    }

    private IEnumerator FlashUnlockAnimation(int oldSlots, int newSlots)
    {
        Entity targetEntity = null;
        SpriteAnimator targetAnimator = null;

        if (newSlots == 5 || newSlots == 6)
        {
            targetEntity = unlockAnim1Entity;
            targetAnimator = unlockAnimator1;
        }
        else if (newSlots == 7 || newSlots == 8)
        {
            targetEntity = unlockAnim2Entity;
            targetAnimator = unlockAnimator2;
        }
        else if (newSlots == 9 || newSlots == 10)
        {
            targetEntity = unlockAnim3Entity;
            targetAnimator = unlockAnimator3;
        }

        if (targetEntity != null && targetAnimator != null)
        {
            Debug.Log("[HealthHUD] Playing unlock animation on: " + targetEntity.Name);
            targetEntity.SetActive(true);
            targetAnimator.Play();

            // Wait for animation to finish based on editor settings
            float duration = (float)targetAnimator.FrameCount / targetAnimator.FPS;
            if (duration <= 0f) duration = 1.0f; // fallback
            
            Debug.Log("[HealthHUD] Waiting for animator duration: " + duration + "s");
            yield return new WaitForSeconds(duration + 0.15f);

            targetEntity.SetActive(false);
            targetAnimator.Stop(true);
            Debug.Log("[HealthHUD] Animator finished and deactivated.");
        }
        else
        {
            Debug.Log("[HealthHUD] Warning: No unlock animator found for slot transition " + oldSlots + " -> " + newSlots);
        }
    }
};