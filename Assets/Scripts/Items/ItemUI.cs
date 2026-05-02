using System;
using Loopie;

public class WeaponItem : Component
{
    [Header("Configuration")]
    public string uniqueId = "Grapple";
    public string popupName = "Popup_Grapple";
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

       
        if (triggerDetection != null && triggerDetection.HasCollided)
        {
            if (!PlayerStats.collectedItems.Contains(uniqueId))
            {
                PlayerStats.collectedItems.Add(uniqueId);
            }

            TriggerVFX();

            ShowUIPopup();

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

    private void ShowUIPopup()
    {
        if (UIPopupManager.Instance != null)
        {
            UIPopupManager.Instance.ShowPopup(popupName);
        }
    }
}; 