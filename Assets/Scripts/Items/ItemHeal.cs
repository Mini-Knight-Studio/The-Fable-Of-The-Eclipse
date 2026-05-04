using System;
using Loopie;

public class HealItem : Component
{
    [Header("Identity")]
    public string healItemID = "UNASSIGNED_HEALITEM";

    [Header("Configuration")]
    public int healAmount = 1;
    public bool canIncreaseMaxHealth = false;
    public int maxHealthIncreaseAmount = 1;

    [Header("Feedback")]
    public Entity vfx;

    private bool collected = false;
    BoxCollider triggerDetection;

    void OnCreate()
    {
        if (DatabaseRegistry.levelsDB.Levels.IsHealingItemCollected(healItemID))
        {
            collected = true;
            entity.SetActive(false);
        }
        triggerDetection = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        if (collected) return;

        if(triggerDetection != null && triggerDetection.HasCollided)
        {
            if (canIncreaseMaxHealth)
            Player.Instance.PlayerHealth.IncreaseMaxHealth(maxHealthIncreaseAmount);
            Player.Instance.PlayerHealth.Heal(healAmount);

            DatabaseRegistry.levelsDB.Levels.SetHealingItemCollected(healItemID);

            TriggerVFX();

            collected = true;
            entity.SetActive(false);

        }
    }

    private void TriggerVFX()
    {
        if (vfx != null)
        {
            vfx.transform.position = entity.transform.position;
            vfx.SetActive(true);

            AudioSource sfx = vfx.GetComponent<AudioSource>();
            if (sfx != null) sfx.Play();
        }
    }
}; 