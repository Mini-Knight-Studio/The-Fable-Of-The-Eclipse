using Loopie;
using System;
using System.Collections;

public class HealthHUD : Component
{
    public int maxHealthIcons = 10;

    private HealthSlot[] healthIcons;
    private int lastKnownHealth = -1;
    private int lastKnownMaxHealth = -1;

    [Header("Healt Icons")]
    public Entity healthIconsParent;

    [Header("Health Bars")]
    public Entity bg4;
    public Entity bg6;
    public Entity bg8;
    public Entity bg10;

    [Header("Health Unlock Anims")]
    public Entity unlock6;
    SpriteAnimator unlock6Anim;
    public float timeUntilUnlock6 = 0.5f;
    public Entity unlock8;
    SpriteAnimator unlock8Anim;
    public float timeUntilUnlock8 = 0.5f;
    public Entity unlock10;
    SpriteAnimator unlock10Anim;
    public float timeUntilUnlock10 = 0.5f;

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

        unlock6Anim = unlock6.GetComponent<SpriteAnimator>();
        unlock8Anim = unlock8.GetComponent<SpriteAnimator>();
        unlock10Anim = unlock10.GetComponent<SpriteAnimator>();

    }

    void OnUpdate()
    {
        if (Player.Instance.PlayerHealth == null) return;

        int currentHealth = Player.Instance.PlayerHealth.GetActualHealth();
        int maxHealth = Player.Instance.PlayerHealth.GetMaxHealth();

        if (currentHealth != lastKnownHealth || maxHealth != lastKnownMaxHealth)
        {
            if(maxHealth != lastKnownMaxHealth)
            {
                UpdateHUD(maxHealth);
            }
            UpdateIcons(currentHealth, maxHealth);

            lastKnownHealth = currentHealth;
            lastKnownMaxHealth = maxHealth;
        }
    }

    private void UpdateIcons(int currentHealth, int maxHealth)
    {

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

    public void UpdateHUD(int maxHealth)
    {
        bg4.SetActive(false);



        if (maxHealth == 8)
        {
            bg4.SetActive(true);
            bg6.SetActive(false);
            bg8.SetActive(false);
            bg10.SetActive(false);
        }
        else if (maxHealth == 12)
        {
            StartCoroutine(AnimateUnlocking(bg4, bg6, unlock6Anim, timeUntilUnlock6));
            bg4.SetActive(true);
            bg6.SetActive(false);
            bg8.SetActive(false);
            bg10.SetActive(false);
        }
        else if (maxHealth == 16)
        {
            StartCoroutine(AnimateUnlocking(bg6, bg8, unlock8Anim, timeUntilUnlock8));

            bg4.SetActive(false);
            bg6.SetActive(true);
            bg8.SetActive(false);
            bg10.SetActive(false);
        }
        else if (maxHealth == 20)
        {
            StartCoroutine(AnimateUnlocking(bg8, bg10, unlock10Anim, timeUntilUnlock10));

            bg4.SetActive(false);
            bg6.SetActive(false);
            bg8.SetActive(true);
            bg10.SetActive(false);
        }
    }


    IEnumerator AnimateUnlocking(Entity oldHUD, Entity newHUD, SpriteAnimator animation, float timeToChange) 
    {
        oldHUD.SetActive(true);
        newHUD.SetActive(false);

        Debug.Log("dsadasdasd");
        animation.entity.SetActive(true);
        float timer = 0f;
        animation.Play();
        while (timer<timeToChange)
        {
            timer+= Time.deltaTime;
            yield return null;
        }

        oldHUD.SetActive(false);
        newHUD.SetActive(true);

        yield return new WaitForSeconds(3);

        animation.entity.SetActive(false);
    }
};