using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

public class Enemy : Component
{
    protected Entity reference;
    protected Health health;
    protected Movement movement;
    protected EnemyAnimation animator;
    protected EnemyFeedback feedback;
    protected BoxCollider collision;
    protected BoxCollider hitbox;
    protected TemporalEffect effect;

    protected ParticleComponent enemyParticles;
    protected ParticleComponent attackParticles;
    protected ParticleComponent hitParticles;

    protected Player target;


    //-- Wander --//
    private bool wanderRange = false;
    private Vector3 lastWanderPosition;

    //-- Attack --//
    protected bool isAttacking;
    private Vector2 attack_stages; //x: ended preparing attack | y: ended attacking
    private float knockback_time;

    private float attack_cooldown;
    private float preparation_time;
    private float reach_distance;

    public string type = "Enemy";
    #region Set Up
    protected void SetEnemy(Entity reference_enemy, float attack_cooldown, float preparation_time, float reach_distance, string enemyType)
    {
        reference = reference_enemy;
        type = enemyType;
        target = Player.Instance;
        knockback_time = 0.25f;

        health = entity.GetComponent<Health>();
        movement = entity.GetComponent<Movement>();
        collision = entity.GetComponent<BoxCollider>();
        animator = entity.GetComponent<EnemyAnimation>();
        feedback = entity.GetComponent<EnemyFeedback>();
        effect = entity.GetComponent<TemporalEffect>();

        attackParticles = entity.GetComponent<ParticleComponent>();
        hitParticles = entity.GetComponent<ParticleComponent>();

        foreach(Entity child in entity.GetChildren())
        {
            if (child.Name == "Hitbox")
            {
                hitbox = child.GetComponent<BoxCollider>();
                hitParticles = child.GetComponent<ParticleComponent>();
            }
        }
        health.Init();
        wanderRange = false;
        //ResetWander();


        attackParticles.Enabled = false;
        hitParticles.Enabled = false;
    }
    #endregion
    #region Detection
    protected bool DetectedTargetInViewField(float field_width, float field_depth)
    {
        Vector3 front = transform.Forward;
        Vector3 targetDirection = GetDirectionToTarget();
        if (Mathf.Abs(Vector3.Angle(front, targetDirection)) <= field_width)
        {
            return DetectedTargetInDistance(field_depth);
        }
        return false;
    }

    protected bool DetectedTargetInDistance(float distance)
    {
        RaycastHit hit;
        int PlayerLayer = Collisions.GetLayerBit("Player");
        int WallLayer = Collisions.GetLayerBit("WorldLimits");
        int LayerMask = PlayerLayer | WallLayer;

        if (Collisions.Raycast(transform.position + transform.Up, GetDirectionToTarget(), distance, out hit, LayerMask))
        {
            if (hit.entity == target.entity)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Target
    protected Vector3 GetDirectionToTarget()
    {
        return (target.transform.position - transform.position).normalized;
    }
    #endregion

    #region Attack
    protected IEnumerator Attack(float reach_distance, float preparation_time, float attack_cooldown, int damage)
    {
        isAttacking = true;
        DoChargeAttack();
        yield return new WaitForSeconds(preparation_time);
        DoAttack(reach_distance, damage);
        yield return new WaitForSeconds(animator.ClipDuration());
        DoAttackCooldown();
        yield return new WaitForSeconds(attack_cooldown);
        EndAttack();
        isAttacking = false;
    }

    public virtual void DoChargeAttack()
    {
        attack_stages = Vector2.Zero;
        movement.CanMove = false;
        transform.LookAt(target.transform.position, Vector3.Up);
        animator.PlayClip("Armature|ChargeAttack", false, 0.0f);
    }

    public virtual void DoAttack(float reach_distance, int damage)
    {
        attack_stages.x = 1.0f;
        animator.PlayClip("Armature|Attack", false, 0.0f);
        if (Mathf.Abs((float)Vector3.Distance(transform.position, target.transform.position)) <= reach_distance)
        {
            target.Effects.AddEffect(effect);
            target.PlayerHealth.Damage(target.Effects.GetEffectValueInt(damage, "ModifyDamage"));
            target.Movement.ApplyKnockback((float)damage * 10.0f, 0.3f, GetDirectionToTarget());
        }
    }

    public virtual void DoAttackCooldown()
    {
        attack_stages.y = 1.0f;
        animator.PlayClip("Armature|IdleWalk", false, 0.0f);
    }

    public virtual void EndAttack()
    {
        attack_stages = Vector2.Zero;
        movement.CanMove = true;
    }

    public virtual void Hit(int points, float force_scale)
    {
        if (OnHitCooldown() || !health.canBeDamaged) return;
        if (target.Combat.TemporalFunctionIsAttacking())
        {
            if (hitbox.HasCollided)
            {
                health.Damage(points);
                StartCoroutine(movement.Push(points * force_scale, knockback_time, GetDirectionToTarget() * -1));
            }
        }
    }
    #endregion

    #region Wander
    protected void ResetWander()
    {
        lastWanderPosition = transform.position + Vector3.Forward;
    }

    protected void Wander(float areaWidth, float reachDistance, float speedMultiplier)
    {
        RaycastHit hit;
        int WallLayer = Collisions.GetLayerBit("WorldLimits");
        int Wall2Layer = Collisions.GetLayerBit("EnemyLimit");
        int EnemyLayer = Collisions.GetLayerBit("Enemy");
        int LayerMask = WallLayer | Wall2Layer | EnemyLayer;

        if (!Collisions.Raycast(transform.position + transform.Up, transform.Forward, reachDistance, out hit, LayerMask, collision))
        {
            movement.Move(speedMultiplier / 2, transform.Forward);
            if (Vector3.Distance(lastWanderPosition, transform.position) > reachDistance)
                return;
        }

            wanderRange = true;
        for (int i = 0; i < 2; i++)
        {
            int tries = 0;
            while (tries < 10)
            {
                float newDir = Loopie.Random.Range(!wanderRange ? -180.0f : -areaWidth, !wanderRange ? 180.0f : areaWidth);

                Vector3 newDirection = Vector3.RotateAroundAxis(transform.Forward, transform.Up, newDir);
                if (!Collisions.Raycast(transform.position + transform.Up, newDirection, reachDistance, out hit, LayerMask))
                {
                    transform.LookAt(transform.position + newDirection, transform.Up);
                    lastWanderPosition = transform.position + transform.Forward * reachDistance * 1.5f;
                    return;
                }
                tries++;
            }
            wanderRange = false;
        }
    }
    #endregion

    #region Debug
    protected void DebugViewField(float field_width, float field_depth)
    {
        Vector3 left = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, -field_width);
        Vector3 right = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, field_width);
        Gizmo.DrawLine(transform.position + transform.Up, transform.position + right * field_depth + transform.Up, Color.White);
        Gizmo.DrawLine(transform.position + transform.Up, transform.position + left * field_depth + transform.Up, Color.White);
        Gizmo.DrawLine(transform.position + transform.Forward * field_depth + transform.Up, transform.position + right * field_depth + transform.Up, Color.White);
        Gizmo.DrawLine(transform.position + transform.Forward * field_depth + transform.Up, transform.position + left * field_depth + transform.Up, Color.White);
    }

    protected void DebugForcedDetection(float detection_distance)
    {
        Gizmo.DrawLine(transform.position + transform.Up, transform.position + transform.Up + GetDirectionToTarget()*detection_distance, Color.Red);
    }
    #endregion

    #region Variables & Control
    public bool Bool(float value)
    {
        return value == 1.0f;
    }

    protected bool EndedPreparingAttack()
    {
        return Bool(attack_stages.x);
    }

    protected bool EndedAttack()
    {
        return Bool(attack_stages.y);
    }

    protected bool OnHitCooldown()
    {
        return (attack_stages == Vector2.One && isAttacking);
    }
    #endregion
}