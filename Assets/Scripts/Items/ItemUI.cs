using System;
using Loopie;

public class WeaponItem : Component
{
    public string uniqueId = "Grapple"; 
    public string vfxName = "GrappleVFX";
    public string uiManagerName = "HUD";
    public string popupName = "Popup_Grapple";

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
        Entity vfx = Entity.FindEntityByName(vfxName);
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
        Entity uiManager = Entity.FindEntityByName(uiManagerName);
        if (uiManager != null)
        {
            UIPopupManager popup = uiManager.GetComponent<UIPopupManager>();
            if (popup != null) popup.ShowPopup(popupName);
        }
    }
}; 