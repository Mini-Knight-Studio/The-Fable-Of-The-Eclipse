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
        Debug.Log($"{Vector3.Distance(lastWanderPosition, transform.position)} / {reachDistance}");
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
                Debug.Log($"{newDir}");
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

    //private Vector2 SearchNewWanderDirection(float areaWidth, float reachDistance, float rotationSpeed)
    //{
    //    Vector2 areaToGetDirection = new Vector2(-1.0f, -1.0f);
    //    Vector3 checks = Vector3.Zero;

    //    checks = CheckFrontalCollisions(areaWidth, reachDistance);
    //    if (checks.y == 1)
    //    {
    //        areaToGetDirection.x = 0;
    //        areaToGetDirection.y = 0;
    //    }
    //    if (checks.x == 1)
    //    {
    //        areaToGetDirection.x = -areaWidth;
    //    }
    //    if (checks.z == 1)
    //    {
    //        areaToGetDirection.y = areaWidth;
    //    }

    //    if(Mathf.Abs(areaToGetDirection.x)-Mathf.Abs(areaToGetDirection.y) < 1.0f)
    //    {
    //        wanderDirection = Loopie.Random.RandomBool();
    //        if (areaToGetDirection.x == -areaWidth)
    //            wanderDirection = false;
    //        else if (areaToGetDirection.y == areaWidth)
    //            wanderDirection = true;

    //        transform.Rotate(new Vector3(0,Time.deltaTime * rotationSpeed * (wanderDirection? 1:-1),0), Transform.Space.LocalSpace);
    //        return Vector2.Zero;
    //    }
    //    return areaToGetDirection;
    //}

    //private Vector3 CheckFrontalCollisions(float areaWidth, float reachDistance)
    //{
    //    Vector3 checks = Vector3.Zero;
    //    RaycastHit hit;
    //    Vector3 leftZone = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, -areaWidth);
    //    Vector3 rightZone = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, areaWidth);

    //    int EnemiesLayer = Collisions.GetLayerBit("Enemy");
    //    int WallLayer = Collisions.GetLayerBit("WorldLimits");
    //    int LayerMask = EnemiesLayer | WallLayer;

    //    if (!Collisions.Raycast(transform.position + transform.Up, leftZone, reachDistance * 1.1f, out hit, LayerMask))
    //    {
    //        checks.x = 1;
    //    }
    //    if (!Collisions.Raycast(transform.position + transform.Up, transform.Forward, reachDistance * 1.1f, out hit, LayerMask))
    //    {
    //        checks.y = 1;
    //    }
    //    if (!Collisions.Raycast(transform.position + transform.Up, rightZone, reachDistance * 1.1f, out hit, LayerMask))
    //    {
    //        checks.z = 1;
    //    }
    //    return checks;
    //}
}


