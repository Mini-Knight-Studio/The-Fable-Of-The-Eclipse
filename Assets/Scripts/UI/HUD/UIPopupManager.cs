using System;
using System.Collections;
using Loopie;

public class UIPopupManager : Component
{
    public float showDuration = 3.0f;

    public static UIPopupManager Instance { get; private set; }

    void OnCreate()
    {
        Instance = this;
    }

    void OnUpdate() { }

    public void ShowPopup(string panelName)
    {
        Entity panel = Entity.FindEntityByName(panelName);
        if (panel != null)
        {
            panel.SetActive(true);

            StartCoroutine(HidePopupRoutine(panel));
        }
        else
        {
            Debug.Log("UIPopupManager: No se encontro el panel en la escena llamado " + panelName);
        }
    }

    private IEnumerator HidePopupRoutine(Entity panelToHide)
    {
        float timer = 0f;
        while (timer < showDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (panelToHide != null)
        {
            panelToHide.SetActive(false);
        }
    }
}; 