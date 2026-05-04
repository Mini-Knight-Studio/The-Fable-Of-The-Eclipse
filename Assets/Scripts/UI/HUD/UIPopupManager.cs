using System;
using System.Collections;
using Loopie;

public class UIPopupManager : Component
{
    public static UIPopupManager Instance { get; private set; }

    private Entity currentActivePanel = null;
    private bool isWaitingForInput = false;

    void OnCreate()
    {
        Instance = this;
    }

    void OnUpdate()
    {
        if (isWaitingForInput && currentActivePanel != null)
        {
            if (Input.IsKeyPressed(KeyCode.E) ||
                Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_A))
            {
                ClosePopup();
            }
        }
    }

    public void ShowPopup(string panelName)
    {
        Entity panel = Entity.FindEntityByName(panelName);
        if (panel != null)
        {
            panel.SetActive(true);
            currentActivePanel = panel;
            isWaitingForInput = true;

            Time.timeScale = 0.0f;
        }
        else
        {
            Debug.Log("UIPopupManager: No se encontro el panel en la escena llamado " + panelName);
        }
    }

    private void ClosePopup()
    {
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            currentActivePanel = null;
        }

        isWaitingForInput = false;

        Time.timeScale = 1.0f;
    }
};