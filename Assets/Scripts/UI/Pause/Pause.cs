using System;
using Loopie;

public class Pause : Component
{
    public static Pause Instance { get; private set; }

    public Entity pauseMenuEntity;
    public Entity infoDebugEntity;

    public bool isPaused = false;

    private float inputCooldown = 0.2f;
    private float inputTimer = 0f;

    public static GameManager.GameState previousState;
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

            if (!isPaused)
            {
                pauseMenuEntity.SetActive(false);
                infoDebugEntity.SetActive(false);

                PauseMenu.quickStartAnimations = true;
                PauseMenu.invertedPassPagePlayed = false;

                GameManager.SetState(previousState);
            }
            else
            {
                pauseMenuEntity.SetActive(true);
                infoDebugEntity.SetActive(true);

                if (!pauseMenuEntity.Active)
                {
                    pauseMenuEntity.GetComponent<PauseMenu>().Open();
                }

                previousState = GameManager.state;
                GameManager.SetState(GameManager.GameState.FREEZE);
            }
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (!isPaused)
        {
            pauseMenuEntity.SetActive(false);
            infoDebugEntity.SetActive(false);

            PauseMenu.quickStartAnimations = true;
            PauseMenu.invertedPassPagePlayed = false;

            GameManager.SetState(previousState);
        }
        else
        {
            pauseMenuEntity.SetActive(true);
            infoDebugEntity.SetActive(true);

            if (!pauseMenuEntity.Active)
            {
                pauseMenuEntity.GetComponent<PauseMenu>().Open();
            }

            previousState = GameManager.state;
            GameManager.SetState(GameManager.GameState.FREEZE);
        }
    }
    public void SetStateToPrevious()
    {
        isPaused = false;
        GameManager.SetState(previousState);
    }
};