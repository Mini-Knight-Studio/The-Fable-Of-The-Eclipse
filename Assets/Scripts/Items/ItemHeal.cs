using System;
using Loopie;

public class HealItem : Component
{
    public int healAmount = 1;
    public string uniqueId = "Potion_1"; 
    public string vfxName = "PotionVFX"; 

    private bool alreadyCollected = false;

    void OnCreate()
    {
        if (PlayerStats.collectedItems != null && PlayerStats.collectedItems.Contains(uniqueId))
        {
            alreadyCollected = true;
            entity.SetActive(false);
        }
    }

    void OnUpdate()
    {
        if (alreadyCollected) return;

        BoxCollider col = entity.GetComponent<BoxCollider>();
        if (col != null && col.HasCollided)
        {
            Entity playerEntity = Entity.FindEntityByName("Player");
            if (playerEntity != null)
            {
                Health playerHealth = playerEntity.GetComponent<Health>();
                if (playerHealth != null)
                {
                    if (playerHealth.actualHealth < playerHealth.maxHealth)
                    {
                        playerHealth.actualHealth += healAmount;
                        if (playerHealth.actualHealth > playerHealth.maxHealth)
                        {
                            playerHealth.actualHealth = playerHealth.maxHealth;
                        }

                        if (!PlayerStats.collectedItems.Contains(uniqueId))
                        {
                            PlayerStats.collectedItems.Add(uniqueId);
                        }

                        TriggerVFX();

                        alreadyCollected = true;
                        entity.SetActive(false);
                    }
                }
            }
        }
    }

    private void TriggerVFX()
    {
        Entity vfx = Entity.FindEntityByName(vfxName);
        if (vfx != null)
        {
            vfx.transform.position = entity.transform.position;
            vfx.SetActive(true);

            AudioSource sfx = vfx.GetComponent<AudioSource>();
            if (sfx != null) sfx.Play();
        }
    }
}; 