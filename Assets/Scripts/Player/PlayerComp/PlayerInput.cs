using System;
using Loopie;

public class PlayerInput : PlayerComponent
{
    public Vector3 moveDirection = Vector3.Zero;
    public bool dashKeyPressed = false;
    public bool attackKeyPressed = false;
    public bool grappleKeyPressed = false;
    public bool torchKeyPressed = false;

    /// <summary>
    /// DEBUG KEYS
    /// </summary>
    public bool godModeKeyPressed = false;

    public void ProcessInputs()
    {
        CollectMovementInput();

        CollectDashInput();

        CollectAttackInput();

        CollectGrappleInput();

        CollectTorch();
        //// MORE IF NEED IT
        /// ...








        ///// DEBUG INPUTS
        CollectDebugInputs();
        ////

    }

    private void CollectMovementInput()
    {
        moveDirection = Vector3.Zero;

        if (Input.IsKeyPressed(KeyCode.W) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_UP)) moveDirection.z += 1;
        if (Input.IsKeyPressed(KeyCode.S) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_DOWN)) moveDirection.z -= 1;
        if (Input.IsKeyPressed(KeyCode.A) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_LEFT)) moveDirection.x -= 1;
        if (Input.IsKeyPressed(KeyCode.D) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_RIGHT)) moveDirection.x += 1;

        if (Input.LeftAxis.x != 0 || Input.LeftAxis.y != 0)
        {
            moveDirection.x = Input.LeftAxis.x;
            moveDirection.z = Input.LeftAxis.y;
        }
    }

    private void CollectDashInput()
    {
        dashKeyPressed = Input.IsKeyDown(KeyCode.SPACE) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_X);
    }

    private void CollectAttackInput()
    {
        attackKeyPressed = Input.IsKeyDown(KeyCode.J) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A);
    }

    private void CollectDebugInputs()
    {
        godModeKeyPressed = Input.IsKeyDown(KeyCode.G);
    }
    private void CollectGrappleInput()
    {
        grappleKeyPressed = Input.IsKeyDown(KeyCode.E) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_B);
    }

    private void CollectTorch()
    {
        torchKeyPressed = Input.IsKeyPressed(KeyCode.O) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_Y);
    }

    //// MORE IF NEED IT
};