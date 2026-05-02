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

    //Private//
    private int LayerOverride;
    private bool isShielding;

    void OnCreate()
    {
        SetEnemy(Reference, AttackCooldown, PreparationTime, ReachDistance, "Golem");
        int EnemyLayer = Collisions.GetLayerBit("Player");
        int PlayerHitLayer = Collisions.GetLayerBit("WorldLimits");
        LayerOverride = EnemyLayer | PlayerHitLayer;
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
            movement.CanMove = false;
            entity.Destroy();
        }
        #endregion
        Hit(1, PushForceScale, "G_Scale_CTRL|Walk");
        if (!isAttacking && !health.IsDead())
        {
            isShielding = true;
            
            #region Movement
            if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(GetEntityForwardBase() + ForcedDetectionDistance))
            {
                animator.PlayClip("G_Scale_CTRL|Chase", true, 0.25f);
                transform.LookAt(Player.Instance.transform.position, transform.Up);
                movement.Move(2.0f, transform.Forward);
                ResetWander();
                #region Attack
                if (Vector3.Distance(Player.Instance.transform.position, transform.position) < GetEntityForwardBase() + ReachDistance)
                    StartCoroutine(Attack(AttackDistance, PreparationTime, AttackCooldown, 0, Damage, "G_Scale_CTRL|ChargeAttack", "G_Scale_CTRL|Attack", "G_Scale_CTRL|ArmStuck", "G_Scale_CTRL|ArmOut"));
                #endregion
            }
            else if (!health.IsDead())
            {
                animator.PlayClip("G_Scale_CTRL|Walk", true, 0.25f);
                Wander(ViewField);
            }
            #endregion
        }
        else if (EndedPreparingAttack() && !health.IsDead())
        {
            isShielding = false;
        }
    }

    public override void Hit(int points, float force_scale, string hit_clip)
    {
        if (!isShielding && OnHitCooldown())
            base.Hit(points, force_scale, hit_clip);
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