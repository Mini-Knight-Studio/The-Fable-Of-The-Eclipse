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
            int LayerMask = PlayerLayer;

            if (Collisions.Raycast(transform.position, targetDirection, ViewFieldDepth, out hit,LayerMask))
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


        //CoroutineSystem.StartCoroutine(ApplyKnockback(0,new));
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
}


