using System;
using Loopie;

public class Enemy : Component
{
    protected Health health;
    protected Entity target;
    protected Health targetHealth;
    protected Entity reference;
    private float attackCooldown;

    protected void SetReference(string EnemyReference)
    {
        reference = Entity.FindEntityByName(EnemyReference);
    }

    protected void SetEnemy(string EnemyReference)
    {
        SetReference(EnemyReference);
        health = entity.GetComponent<Health>();
        health.CanBeDamaged(true);
        health.Init();
    }

    protected void SetTarget(string name)
    {
        target = Entity.FindEntityByName(name);
        targetHealth = target.GetComponent<Health>();
        targetHealth.Init();
    }

    protected bool DetectedTargetInViewField(float ViewFieldWidth, float ViewFieldDepth)
    {
        Vector3 front = transform.Forward;
        Vector3 targetDirection = GetDirectionToTarget();
        if (Mathf.Abs(Vector3.Angle(front, targetDirection)) <= ViewFieldWidth)
        {
            RaycastHit hit;

            if (Collisions.Raycast(transform.position, targetDirection, ViewFieldDepth, out hit))
            {
                if (hit.entity.ID == target.ID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected Vector3 GetDirectionToTarget()
    {
        return target.transform.position - transform.position;
    }

    protected bool HasAttackCooldown()
    {
        return attackCooldown > 0;
    }

    protected void StartAttackCooldown(float cooldown)
    {
        attackCooldown = cooldown;
    }

    protected void UpdateEnemy()
    {
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        else attackCooldown = 0;
    }
}


