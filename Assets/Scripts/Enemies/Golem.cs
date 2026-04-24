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
        Hit(1, PushForceScale);
        if (!isAttacking)
        {
            if (ShieldLife > 0)
                isShielding = true;
            else
                isShielding = false;
            #region Movement
            if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(ForcedDetectionDistance))
            {
                transform.LookAt(target.transform.position, transform.Up);
                movement.Move(isShielding ? 0.5f : 1.0f, transform.Forward);
                ResetWander();
                #region Attack
                if (Vector3.Distance(target.transform.position, transform.position) < ReachDistance)
                {
                    StartCoroutine(Attack(ReachDistance, PreparationTime, AttackCooldown, Damage));
                }
                #endregion
            }
            else
                Wander(ViewField.x, ViewField.y, isShielding ? 0.5f : 1.0f);
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

    public override void Hit(int points, float force_scale)
    {
        if (isShielding && !OnHitCooldown())
        {
            ShieldLife--;
        }
        else
            base.Hit(points, force_scale);
    }

    void OnDrawGizmo()
    {
        DebugViewField(ViewField.x, ViewField.y);
        DebugForcedDetection(ForcedDetectionDistance);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};