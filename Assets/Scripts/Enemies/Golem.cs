using Loopie;
using System;
using System.Collections;
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
    public Vector2 HitOffset;
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
    private bool dead;

    void OnCreate()
    {
        SetEnemy(Reference, AttackCooldown, PreparationTime, ReachDistance, "Golem");
        int EnemyLayer = Collisions.GetLayerBit("Player");
        int PlayerHitLayer = Collisions.GetLayerBit("WorldLimits");
        LayerOverride = EnemyLayer | PlayerHitLayer;
        ReceivedHits = 0;
        dead = false;
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        #region Health
        if (health.IsDead())
        {
            if (!dead)
            {
                dead = true;
                movement.CanMove = false;
                StopAllOwnedCoroutines();
                animator.PlayClip("G_Scale_CTRL|Death", false, 0.0f, false, true);
                feedback.PlaySound("Death");
            }
            if (animator.AnimationEnded())
                entity.Destroy();
        }
        #endregion
        if (!isAttacking)
            attack_cooldown -= Time.deltaTime;

        Hit(Player.Instance.Combat.GetCurrentComboDamage(), PushForceScale, "G_Scale_CTRL|Walk");
        movement.CanMove = (animator.CurrentClip() == "G_Scale_CTRL|GetHitShieldOn" || isAttacking || ReceivedHits > 0) ? false : true;
        if (!isAttacking && !health.IsDead())
        {
            ReceivedHits = 0;
            #region Movement
            if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(GetEntityForwardBase() + ForcedDetectionDistance))
            {
                animator.PlayClip("G_Scale_CTRL|Chase", true, 0.25f);
                transform.LookAt(GetTargetPosition(), transform.Up);
                movement.Move(2.0f, transform.Forward);
                ResetWander();
                #region Attack
                if (CanDoAttack() && !IsBeingHitted())
                {
                    if (GetDistanceToTarget() < GetEntityForwardBase() + ReachDistance)
                    {
                        ReceivedHits = 0;
                        attackCoroutine = StartCoroutine(Attack(AttackDistance, PreparationTime, AttackCooldown, animator.ClipDuration("G_Scale_CTRL|AttackRecovery_GetArmOut"), HitOffset, Damage, "G_Scale_CTRL|ChargeAttack", "G_Scale_CTRL|Attack", "G_Scale_CTRL|AttackRecovery", "G_Scale_CTRL|AttackRecovery_GetArmOut", false));
                    }
                }
          
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
        if (isAttacking)
        {
            if (ReceivedHits >= HitsForRecovery)
            {
                ReceivedHits = 0;
                StopCoroutine(attackCoroutine);
                StartCoroutine(CancelAttack("G_Scale_CTRL|GetHitShieldOff", animator.ClipDuration("G_Scale_CTRL|GetHitShieldOff")));
                return;
            }
            else if (isShielding) return;
        }
        if (Player.Instance.Combat.TemporalFunctionIsAttacking())
        {
            if (hitbox.HasCollided)
            {
                if (!health.canBeDamaged || isShielding)
                    animator.PlayClip("G_Scale_CTRL|GetHitShieldOn", false, 0.0f, true, true);
                else
                {
                    ReceivedHits++;
                    health.Damage(points);
                    feedback.TickParticles("Hurt", Time.deltaTime);
                    feedback.PlaySound("Hit");
                }
            }
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

    private bool IsBeingHitted()
    {
        return animator.CurrentClip() == "G_Scale_CTRL|GetHitShieldOn" || ReceivedHits > 0;
    }
    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }

};