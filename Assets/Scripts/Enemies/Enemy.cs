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

    //-- Wander --//
    private bool wanderRange = false;
    private Vector3 lastWanderPosition;
    private Vector3 interest_position;
    private bool interest_position_checked;

    //-- Attack --//
    protected bool isAttacking;
    private Vector2 attack_stages; //x: ended preparing attack | y: ended attacking
    private float knockback_time;

    public string type = "Enemy";
    #region Set Up
    protected void SetEnemy(Entity reference_enemy, float attack_cooldown, float preparation_time, float reach_distance, string enemyType)
    {
        reference = reference_enemy;
        type = enemyType;
        knockback_time = 0.25f;

        health = entity.GetComponent<Health>();
        movement = entity.GetComponent<Movement>();
        collision = entity.GetComponent<BoxCollider>();
        animator = entity.GetComponent<EnemyAnimation>();
        feedback = entity.GetComponent<EnemyFeedback>();
        effect = entity.GetComponent<TemporalEffect>();

        Debug.Log("1");

        foreach(Entity child in entity.GetChildren())
        {
            if (child.Name == "Hitbox")
            {
                hitbox = child.GetComponent<BoxCollider>();
            }
        }

        Debug.Log("2");

        health.Init();
        wanderRange = false;
        ResetWander();
        Debug.Log("3");
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
        int EnemyWallLayer = Collisions.GetLayerBit("EnemyLimit");
        int EnemyLayer = Collisions.GetLayerBit("Enemy");
        int LayerMask = PlayerLayer | WallLayer | EnemyWallLayer | EnemyLayer;

        if (Collisions.Raycast(transform.position + transform.Up, GetDirectionToTarget(), distance, out hit, LayerMask, collision))
        {
            if (hit.entity == Player.Instance.entity)
            {
                return true;
            }
        }
        return false;
    }

    protected bool TargetDetected()
    {
        return true;
    }
    #endregion

    #region Target
    protected Vector3 GetDirectionToTarget()
    {
        return (Player.Instance.transform.position - transform.position).normalized;
    }
    #endregion

    #region Attack
    protected IEnumerator Attack(float reach_distance, float preparation_time, float attack_cooldown, int damage, string charge_attack_clip, string attack_clip, string cooldown_clip, string idle_clip)
    {
        isAttacking = true;
        float timer = 0.0f;
        DoChargeAttack(charge_attack_clip);
        while (timer < preparation_time)
        {
            timer += Time.deltaTime;
            transform.LookAt(Player.Instance.transform.position, Vector3.Up);
            yield return null;
        }
        DoAttack(reach_distance, damage, attack_clip);
        yield return new WaitForSeconds(animator.ClipDuration());
        DoAttackCooldown(cooldown_clip);
        yield return new WaitForSeconds(attack_cooldown);
        EndAttack(idle_clip);
        isAttacking = false;
    }

    public virtual void DoChargeAttack(string charge_attack_clip)
    {
        attack_stages = Vector2.Zero;
        movement.CanMove = false;
        animator.PlayClip(charge_attack_clip, false, 0.0f);
    }

    public virtual void DoAttack(float reach_distance, int damage, string attack_clip)
    {
        attack_stages.x = 1.0f;
        animator.PlayClip(attack_clip, false, 0.0f);
        if (Mathf.Abs((float)Vector3.Distance(transform.position, Player.Instance.transform.position)) <= reach_distance)
        {
            if (Player.Instance.Effects.AddEffect(effect))feedback.TickParticles("Effect", Time.deltaTime);
            else feedback.TickParticles("Attack", Time.deltaTime);
            feedback.PlaySound("Attack");
            feedback.ShakeCamera(damage / 20.0f, knockback_time/2);

            Player.Instance.PlayerHealth.Damage(Player.Instance.Effects.GetEffectValueInt(damage, "ModifyDamage"));
            Player.Instance.Movement.ApplyKnockback((float)damage * 10.0f, 0.3f, GetDirectionToTarget());
        }
    }

    public virtual void DoAttackCooldown(string cooldown_clip)
    {
        attack_stages.y = 1.0f;
        animator.PlayClip(cooldown_clip, false, 0.0f);
    }

    public virtual void EndAttack(string idle_clip)
    {
        animator.PlayClip(idle_clip, false, 0.0f);
        attack_stages = Vector2.Zero;
        movement.CanMove = true;
    }

    public virtual void Hit(int points, float force_scale, string hit_clip)
    {
        if (OnHitCooldown() || !health.canBeDamaged) return;
        if (Player.Instance.Combat.TemporalFunctionIsAttacking())
        {
            if (hitbox.HasCollided)
            {
                health.Damage(points);
                transform.LookAt(Player.Instance.transform.position, Vector3.Up);
                feedback.TickParticles("Hurt", Time.deltaTime);
                feedback.PlaySound("Hit");
                animator.PlayClip(hit_clip, false, 0.0f, true);
                StartCoroutine(movement.Push(points * force_scale, knockback_time, GetDirectionToTarget() * -1));
            }
        }
    }
    #endregion

    #region Wander

    protected Vector3 ApplySideCorrection(Vector3 point)
    {
        return point;
    }

    protected void Chase()
    {

    }

    protected void ResetWander()
    {
        lastWanderPosition = transform.position + Vector3.Forward;
    }

    protected void Wander(Vector2 ViewField, float speedMultiplier)
    {
        RaycastHit hit;
        int WallLayer = Collisions.GetLayerBit("WorldLimits");
        int EnemyWallLayer = Collisions.GetLayerBit("EnemyLimit");
        int EnemyLayer = Collisions.GetLayerBit("Enemy");
        int LayerMask = WallLayer | EnemyWallLayer;

        if (!Collisions.Raycast(transform.position + transform.Up, transform.Forward, ViewField.y, out hit, LayerMask, collision))
        {
            movement.Move(speedMultiplier, transform.Forward);
            if (Vector3.Distance(lastWanderPosition, transform.position) > ViewField.y)
                return;
        }

            wanderRange = true;
        for (int i = 0; i < 2; i++)
        {
            int tries = 0;
            while (tries < 10)
            {
                float newDir = Loopie.Random.Range(!wanderRange ? -180.0f : -ViewField.x, !wanderRange ? 180.0f : ViewField.x);

                Vector3 newDirection = Vector3.RotateAroundAxis(transform.Forward, transform.Up, newDir);
                if (!Collisions.Raycast(transform.position + transform.Up, newDirection, ViewField.y, out hit, LayerMask))
                {
                    transform.LookAt(transform.position + newDirection, transform.Up);
                    lastWanderPosition = transform.position + transform.Forward * ViewField.y * 1.5f;
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

    protected void DebugToTargetLine(float magnitude, Color color)
    {
        Gizmo.DrawLine(transform.position + transform.Up, transform.position + transform.Up + GetDirectionToTarget() * magnitude, color);
    }

    protected void DebugForwardLine(float magnitude, Color color)
    {
        Gizmo.DrawLine(transform.position + transform.Up, transform.position + transform.Up + transform.Forward * magnitude, color);
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