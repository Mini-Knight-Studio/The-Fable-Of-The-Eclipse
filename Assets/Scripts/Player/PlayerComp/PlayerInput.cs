using System;
using Loopie;

public class PlayerInput : PlayerComponent
{
    public Vector3 moveDirection = Vector3.Zero;
    public bool dashKeyPressed = false;
    public bool attackKeyPressed = false;
    public bool grappleKeyPressed = false;
    public bool torchKeyPressed = false;
    public bool interactKeyPressed = false;
    public bool optionsKeyPressed = false;

    /// <summary>
    /// DEBUG KEYS
    /// </summary>
    public bool godModeKeyPressed = false;

    public void ProcessInputs(GameManager.GameState state)
    {
        ResetInputs();
        switch (state)
        {
            case GameManager.GameState.DEFAULT:
                CollectMovementInput();
                CollectDashInput();
                CollectAttackInput();
                CollectGrappleInput();
                CollectTorchInput();
                CollectOptionsInput();
                CollectInteractInput();
                break;

            case GameManager.GameState.PAUSE:
                CollectInteractInput();
                break;

            case GameManager.GameState.FREEZE:
                break;
        }

        if (state == GameManager.GameState.DEFAULT) { 
            
        }

        CollectDebugInputs();
    }

    private void ResetInputs()
    {
        moveDirection = Vector3.Zero;
        dashKeyPressed = false;
        attackKeyPressed = false;
        grappleKeyPressed = false;
        torchKeyPressed = false;
        interactKeyPressed = false;
        optionsKeyPressed = false;
        godModeKeyPressed = false;
        torchKeyPressed = false;
        interactKeyPressed = false;
        optionsKeyPressed = false;
    }

    private void CollectMovementInput()
    {
        moveDirection = Vector3.Zero;

        if (Input.IsKeyPressed(KeyCode.W) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_UP)) moveDirection.z += 1;
        if (Input.IsKeyPressed(KeyCode.S) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_DOWN)) moveDirection.z -= 1;
        if (Input.IsKeyPressed(KeyCode.A) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_LEFT)) moveDirection.x -= 1;
        if (Input.IsKeyPressed(KeyCode.D) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_RIGHT)) moveDirection.x += 1;

        if (Mathf.Abs(Input.LeftAxis.x) > 0.15 )
            moveDirection.x = Input.LeftAxis.x;
        if (Mathf.Abs(Input.LeftAxis.y) > 0.15)
            moveDirection.z = Input.LeftAxis.y;
    }

    private void CollectDashInput()
    {
        dashKeyPressed = Input.IsKeyDown(KeyCode.SPACE) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A);
    }

    private void CollectAttackInput()
    {
        attackKeyPressed = Input.IsKeyDown(KeyCode.J) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_X);
    }

    private void CollectDebugInputs()
    {
        godModeKeyPressed = Input.IsKeyDown(KeyCode.G);
    }
    private void CollectGrappleInput()
    {
        grappleKeyPressed = Input.IsKeyDown(KeyCode.I) || (Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_LEFT_SHOULDER) || Input.LeftTriggerRaw > 0.15f);
    }

    private void CollectTorchInput()
    {
        torchKeyPressed = Input.IsKeyPressed(KeyCode.O) || (Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_RIGHT_SHOULDER) || Input.RightTriggerRaw > 0.15f);
    }

    private void CollectInteractInput()
    {
        interactKeyPressed = Input.IsKeyPressed(KeyCode.E) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_Y);
    }

    private void CollectOptionsInput()
    {
        optionsKeyPressed = Input.IsKeyPressed(KeyCode.ESCAPE) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_START);
    }

    //// MORE IF NEED IT
};