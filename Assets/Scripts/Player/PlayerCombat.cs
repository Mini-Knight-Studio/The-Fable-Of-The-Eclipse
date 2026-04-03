using System;
using Loopie;

public class PlayerCombat : Component
{
    public bool isAttacking = false;
    private float attackTimer = 0f;

    public float attackCooldown = 0.5f;
    public float hitboxDuration = 0.2f;

    public Entity swordTrigger;
    public AudioSource attackSfxSource;

    void OnCreate()
    {
        swordTrigger = Entity.FindEntityByName("PlayerSwordTrigger");
        if (swordTrigger != null) swordTrigger.SetActive(false);
        attackSfxSource = swordTrigger.GetComponent<AudioSource>();
    }

    void OnUpdate()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer < (attackCooldown - hitboxDuration))
            {
                if (swordTrigger != null) swordTrigger.SetActive(false);
            }

            if (attackTimer <= 0)
            {
                isAttacking = false;
            }
            return;
        }

        if (Input.IsKeyPressed(KeyCode.J) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_A))
        {
            if (swordTrigger != null) swordTrigger.SetActive(true);

            isAttacking = true;
            attackTimer = attackCooldown;
            attackSfxSource.Play();
        }
    }
};