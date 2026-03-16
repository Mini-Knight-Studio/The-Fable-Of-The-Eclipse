using System;
using Loopie;

class WinLoseCanvas : Component
{
    private Entity loseCanvas;
    private Entity winCanvas;

    void OnCreate()
    {
        winCanvas = entity.GetChild(0);
        loseCanvas = entity.GetChild(1);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
    }

    void Win()
    {
        loseCanvas.SetActive(false);
        winCanvas.SetActive(true);
    }
    void Lose()
    {
        loseCanvas.SetActive(true);
        winCanvas.SetActive(false);
    }

    void OnUpdate()
    {
        if(Input.IsKeyDown(KeyCode.N) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_LEFT_SHOULDER))
        {
            Win();
        }

        if(Input.IsKeyDown(KeyCode.M) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_RIGHT_SHOULDER))
        {
            Lose();
        }
    }
};
