using Loopie;
using System;
using static Loopie.Transform;

class Golem : Enemy
{
    public string Reference;

    public int ShieldLife;
    public bool isShielding;

    public float ViewFieldWidth;
    public float ViewFieldFar;

    public float KnockbackForce;
    public float KnockbackTime;

    public int Damage;
    public float AttackReachDistance;
    public float AttackCooldownTime;
    public float AttackPreparationTime;

    public float TargetForcedDetectionDistance;

    private int LayerOverride;

    void OnCreate()
    {
        SetEnemy(Reference, AttackCooldownTime, AttackPreparationTime, AttackReachDistance);
        int EnemyLayer = Collisions.GetLayerBit("Player");
        int PlayerHitLayer = Collisions.GetLayerBit("WorldLimits");
        LayerOverride = EnemyLayer | PlayerHitLayer;
    }

    void OnUpdate()
    {
        //Temporal
        TestKeys();
        //
        if (!isAttacking)
        {
            if (ShieldLife > 0)
                isShielding = true;
            else
                isShielding = false;
            #region Movement
            if (DetectedTargetInViewField(ViewFieldWidth, ViewFieldFar) || DetectedTargetInDistance(TargetForcedDetectionDistance))
            {
                transform.LookAt(target.transform.position, transform.Up);
                movement.Move(isShielding ? 0.5f : 1.0f, transform.Forward);
                ResetWander();
                #region Attack
                if (Vector3.Distance(target.transform.position, transform.position) < AttackReachDistance)
                {
                    StartCoroutine(DoAttack(Damage));
                }
                #endregion
            }
            else
                Wander(ViewFieldWidth, ViewFieldFar, isShielding ? 0.5f : 1.0f);
            #endregion
            #region Health
            health.UpdateHealth();
            if (health.IsDead())
            {
                entity.Destroy();
            }
            #endregion
        }
        else if (EndedPreparingAttack())
        {
            isShielding = false;
        }
    }

    public override void Hit(int points)
    {
        if (isShielding)
            ShieldLife--;
        else
            health.Damage(points);
    }

    private void TestKeys()
    {
        if (Input.IsKeyDown(KeyCode.O) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A))
        {
            Hit(1);
            StartCoroutine(movement.Push(KnockbackForce, KnockbackTime, GetDirectionToTarget() * -1));
        }
    }

    void OnDrawGizmo()
    {
        DebugViewField(ViewFieldWidth, ViewFieldFar);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};