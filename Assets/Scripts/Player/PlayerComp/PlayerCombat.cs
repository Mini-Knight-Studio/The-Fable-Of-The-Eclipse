using System;
using Loopie;

public class PlayerCombat : PlayerComponent
{
    public bool isAttacking;
    private float attackTimer = 0f;

    public float attackCooldown;
    public float hitboxDuration;

    public Entity swordTriggerEntity;
    private BoxCollider swordTriggerCollider;

    public Entity attackSFXEntity;
    private AudioSource attackSfxSource;

    void OnCreate()
    {
        swordTriggerCollider = swordTriggerEntity.GetComponent<BoxCollider>();
        if (swordTriggerCollider != null) swordTriggerCollider.entity.SetActive(false);
        swordTriggerCollider.Trigger = true;
        attackSfxSource = attackSFXEntity.GetComponent<AudioSource>();

        attackTimer = 0.0f;
        isAttacking = false;
    }

    public void ProcessCombat()
    {
        if (swordTriggerCollider == null)
            return;
        if (isAttacking)
        {
            if (attackTimer <= attackCooldown && swordTriggerCollider.entity.Active == true)
            {
                swordTriggerCollider.entity.SetActive(false);
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
                swordTriggerCollider.entity.SetActive(true);

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