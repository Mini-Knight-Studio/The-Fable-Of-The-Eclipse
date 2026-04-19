using Loopie;
using System;
using static Loopie.Transform;

class Golem : Enemy
{
    public Entity Reference;

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
        SetEnemy(Reference, AttackCooldownTime, AttackPreparationTime, AttackReachDistance, "Golem");
        int EnemyLayer = Collisions.GetLayerBit("Player");
        int PlayerHitLayer = Collisions.GetLayerBit("WorldLimits");
        LayerOverride = EnemyLayer | PlayerHitLayer;
    }

    void OnUpdate()
    {
        //Temporal
        TestKeys();
        //

        UpdateEnemy();

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

        }
        else if (EndedPreparingAttack())
        {
            isShielding = false;
        }
        #region Health
        if (health.IsDead())
        {
            entity.Destroy();
        }
        #endregion
    }

    public override void Hit(int points)
    {
        if (isShielding && !OnHitCooldown())
        {
            ShieldLife--;
            StartHitCooldown(target.Combat.GetAttackDuration());
        }
        else
            base.Hit(points);
    }

    private void TestKeys()
    {
        if (Input.IsKeyDown(KeyCode.P))
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