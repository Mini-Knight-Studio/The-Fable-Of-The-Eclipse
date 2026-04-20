using System;
using Loopie;

public class Pause : Component
{
    public Entity pauseMenuEntity;

    public static bool isPaused = false;

    private float inputCooldown = 0.2f;
    private float inputTimer = 0f;
    void OnCreate()
    {

    }

    void OnUpdate()
    {
        inputTimer += Time.deltaTime;

        if (inputTimer < inputCooldown)
            return;

        if (Input.IsKeyPressed(KeyCode.ESCAPE) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_START))
        {
            isPaused = !isPaused;
            inputTimer = 0f;
        }

        if (!isPaused)
        {
            pauseMenuEntity.SetActive(false);
        }
        else
        {
            pauseMenuEntity.SetActive(true);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }
};