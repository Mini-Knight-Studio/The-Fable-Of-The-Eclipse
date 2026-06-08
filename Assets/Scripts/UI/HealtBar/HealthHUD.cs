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

    // Unlock animation properties
    private Entity unlockAnimEntity;
    private SpriteAnimator unlockAnimator;
    private bool hasUnlockAnimator = false;
    private RectTransform unlockRectTransform;
    private bool hasUnlockRect = false;

    private const string BAR_UNLOCK1_UUID = "d1a1af3b-4516-49ec-a27d-4a9b163fbbcc";
    private const string BAR_UNLOCK2_UUID = "ba9b0e41-333f-46c8-9c2c-e367fa1367c2";
    private const string BAR_UNLOCK3_UUID = "ff1ae631-6e59-44ee-95fd-fd0bf23fe4bb";

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

        // Find background bar entities
        barBG4  = entity.GetChildByName("BarBG_4");
        barBG6  = entity.GetChildByName("BarBG_6");
        barBG8  = entity.GetChildByName("BarBG_8");
        barBG10 = entity.GetChildByName("BarBG_10");

        // Find unlock animator child
        unlockAnimEntity = entity.GetChildByName("UnlockAnim");
        if (unlockAnimEntity != null)
        {
            Debug.Log("[HealthHUD] Found UnlockAnim entity: " + unlockAnimEntity.Name + ", Parent: " + (unlockAnimEntity.Parent != null ? unlockAnimEntity.Parent.Name : "None"));
            if (unlockAnimEntity.HasComponent<SpriteAnimator>())
            {
                unlockAnimator = unlockAnimEntity.GetComponent<SpriteAnimator>();
                hasUnlockAnimator = true;
                Debug.Log("[HealthHUD] Found SpriteAnimator component on UnlockAnim.");
            }
            else
            {
                Debug.Log("[HealthHUD] SpriteAnimator component NOT found on UnlockAnim!");
            }

            if (unlockAnimEntity.HasComponent<RectTransform>())
            {
                unlockRectTransform = unlockAnimEntity.GetComponent<RectTransform>();
                hasUnlockRect = true;
                Debug.Log("[HealthHUD] Found RectTransform component on UnlockAnim. Size: " + unlockRectTransform.size + ", AnchoredPos: " + unlockRectTransform.anchored_position);
            }
            else
            {
                Debug.Log("[HealthHUD] RectTransform component NOT found on UnlockAnim!");
            }
            unlockAnimEntity.SetActive(false);
        }
        else
        {
            Debug.Log("[HealthHUD] UnlockAnim entity NOT found as child of " + entity.Name + "!");
        }
    }

    void OnUpdate()
    {
        if (Player.Instance == null) return;
        if (Player.Instance.PlayerHealth == null) return;

        int currentHealth = Player.Instance.PlayerHealth.GetActualHealth();
        int maxHealth = Player.Instance.PlayerHealth.GetMaxHealth();

        // First frame initialization (ensure we don't trigger unlock animation on startup)
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
            // Detect unlock event (maxHealth increased)
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
        Debug.Log("[HealthHUD] FlashUnlockAnimation started. oldSlots=" + oldSlots + ", newSlots=" + newSlots + ", hasUnlockAnimator=" + hasUnlockAnimator + ", hasUnlockRect=" + hasUnlockRect);
        if (hasUnlockAnimator)
        {
            // Select correct sprite animation sheet based on the new slot count
            // 4 to 6 slots: BarUnlock1SS
            // 6 to 8 slots: BarUnlock2SS
            // 8 to 10 slots: BarUnlock3SS
            if (newSlots == 5 || newSlots == 6)
            {
                Debug.Log("[HealthHUD] Playing BarUnlock1SS");
                unlockAnimator.TextureUUID = BAR_UNLOCK1_UUID;
                unlockAnimator.SetGrid(6, 3);
                unlockAnimator.FrameCount = 18;
                unlockAnimator.FPS = 18;
                if (hasUnlockRect)
                {
                    unlockRectTransform.size = new Vector2(80f, 54f);
                    unlockRectTransform.anchored_position = new Vector2(0f, -20.5f);
                }
            }
            else if (newSlots == 7 || newSlots == 8)
            {
                Debug.Log("[HealthHUD] Playing BarUnlock2SS");
                unlockAnimator.TextureUUID = BAR_UNLOCK2_UUID;
                unlockAnimator.SetGrid(6, 3);
                unlockAnimator.FrameCount = 18;
                unlockAnimator.FPS = 18;
                if (hasUnlockRect)
                {
                    unlockRectTransform.size = new Vector2(80f, 54f);
                    unlockRectTransform.anchored_position = new Vector2(0f, -20.5f);
                }
            }
            else if (newSlots == 9 || newSlots == 10)
            {
                Debug.Log("[HealthHUD] Playing BarUnlock3SS");
                unlockAnimator.TextureUUID = BAR_UNLOCK3_UUID;
                unlockAnimator.SetGrid(7, 6);
                unlockAnimator.FrameCount = 42;
                unlockAnimator.FPS = 18;
                if (hasUnlockRect)
                {
                    unlockRectTransform.size = new Vector2(80f, 31.5f);
                    unlockRectTransform.anchored_position = new Vector2(0f, -9.25f);
                }
            }
            else
            {
                Debug.Log("[HealthHUD] Warning: newSlots (" + newSlots + ") is not mapped. No custom grid/texture applied.");
            }

            unlockAnimator.StartFrame = 0;
            unlockAnimator.Loop = false;

            unlockAnimEntity.SetActive(true);
            unlockAnimator.Play();

            // Wait for animation to finish
            float duration = (float)unlockAnimator.FrameCount / unlockAnimator.FPS;
            Debug.Log("[HealthHUD] Waiting for animator duration: " + duration + "s");
            yield return new WaitForSeconds(duration + 0.05f);

            unlockAnimEntity.SetActive(false);
            unlockAnimator.Stop(true);
            Debug.Log("[HealthHUD] Animator finished and deactivated.");
        }
        else
        {
            Debug.Log("[HealthHUD] Warning: hasUnlockAnimator is false, skipping SpriteAnimator.");
        }

        // Flash the newly unlocked slots with a bright effect
        for (int flash = 0; flash < 4; flash++)
        {
            for (int i = oldSlots; i < newSlots && i < maxHealthIcons; i++)
            {
                var icon = healthIcons[i];
                if (icon == null) continue;
                icon.FlashTint(flash % 2 == 0);
            }
            yield return new WaitForSeconds(0.1f);
        }

        // Reset tint on newly unlocked slots
        for (int i = oldSlots; i < newSlots && i < maxHealthIcons; i++)
        {
            var icon = healthIcons[i];
            if (icon == null) continue;
            icon.ResetTint();
        }
    }
};