using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

public class Enemy : Component
{
    protected Entity reference;
    protected Health health;
    protected Movement movement;
    protected Animator animator;
    protected BoxCollider attackBox;
    protected BoxCollider collision;
    protected BoxCollider hitbox;
    protected TemporalEffect effect;

    protected ParticleComponent enemyParticles;
    protected ParticleComponent attackParticles;
    protected ParticleComponent hitParticles;

    protected Player target;

    private float attackCooldown;
    private float attackPreparationTime;
    private float attackReachDistance;

    private bool wanderRange = false;
    protected bool isAttacking;
    private Vector3 lastWanderPosition;
    private bool endedPreparingAttack;
    private bool endedAttack;
    private float internal_hit_cooldown;

    protected void UpdateEnemy()
    {
        if(internal_hit_cooldown > 0.0f)
            internal_hit_cooldown -= Time.deltaTime;
        else
            internal_hit_cooldown = 0.0f;
        //Temporal
        if(target.Combat.TemporalFunctionIsAttacking())
        {
            if (hitbox.IsColliding)
            {
                Hit(1);
            }
        }
        //
    }
    #region Set Up
    protected void SetEnemy(string reference_name, float attack_cooldown, float attack_preparation_time, float attack_reach_distance)
    {
        reference = Entity.FindEntityByName(reference_name);

        health = entity.GetComponent<Health>();
        movement = entity.GetComponent<Movement>();
        collision = entity.GetComponent<BoxCollider>();
        animator = entity.GetComponent<Animator>();
        effect = entity.GetComponent<TemporalEffect>();

        attackParticles = entity.GetComponent<ParticleComponent>();
        hitParticles = entity.GetComponent<ParticleComponent>();

        foreach(Entity child in entity.GetChildren())
        {
            if (child.Name == "AttackBox")
            {
                attackBox = child.GetComponent<BoxCollider>();
                attackParticles = child.GetComponent<ParticleComponent>();
            }
            if (child.Name == "Hitbox")
            {
                hitbox = child.GetComponent<BoxCollider>();
                hitParticles = child.GetComponent<ParticleComponent>();
            }
        }
        health.Init();
        wanderRange = false;
        ResetWander();

        attackCooldown = attack_cooldown;
        attackPreparationTime = attack_preparation_time;
        attackReachDistance = attack_reach_distance;
        internal_hit_cooldown = 0.0f;
        target = Entity.FindEntityByName("Player").GetComponent<Player>();

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
    protected IEnumerator DoAttack(int damage)
    {
        float timer = 0.0f;
        movement.CanMove = false;
        isAttacking = true;
        endedPreparingAttack = false;
        endedAttack = false;
        PlayAnimation("Armature|ChargeAttack", 0.0f);
        while (timer < attackPreparationTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        PlayAnimation("Armature|Attack", 0.0f);
        animator.Looping = false;
        endedPreparingAttack = true;
        attackBox.entity.SetActive(true);
        if (attackBox.IsColliding || Vector3.Distance(transform.position, target.transform.position) < attackReachDistance)
        {
            target.Effects.AddEffect(effect);
            Attack(damage);
        }
        attackParticles.Enabled = true;
        yield return new WaitForSeconds(Time.deltaTime);
        attackParticles.Enabled = false;
        timer = 0.0f;
        float animation_duration = animator.GetCurrentClipDuration();
        while (timer < animation_duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        PlayAnimation("Armature|IdleWalk", 0.0f);
        animator.Looping = true;
        timer = 0.0f;
        attackBox.entity.SetActive(false);
        endedAttack = true;
        while (timer < attackCooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        attackBox.entity.SetActive(true);
        movement.CanMove = true;
        isAttacking = false;
        endedPreparingAttack = false;
        endedAttack = false;
    }

    private void Attack(int points)
    {
        target.PlayerHealth.Damage(target.Effects.GetEffectValueInt(points, "ModifyDamage"));
        StartCoroutine(target.Movement2.Push((float)points * 10.0f, 0.3f, GetDirectionToTarget()));
    }

    protected bool EndedPreparingAttack()
    {
        return endedPreparingAttack;
    }

    protected bool EndedAttack()
    {
        return endedPreparingAttack;
    }

    protected bool OnHitCooldown()
    {
        return internal_hit_cooldown > 0.0f;
    }

    protected void StartHitCooldown(float attack_duration)
    {
        internal_hit_cooldown = attack_duration;
    }

    public virtual void Hit(int points)
    {
        if (OnHitCooldown() || !health.canBeDamaged) return;
        StartHitCooldown(target.Combat.GetAttackDuration());
        StartCoroutine(ParticleTick(hitParticles, Vector3.Up, Time.deltaTime));
        health.Damage(points);
        StartCoroutine(movement.Push(points * 25 - health.maxHealth, 0.3f, GetDirectionToTarget() * -1));
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
        int LayerMask = WallLayer | Wall2Layer /*| EnemyLayer*/;

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
        Vector3 leftZone = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, -field_width);
        Vector3 rightZone = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, field_width);
        Gizmo.DrawLine(transform.position + transform.Forward * field_depth + transform.Up, transform.position - leftZone * -1.0f * field_depth + transform.Up, Color.White);
        Gizmo.DrawLine(transform.position + transform.Forward * field_depth + transform.Up, transform.position - rightZone * -1.0f * field_depth + transform.Up, Color.White);
        Gizmo.DrawLine(transform.position + transform.Up, transform.position + rightZone * field_depth + transform.Up, Color.White);
        Gizmo.DrawLine(transform.position + transform.Up, transform.position + leftZone * field_depth + transform.Up, Color.White);
    }
    #endregion
    #region Visual
    public void PlayAnimation(string clipName, float transitionTime)
    {
        if (!animator.InTransition)
        {
            if (animator.GetCurrentClipName() == clipName)
                return;
        }
        else
        {
            if (animator.GetNextClipName() == clipName)
                return;
        }
        Debug.Log(clipName);
        animator.Play(clipName, transitionTime);
    }

    public IEnumerator ParticleTick(ParticleComponent particles, Vector3 sequence, float tick_duration/*, string emitter, bool force_system_active, bool only_active*/) //Sequence options: -2 no modify | -1 opposite | 0 false | 1 true 
    {
        particles.SetActive(TranslateBool(particles, sequence.x));
        yield return new WaitForSeconds(tick_duration);
        particles.SetActive(TranslateBool(particles, sequence.y));
        yield return new WaitForSeconds(tick_duration);
        particles.SetActive(TranslateBool(particles, sequence.z));
    }

    private bool TranslateBool(ParticleComponent particles, float number)
    {
        if( particles == null ) return false;
        if (number == -1) return !particles.Enabled;
        if (number == 0) return false;
        if (number == 1) return true;
        return particles.Enabled;
    }
    #endregion
}