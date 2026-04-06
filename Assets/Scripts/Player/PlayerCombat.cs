using System;
using Loopie;

public class PlayerCombat : Component
{
    public bool isAttacking;
    private float attackTimer = 0f;

    public float attackCooldown;
    public float hitboxDuration;

    public BoxCollider swordTrigger;
    public AudioSource attackSfxSource;

    void OnCreate()
    {
        swordTrigger = Entity.FindEntityByName("PlayerSwordTrigger").GetComponent<BoxCollider>();
        if (swordTrigger != null) swordTrigger.entity.SetActive(false);
        swordTrigger.Trigger = true;
        attackSfxSource = swordTrigger.entity.GetComponent<AudioSource>();

        attackTimer = 0.0f;
        isAttacking = false;
    }

    void OnUpdate()
    {
        if (swordTrigger == null)
            return;
        if (isAttacking)
        {
            if (attackTimer <= attackCooldown && swordTrigger.entity.Active == true)
            {
                swordTrigger.entity.SetActive(false);
            }

            if (attackTimer > 0.0f)
                attackTimer -= Time.deltaTime;
            else
            {
                attackTimer = 0.0f;
                isAttacking = false;
            }

        }
        else
        {
            if (Input.IsKeyDown(KeyCode.J) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A))
            {
                isAttacking = true;
                attackTimer = attackCooldown + hitboxDuration;
                attackSfxSource.Play();
                swordTrigger.entity.SetActive(true);
            }
        }
    }

    public bool TemporalFunctionIsAttacking()
    {
        return isAttacking;
    }

    public float GetAttackDuration()
    {
        return hitboxDuration + attackCooldown;
    }
};