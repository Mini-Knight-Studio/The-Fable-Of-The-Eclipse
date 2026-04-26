using Loopie;
using System;
using System.Reflection.Emit;

public class PlayerMovement : PlayerComponent
{
    private Movement movementHelper;

    [Header("Settings")]
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 5.0f;
    private bool isMoving = false;

    public float dashSpeed = 40.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;
    private float dashBufferTime = 0.15f;

    private float dashTimer = 0.0f;
    public float dashCooldownTimer = 0.0f;
    private float dashBufferTimer = 0.0f;
    private Vector3 dashDirection = new Vector3(0, 0, 0);
    private bool isDashing = false;

    [Header("Gravity")]
    public bool gravityActive;
    public float gravity;

    [Header("Others")]
    // Walk SFX
    private float walkSFXTimer = 0;
    public float walkSFXInterval = 5;

    // Idle SFX
    private float idleSFXTimer = 0;
    public float idleSFXInterval = 5;

    private BoxCollider playerCollider;

    public bool isGodMode = false;
    public float godModeSpeedMultiplier = 2.5f;

    public void OnCreate()
    {
        movementHelper = entity.GetComponent<Movement>();
        if (movementHelper != null)
            movementHelper.CanMove = true;

        playerCollider = entity.GetComponent<BoxCollider>();
    }

    public void ProcessMovement()
    {
        if (!player.Grapple.IsGrappling && !player.Torch.IsTorching && !player.Combat.isAttacking)
            isDashing = HandleDash();

        if (!isDashing)
            HandleNormalMovement();

        if (!isMoving && !isDashing)
        {
            idleSFXTimer += Time.deltaTime;

            if (idleSFXTimer >= idleSFXInterval)
            {
                player.Feedback.PlayIdle();
                idleSFXTimer = 0f;
            }
        }



        HandleGodMode();
        HandleGravity();
    }

    private void HandleGravity()
    {
        if (gravityActive && !isGodMode)
        {

            movementHelper.Speed = -gravity;
            movementHelper.Move(Vector3.Up);
        }
    }

    private void HandleGodMode()
    {
        if (player.Input.godModeKeyPressed)
        {
            if (playerCollider != null)
            {
                isGodMode = !isGodMode;
                playerCollider.Trigger = isGodMode;
            }
        }
    }

    private bool HandleDash()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
        if (dashBufferTimer > 0)
            dashBufferTimer -= Time.deltaTime;

        if (player.Input.dashKeyPressed)
            dashBufferTimer = dashBufferTime;

        if (dashTimer > 0)
        {
            movementHelper.Speed = player.Effects.GetEffectValueFloat(dashSpeed, "ModifySpeed");
            movementHelper.Move(dashDirection);
            dashTimer -= Time.deltaTime;
            return true;
        }

        if (dashBufferTimer > 0 && dashCooldownTimer <= 0)
        {
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            dashBufferTimer = 0;

            dashDirection = entity.transform.Forward;
            player.Feedback.PlayDash();
        }

        return false;
    }
    public void ApplyKnockback(float force, float duration, Vector3 direction)
    {
        StartCoroutine(movementHelper.Push(force, duration, direction));
    }

    private void HandleNormalMovement()
    {
        if (IsDashing() || player.Grapple.IsGrappling || player.Torch.IsTorching || player.Combat.isAttacking)
            return;

        Vector3 moveDirection = player.Input.moveDirection;

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

            movementHelper.Speed = player.Effects.GetEffectValueFloat(moveSpeed, "ModifySpeed");
            movementHelper.Move(rotatedDirection, isGodMode? godModeSpeedMultiplier : 1);

            Vector3 targetLookAt = entity.transform.position + rotatedDirection;
            entity.transform.LookAt(targetLookAt, new Vector3(0, 1, 0));

            walkSFXTimer += Time.deltaTime;

            if (walkSFXTimer >= walkSFXInterval)
            {
                player.Feedback.PlayWalk();
                walkSFXTimer = 0f;
            }
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
    public bool CanDash()
    {
        return dashCooldownTimer <= 0 && dashBufferTimer > 0;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}