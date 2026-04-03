using System;
using Loopie;

class WinLoseSwap : Component
{
    public string loseSceneUUID;
    public string winSceneUUID;

    void Win()
    {
        SceneManager.LoadSceneByID(winSceneUUID);
    }

    void Lose()
    {
        SceneManager.LoadSceneByID(loseSceneUUID);
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
