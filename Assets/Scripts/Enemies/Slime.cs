using System;
using Loopie;

class Slime : Enemy
{
    public int Stage = 3;
    public float SlimeSize;

    public float ViewFieldWidth;
    public float ViewFieldFar;

    public string targetEntityName = "Player";

    public float Speed;
    public int Damage;
    public float AttackReachDistance;

    public float CooldownTime;
    private float cooldownTimer;
    private Effect effect;
    private Health targetHealth;
    private Health health;
    
    void OnCreate()
    {
        SetTarget(targetEntityName);
        SetStage(Stage);
        targetHealth = target.GetComponent<Health>();
        targetHealth = entity.GetComponent<Health>();
        effect = entity.GetComponent<Effect>();
    }

    void OnUpdate()
    {
        Vector3 front = transform.Forward;
        Vector3 targetDirection = target.transform.position - transform.position;
        if (Mathf.Abs(Vector3.Angle(front, targetDirection)) <= ViewFieldWidth)
        {
            RaycastHit hit;

            if (Collisions.Raycast(transform.position, targetDirection, ViewFieldFar*Stage, out hit))
            {
                if(hit.entity.ID == target.ID && cooldownTimer <= 0)
                {
                    transform.LookAt(target.transform.position, transform.Up);
                    Move(transform.Forward);
                }
            }
        }

        if(cooldownTimer > 0)
            cooldownTimer-= Time.deltaTime;
        if(cooldownTimer < 0)
            cooldownTimer = 0;
        if(targetDirection.magnitude <= AttackReachDistance)
        {
            //Init attack animation
            RaycastHit hit;
            if (Collisions.Raycast(transform.position, transform.Forward, AttackReachDistance*Stage, out hit))
            {
                if (hit.entity.ID == target.ID && cooldownTimer <= 0)
                {
                    Attack();
                }
            }
            StartCooldown();
            //End attack animation
        }

        health.UpdateHealth();
        if(health.IsDead())
        {
            Debug.Log("I'm dead");
        }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * Speed*Stage/2;
    }

    public void SetStage(int stage)
    {
        Stage = stage;
        transform.scale = Vector3.One*SlimeSize*stage;
    }

    public void Attack()
    {
        targetHealth.Damage(Damage);
        if(effect != null)
        {
            targetHealth.AddEffect(effect);
        }
    }

    public void StartCooldown()
    {
        cooldownTimer = CooldownTime;
    }
};
