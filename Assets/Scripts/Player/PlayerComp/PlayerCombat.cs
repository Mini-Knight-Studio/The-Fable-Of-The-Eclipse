using System;
using Loopie;

public class PlayerCombat : PlayerComponent
{
    public bool isAttacking;
    private float attackTimer = 0f;

    [Header("Cooldowns")]
    public float attack1_Cooldown;
    public float hitbox1_Duration;

    public float attack2_Cooldown;
    public float hitbox2_Duration;

    public float attack3_Cooldown;
    public float hitbox3_Duration;

    [Header("References")]

    public Entity swordTriggerEntity;
    private BoxCollider swordTriggerCollider;

    [ShowInInspector]private int comboIndex = 0;
    private bool wantsToCombo = false;

    [Header("Settings")]
    public float comboWindow = 0.8f;
    public float inputBufferWindow = 0.2f;
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
        if (swordTriggerCollider == null || player.Grapple.IsGrappling || player.Torch.IsTorching || player.Movement.IsDashing())
            return;

        if (isAttacking && player.Input.attackKeyPressed)
        {
            if (attackTimer <= inputBufferWindow)
            {
                wantsToCombo = true;
            }
        }

        if (isAttacking)
        {
            if (attackTimer <= GetAttackCooldownTime() && swordTriggerCollider.entity.Active)
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
            if (player.Input.attackKeyPressed)
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

        attackTimer = GetAttackDuration();
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
        return GetAttackHitboxTime() + GetAttackCooldownTime();
    }

    private float GetAttackCooldownTime()
    {

        switch (comboIndex)
        {
            case (1):
                return attack1_Cooldown;
            case (2):
                return attack2_Cooldown;
            case (3):
                return attack3_Cooldown;
            default:
                return 0;
        }
    }

    private float GetAttackHitboxTime()
    {

        switch (comboIndex)
        {
            case (1):
                return hitbox1_Duration;
            case (2):
                return hitbox2_Duration;
            case (3):
                return hitbox3_Duration;
            default:
                return 0;
        }
    }
};