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

    private int comboIndex = 0;
    private bool wantsToCombo = false;
    public float comboWindow = 0.8f;
    private float comboResetTimer = 0f;

    void OnCreate()
    {
        swordTriggerCollider = swordTriggerEntity.GetComponent<BoxCollider>();
        if (swordTriggerCollider != null) swordTriggerCollider.entity.SetActive(false);
        swordTriggerCollider.Trigger = true;

        attackTimer = 0.0f;
        isAttacking = false;
    }

    public void ProcessCombat()
    {
        if (swordTriggerCollider == null) return;

        if (isAttacking && (Input.IsKeyDown(KeyCode.J) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A)))
        {
            wantsToCombo = true;
        }

        if (isAttacking)
        {
            if (attackTimer <= attackCooldown && swordTriggerCollider.entity.Active)
            {
                swordTriggerCollider.entity.SetActive(false);
            }

            if (attackTimer > 0.0f)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                isAttacking = false;
                if (wantsToCombo)
                {
                    PerformAttack();
                }
            }
        }
        else
        {
            if (Input.IsKeyDown(KeyCode.J) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A))
            {
                PerformAttack();
            }

            comboResetTimer += Time.deltaTime;
            if (comboResetTimer > comboWindow)
            {
                comboIndex = 0;
            }
        }
    }

    private void PerformAttack()
    {
        isAttacking = true;
        wantsToCombo = false;
        comboResetTimer = 0f;

        comboIndex++;
        if (comboIndex > 3) comboIndex = 1;

        attackTimer = hitboxDuration + attackCooldown;
        player.Feedback.PlayAttack();
        swordTriggerCollider.entity.SetActive(true);
    }

    public int GetCurrentComboIndex()
    {
        return comboIndex;
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