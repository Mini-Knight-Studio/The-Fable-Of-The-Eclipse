using System;
using Loopie;

public class PlayerMovement : Component
{
    //Movement Settings
    public float speed = 10.0f;
    public float rotSpeed = 5.0f;
    public bool isMoving = false;

    //Dash Settings
    public float dashSpeed = 40.0f;
    public float dashDuration = 0.2f;
    private float dashTimer = 0.0f;
    private Vector3 dashDirection = new Vector3(0, 0, 0);
    private bool tempDashOnce = false;
    private bool wasSpacePressed = false;
    public bool isDashing = false;
    public AudioSource dashSfxSource;

    public PlayerMovement() { }

    public void OnCreate() {
        dashSfxSource = entity.GetComponent<AudioSource>();
    }

    public void OnUpdate()
    {

        isDashing = HandleDash();
        HandleNormalMovement();
    }

    private bool HandleDash()
    {
        if (dashTimer > 0)
        {
            entity.transform.position += dashDirection * dashSpeed * Time.deltaTime;
            dashTimer -= Time.deltaTime;

            return true;
        }

        bool isSpacePressed = Input.IsKeyPressed(KeyCode.SPACE);

        if (isSpacePressed && !wasSpacePressed)
        {
            dashTimer = dashDuration;
            dashDirection = entity.transform.Forward;
            dashSfxSource.Play();
        }

        wasSpacePressed = isSpacePressed;

        return false;
    }

    private void HandleNormalMovement()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.IsKeyPressed(KeyCode.W) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_UP))
        {
            moveDirection.z += 1;
        }
        if (Input.IsKeyPressed(KeyCode.S) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_DOWN))
        {
            moveDirection.z -= 1;
        }
        if (Input.IsKeyPressed(KeyCode.A) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_LEFT))
        {
            moveDirection.x -= 1;
        }
        if (Input.IsKeyPressed(KeyCode.D) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_RIGHT))
        {
            moveDirection.x += 1;
        }

        if (Input.LeftAxis.x != 0 || Input.LeftAxis.y != 0)
        {
            moveDirection.x = Input.LeftAxis.x;
            moveDirection.z = Input.LeftAxis.y;
        }

        float length = (float)Math.Sqrt(moveDirection.x * moveDirection.x + moveDirection.z * moveDirection.z);
        isMoving = length > 0.01f;

        if (isMoving)
        {
            if (length > 1f)
            {
                moveDirection.x /= length;
                moveDirection.z /= length;
            }

            float cos = (float)Math.Cos(Math.PI / 4f);
            float sin = (float)Math.Sin(Math.PI / 4f);

            Vector3 rotatedDirection = new Vector3(
                moveDirection.x * cos + moveDirection.z * sin,
                0f,
                moveDirection.z * cos - moveDirection.x * sin
            );

            entity.transform.position += rotatedDirection * Time.deltaTime * speed;

            Vector3 targetLookAt = entity.transform.position + rotatedDirection;
            entity.transform.LookAt(targetLookAt, new Vector3(0, 1, 0));
        }
    }
}
