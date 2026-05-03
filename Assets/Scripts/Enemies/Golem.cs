using Loopie;
using System;
using static Loopie.Transform;

class Golem : Enemy
{
    [Header("Global Enemy")]
    public Entity Reference;
    public Vector2 ViewField;
    public float ForcedDetectionDistance;
    [Space(5)]
    public int Damage;
    public float ReachDistance;
    public float AttackDistance;
    public float PushForceScale;
    [Space(5)]
    public float PreparationTime;
    public float AttackCooldown;
    [Space(5)]
    [Header("Golem")]
    public int HitsForRecovery;
    //Private//
    private int LayerOverride;
    private bool isShielding;
    private int ReceivedHits;

    void OnCreate()
    {
        SetEnemy(Reference, AttackCooldown, PreparationTime, ReachDistance, "Golem");
        int EnemyLayer = Collisions.GetLayerBit("Player");
        int PlayerHitLayer = Collisions.GetLayerBit("WorldLimits");
        LayerOverride = EnemyLayer | PlayerHitLayer;
        ReceivedHits = 0;
    }

    void OnUpdate()
    {
        if (Pause.isPaused)
        {
            return;
        }
        #region Health
        if (health.IsDead())
        {
            StopAllCoroutines();
            movement.CanMove = false;
            entity.Destroy();
        }
        #endregion
        Hit(1, PushForceScale, "G_Scale_CTRL|Walk");
        if (!isAttacking && !health.IsDead())
        {
            ReceivedHits = 0;
            #region Movement
            if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(GetEntityForwardBase() + ForcedDetectionDistance))
            {
                animator.PlayClip("G_Scale_CTRL|Chase", true, 0.25f);
                transform.LookAt(Player.Instance.transform.position, transform.Up);
                movement.Move(2.0f, transform.Forward);
                ResetWander();
                #region Attack
                if (Vector3.Distance(Player.Instance.transform.position, transform.position) < GetEntityForwardBase() + ReachDistance)
                    attackCoroutine = StartCoroutine(Attack(AttackDistance, PreparationTime, AttackCooldown, animator.ClipDuration("G_Scale_CTRL|AttackRecovery_ArmOut"), Damage, "G_Scale_CTRL|ChargeAttack", "G_Scale_CTRL|Attack", "G_Scale_CTRL|AttackRecovery_ArmStuck", "G_Scale_CTRL|AttackRecovery_ArmOut"));
                #endregion
            }
            else if (!health.IsDead())
            {
                animator.PlayClip("G_Scale_CTRL|Walk", true, 0.25f);
                Wander(ViewField);
            }
            #endregion
        }

        if (OnHitCooldown() && !health.IsDead())
            isShielding = false;
        else
            isShielding = true;
    }

    public override void Hit(int points, float force_scale, string hit_clip)
    {
        if (!health.canBeDamaged || isShielding) return;
        if (Player.Instance.Combat.TemporalFunctionIsAttacking())
        {
            if (hitbox.HasCollided)
            {
                ReceivedHits++;
                health.Damage(points);
                transform.LookAt(Player.Instance.transform.position, Vector3.Up);
                feedback.TickParticles("Hurt", Time.deltaTime);
                feedback.PlaySound("Hit");
            }
        }

        if (isAttacking && ReceivedHits >= HitsForRecovery)
        {
            StopCoroutine(attackCoroutine);
            StartCoroutine(CancelAttack("G_Scale_CTRL|AttackRecovery_ArmOut", animator.ClipDuration("G_Scale_CTRL|AttackRecovery_ArmOut")));
        }
    }

    void OnDrawGizmo()
    {
        DebugViewField(ViewField.x, ViewField.y);
        #region Target Not Detected
        if (!DetectedTargetInViewField(ViewField.x, ViewField.y) && !DetectedTargetInDistance(GetEntityForwardBase() + ForcedDetectionDistance))
            Gizmo.DrawCircle(transform.position, GetEntityForwardBase() + ForcedDetectionDistance, Vector3.Up, 32, Color.White);
        #endregion
        if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(GetEntityForwardBase() + ForcedDetectionDistance))
        {
            if (ReachDistance < AttackDistance)
            {
                DebugForwardLine(GetEntityForwardBase() + (ReachDistance), Color.Orange);
                DebugForwardLine(GetEntityForwardBase() + (AttackDistance), Color.Red, GetEntityForwardBase() + (ReachDistance));
            }
            else if (ReachDistance > AttackDistance)
            {
                DebugForwardLine(GetEntityForwardBase() + (ReachDistance), Color.Orange, GetEntityForwardBase() + (AttackDistance));
                DebugForwardLine(GetEntityForwardBase() + (AttackDistance), Color.Red);
            }
            else
            {
                DebugForwardLine(GetEntityForwardBase() + (ReachDistance), Color.Red);
            }
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }

};