using System;
using Loopie;

public class PlayerMovement : Component
{
    public float speed = 10.0f;
    public float rotSpeed = 5.0f;
    public bool isMoving = false;

    public float dashSpeed = 40.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;
    public float dashBufferTime = 0.15f;

    private float dashTimer = 0.0f;
    private float dashCooldownTimer = 0.0f;
    private float dashBufferTimer = 0.0f;

    private Vector3 dashDirection = new Vector3(0, 0, 0);
    private bool wasDashKeyPressed = false;
    public bool isDashing = false;
    public AudioSource dashSfxSource;
    
    private BoxCollider playerCollider;

    public bool isGodMode = false;
    private bool wasGodModeKeyPressed = false;
    public float godModeSpeedMultiplier = 2.5f; 
    private float originalSpeed;

    //public PlayerMovement() { }

    public void OnCreate()
    {
        dashSfxSource = entity.GetComponent<AudioSource>();
        playerCollider = entity.GetComponent<BoxCollider>();
        originalSpeed = speed;
    }

    public void OnUpdate()
    {
        isDashing = HandleDash();
        if (!isDashing) HandleNormalMovement();
        //transform.position -= transform.Up * 9.8f * Time.deltaTime;

        HandleGodMode();

    }

    private void HandleGodMode()
    {
        bool godModeKeyPressed = Input.IsKeyPressed(KeyCode.G); 

        if (godModeKeyPressed && !wasGodModeKeyPressed)
        {
            if (playerCollider != null)
            {
                isGodMode = !isGodMode;
                playerCollider.Trigger = isGodMode;

                if (isGodMode)
                {
                    speed = originalSpeed * godModeSpeedMultiplier; 
                }
                else
                {
                    speed = originalSpeed; 
                }

            }
        }

        wasGodModeKeyPressed = godModeKeyPressed;
    }

    private bool HandleDash()
    {
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;
        if (dashBufferTimer > 0) dashBufferTimer -= Time.deltaTime;

        bool dashKeyPressed = (Input.IsKeyPressed(KeyCode.SPACE) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_X));
        if (dashKeyPressed && !wasDashKeyPressed)
        {
            dashBufferTimer = dashBufferTime;
        }
        wasDashKeyPressed = dashKeyPressed;

        if (dashTimer > 0)
        {
            entity.transform.position += dashDirection * dashSpeed * Time.deltaTime;
            dashTimer -= Time.deltaTime;
            return true;
        }

        if (dashBufferTimer > 0 && dashCooldownTimer <= 0)
        {
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            dashBufferTimer = 0;

            dashDirection = entity.transform.Forward;
            dashSfxSource.Play();
        }

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

        float length = (float)Mathf.Sqrt(moveDirection.x * moveDirection.x + moveDirection.z * moveDirection.z);
        isMoving = length > 0.01f;

        if (isMoving)
        {
            if (length > 1f)
            {
                moveDirection.x /= length;
                moveDirection.z /= length;
            }

            float cos = (float)Mathf.Cos(Mathf.PI / 4f);
            float sin = (float)Mathf.Sin(Mathf.PI / 4f);

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

