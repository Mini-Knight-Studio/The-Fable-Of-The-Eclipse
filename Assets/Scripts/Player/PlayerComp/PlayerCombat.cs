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

    [Header("Attacks Colliders")]
    public Entity attack1ColliderEntity;
    BoxCollider attack1Collider;
    public float attack1Damage;

    public Entity attack2ColliderEntity;
    BoxCollider attack2Collider;
    public float attack2Damage;

    public Entity attack3ColliderEntity;
    BoxCollider attack3Collider;
    public float attack3Damage;

    [ShowInInspector]private int comboIndex = 0;
    private bool wantsToCombo = false;

    [Header("Settings")]
    public float comboWindow = 0.8f;
    public float inputBufferWindow = 0.2f;
    private float comboResetTimer = 0f;

    void OnCreate()
    {
        attack1Collider = attack1ColliderEntity.GetComponent<BoxCollider>();
        if(attack1Collider != null) attack1Collider.entity.SetActive(false);
        attack1Collider.Trigger = true;

        attack2Collider = attack2ColliderEntity.GetComponent<BoxCollider>();
        if (attack2Collider != null) attack2Collider.entity.SetActive(false);
        attack2Collider.Trigger = true;

        attack3Collider = attack3ColliderEntity.GetComponent<BoxCollider>();
        if (attack3Collider != null) attack3Collider.entity.SetActive(false);
        attack3Collider.Trigger = true;

        attackTimer = 0.0f;
        isAttacking = false;
    }

    public void ProcessCombat()
    {
        if (player.Grapple.IsGrappling || player.Torch.IsTorching || player.Movement.IsDashing())
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
            if (attackTimer <= GetAttackCooldownTime())
            {
                attack1Collider.entity.SetActive(false);
                attack2Collider.entity.SetActive(false);
                attack3Collider.entity.SetActive(false);
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

        if (comboIndex == 1) attack1Collider.entity.SetActive(true);
        else if (comboIndex == 2) attack2Collider.entity.SetActive(true);
        else if (comboIndex == 3) attack3Collider.entity.SetActive(true);
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

    public int GetCurrentComboDamage() //For some reason, casting here seems to be the only way it works, casting on Hit() doesn't
    {
        switch (comboIndex)
        {
            case (1):
                return (int)attack1Damage;
            case (2):
                return (int)attack2Damage;
            case (3):
                return (int)attack3Damage;
            default: 
                return 1;
        }
    }
};