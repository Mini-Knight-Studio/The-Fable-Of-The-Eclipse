using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

public class Enemy : Component
{
    protected Entity reference;
    protected Health health;
    protected Movement movement;
    protected BoxCollider attackBox;
    protected BoxCollider collision;
    protected List<Effect> effectList;

    protected Entity target;
    protected Health targetHealth;
    protected Movement targetMovement;

    private float attackCooldown;
    private float attackPreparationTime;
    private float attackReachDistance;

    private bool wanderRange = false;
    protected bool isAttacking;
    private Vector3 lastWanderPosition;
    private bool endedPreparingAttack;
    private bool endedAttack;
    #region Set Up
    protected void SetEnemy(string reference_name, float attack_cooldown, float attack_preparation_time, float attack_reach_distance)
    {
        reference = Entity.FindEntityByName(reference_name);

        health = entity.GetComponent<Health>();
        movement = entity.GetComponent<Movement>();
        collision = entity.GetComponent<BoxCollider>();
        attackBox = entity.GetChild(0).GetComponent<BoxCollider>();
        effectList = new List<Effect>();
        
        SetTarget();
        health.Init();
        wanderRange = false;
        ResetWander();

        attackCooldown = attack_cooldown;
        attackPreparationTime = attack_preparation_time;
        attackReachDistance = attack_reach_distance;
    }

    protected void SetTarget()
    {
        target = Entity.FindEntityByName("Player");
        targetHealth = target.GetComponent<Health>();
        targetMovement = target.GetComponent<Movement>();
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
            if (hit.entity == target)
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
        while (timer < attackPreparationTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        endedPreparingAttack = true;
        //attackBox.entity.SetActive(true);
        if (attackBox.IsColliding || Vector3.Distance(transform.position, target.transform.position) < attackReachDistance)
            Attack(damage);
        timer = 0.0f;
        //attackBox.entity.SetActive(false);
        endedAttack = true;
        while (timer < attackCooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        //attackBox.entity.SetActive(true);
        movement.CanMove = true;
        isAttacking = false;
        endedPreparingAttack = false;
        endedAttack = false;
    }

    private void Attack(int points)
    {
        targetHealth.Damage(points);
        StartCoroutine(targetMovement.Push((float)points*10.0f, 0.3f, GetDirectionToTarget()));
    }

    protected bool EndedPreparingAttack()
    {
        return endedPreparingAttack;
    }

    protected bool EndedAttack()
    {
        return endedPreparingAttack;
    }

    public virtual void Hit(int points)
    {
        health.Damage(points);
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
        int LayerMask = WallLayer;

        if (!Collisions.Raycast(transform.position + transform.Up, transform.Forward, reachDistance, out hit, LayerMask))
        {
            movement.Move(speedMultiplier / 2, transform.Forward);
            if(Vector3.Distance(lastWanderPosition, transform.position) > reachDistance)
                return;
        }
        
        wanderRange = true;
        for (int i = 0; i < 2; i++)
        {
            int tries = 0;
            while (tries < 10)
            {
                float newDir = Loopie.Random.Range(!wanderRange? -180.0f : -areaWidth, !wanderRange? 180.0f : areaWidth);
                
                Vector3 newDirection = Vector3.RotateAroundAxis(transform.Forward,transform.Up, newDir);
                if (!Collisions.Raycast(transform.position + transform.Up, newDirection, reachDistance, out hit, LayerMask))
                {
                    transform.LookAt(transform.position + newDirection, transform.Up);
                    lastWanderPosition = transform.position + transform.Forward * reachDistance*1.5f;
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
        Gizmo.DrawLine(transform.position + transform.Forward * field_depth, transform.position - leftZone * -1.0f * field_depth, Color.White);
        Gizmo.DrawLine(transform.position + transform.Forward * field_depth, transform.position - rightZone * -1.0f * field_depth, Color.White);
        Gizmo.DrawLine(transform.position, transform.position + rightZone * field_depth, Color.White);
        Gizmo.DrawLine(transform.position, transform.position + leftZone * field_depth, Color.White);
    }
    #endregion
}


