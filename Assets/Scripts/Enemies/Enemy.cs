using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Loopie;

public class Enemy : Component
{
    protected Entity reference;
    protected Entity target;
    protected Health health;
    protected Health targetHealth;
    protected BoxCollider attackBox;
    protected BoxCollider collider;
    private float attackCooldown;
    private bool entireWanderingRange = false;
    private Vector3 lastWanderPosition;

    protected void SetReference(string EnemyReference)
    {
        reference = Entity.FindEntityByName(EnemyReference);
    }

    protected void SetEnemy(string EnemyReference)
    {
        SetReference(EnemyReference);
        health = entity.GetComponent<Health>();
        collider = entity.GetComponent<BoxCollider>();
        attackBox = entity.GetChild(0).GetComponent<BoxCollider>();
        health.Init();
        entireWanderingRange = false;
        ResetWanderBehaviour();
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
            int PlayerLayer = Collisions.GetLayerBit("Player");
            int WallLayer = Collisions.GetLayerBit("WorldLimits");
            int LayerMask = PlayerLayer | WallLayer;

            if (Collisions.Raycast(transform.position + transform.Up, targetDirection, ViewFieldDepth, out hit,LayerMask))
            {
                if (hit.entity == target)
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected Vector3 GetDirectionToTarget()
    {
        return (target.transform.position - transform.position).normalized;
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

    protected IEnumerator ApplyKnockback(float force, Vector3 direction, float duration)
    {
        float timer = 0;

        while (timer<duration)
        {
            float factor = duration - timer;
            factor = Mathf.Clamp01(factor/duration);
            transform.position += direction * force * Time.deltaTime*factor;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    protected void ResetWanderBehaviour()
    {
        lastWanderPosition = transform.position + Vector3.Forward;
    }

    protected void Wander(float areaWidth, float reachDistance, float movementSpeed)
    {
        RaycastHit hit;
        
        //int EnemiesLayer = Collisions.GetLayerBit("Enemy");
        int WallLayer = Collisions.GetLayerBit("WorldLimits");
        int LayerMask = WallLayer;

        if (!Collisions.Raycast(transform.position + transform.Up, transform.Forward, reachDistance, out hit, LayerMask))
        {
            transform.position += transform.Forward * movementSpeed * Time.deltaTime;
            if(Vector3.Distance(lastWanderPosition, transform.position) > reachDistance)
                return;
        }
        
        entireWanderingRange = false;
        for (int i = 0; i < 2; i++)
        {
            int tries = 0;
            while (tries < 10)
            {
                float newDir = Loopie.Random.Range(entireWanderingRange? -180.0f : -areaWidth, entireWanderingRange? 180.0f : areaWidth);
                
                Vector3 newDirection = Vector3.RotateAroundAxis(transform.Forward,transform.Up, newDir);
                if (!Collisions.Raycast(transform.position + transform.Up, newDirection, reachDistance, out hit, LayerMask))
                {
                    transform.LookAt(transform.position + newDirection, transform.Up);
                    lastWanderPosition = transform.position + transform.Forward * reachDistance*1.5f;
                    return;
                }
                tries++;
            }
            entireWanderingRange = true;
        }
    }
}


