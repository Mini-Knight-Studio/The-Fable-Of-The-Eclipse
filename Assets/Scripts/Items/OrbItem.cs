using System;
using Loopie;

public class OrbItem : Component
{
    public int orbType = 0; 

    public Entity vfx;
    public Entity uiManager;
    public string popUpName;

    private bool alreadyCollected = false;

    void OnCreate()
    {
        if (DatabaseRegistry.playerDB != null)
        {
            if (orbType == 0 && DatabaseRegistry.playerDB.Player.gemAirCollected) alreadyCollected = true;
            else if (orbType == 1 && DatabaseRegistry.playerDB.Player.gemWaterCollected) alreadyCollected = true;
            else if (orbType == 2 && DatabaseRegistry.playerDB.Player.gemFireCollected) alreadyCollected = true;

            if (alreadyCollected)
            {
                entity.SetActive(false);
            }
        }
    }

    void OnUpdate()
    {
        if (alreadyCollected) return;

        BoxCollider col = entity.GetComponent<BoxCollider>();

        if (col != null && col.HasCollided)
        {
            switch (orbType)
            {
                case 0:
                    DatabaseRegistry.playerDB.Player.gemAirCollected = true;
                    break;
                case 1:
                    DatabaseRegistry.playerDB.Player.gemWaterCollected = true;
                    break;
                case 2:
                    DatabaseRegistry.playerDB.Player.gemFireCollected = true;
                    break;
                default:
                    break;
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
        if (uiManager != null)
        {
            UIPopupManager popup = uiManager.GetComponent<UIPopupManager>();
            if (popup != null) popup.ShowPopup(popUpName);
        }
    }
};