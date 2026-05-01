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
    public float PushForceScale;
    [Space(5)]
    public float PreparationTime;
    public float AttackCooldown;
    [Space(10)]
    [Header("Golem")]
    public int ShieldLife;

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
        Hit(1, PushForceScale, "Armature|IdleWalk");
        if (!isAttacking && !health.IsDead())
        {
            isShielding = ShieldLife > 0;
            
            #region Movement
            if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(transform.scale.x + ForcedDetectionDistance))
            {
                animator.PlayClip("Armature|Chase", true, 0.25f);
                transform.LookAt(Player.Instance.transform.position, transform.Up);
                movement.Move(isShielding ? 0.5f : 1.0f, transform.Forward);
                ResetWander();
                #region Attack
                if (Vector3.Distance(Player.Instance.transform.position, transform.position) < (transform.scale.x + ReachDistance*0.25f))
                {
                    StartCoroutine(Attack(ReachDistance, PreparationTime, AttackCooldown, 0, Damage, "Armature|ChargeAttack", "Armature|Attack", "Armature|IdleWalk", "Armature|IdleWalk"));
                }
                #endregion
            }
            else
                Wander(ViewField, isShielding ? 0.5f : 1.0f);
            #endregion
        }
        else if (EndedPreparingAttack() && !health.IsDead())
        {
            isShielding = false;
        }
    }

    public override void Hit(int points, float force_scale, string hit_clip)
    {
        if (isShielding && !OnHitCooldown())
        {
            ShieldLife--;
            animator.PlayClip("Armature|IdleWalk", false, 0.0f, true);
        }
        else
            base.Hit(points, force_scale, hit_clip);
    }

    void OnDrawGizmo()
    {
        DebugViewField(ViewField.x, ViewField.y);
        DebugToTargetLine(ForcedDetectionDistance, Color.Red);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }

};