using System;
using Loopie;

public class HealItem : Component
{
    [Header("Configuration")]
    public int healAmount = 1;
    public bool canIncreaseMaxHealth = false;
    public int maxHealthIncreaseAmount = 1;
    public string uniqueId = "Potion_1";
    [Header("Feedback")]
    public Entity vfx;

    private bool alreadyCollected = false;
    BoxCollider triggerDetection;

    void OnCreate()
    {
        if (PlayerStats.collectedItems != null && PlayerStats.collectedItems.Contains(uniqueId))
        {
            alreadyCollected = true;
            entity.SetActive(false);
        }
        triggerDetection = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        if (alreadyCollected) return;

        if(triggerDetection != null && triggerDetection.HasCollided)
        {
            if (canIncreaseMaxHealth)
                Player.Instance.PlayerHealth.IncreaseMaxHealth(maxHealthIncreaseAmount);
            Player.Instance.PlayerHealth.Heal(healAmount);

            if (!PlayerStats.collectedItems.Contains(uniqueId))
            {
                PlayerStats.collectedItems.Add(uniqueId);
            }

            TriggerVFX();

            alreadyCollected = true;
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