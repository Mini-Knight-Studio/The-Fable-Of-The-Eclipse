using System;
using Loopie;

class Slime : Enemy
{
    public int Stage;
    public float SlimeSize;
    public int SplitAmmount;

    public float ViewFieldWidth;
    public float ViewFieldFar;

    public string targetEntityName = "Player";

    public float Speed;
    public int Damage;
    public float AttackReachDistance;

    public float CooldownTime;
    private Effect effect;
    
    void OnCreate()
    {
        SetEnemy("Slime_Reference");
        SetTarget(targetEntityName);
        SetStage(Stage);
        effect = entity.GetComponent<Effect>();
    }

    void OnUpdate()
    {
        UpdateEnemy();
        if(Input.IsKeyDown(KeyCode.P))
        {
            health.Damage(1);
        }
        
        #region Movement
        if(DetectedTargetInViewField(ViewFieldWidth, ViewFieldFar*Stage) && HasAttackCooldown())
        {
            transform.LookAt(target.transform.position, transform.Up);
            Move(transform.Forward);
        }
        #endregion
        #region Attack
        if (GetDirectionToTarget().magnitude <= AttackReachDistance*Stage)
        {
            //Init attack animation
            RaycastHit hit;
            if (Collisions.Raycast(transform.position, transform.Forward, AttackReachDistance*Stage, out hit))
            {
                if (hit.entity.ID == target.ID && HasAttackCooldown())
                {
                    Attack();
                }
            }
            StartAttackCooldown(CooldownTime);
            //End attack animation
        }
        #endregion
        #region Health
        health.UpdateHealth();
        if(health.IsDead())
        {
            //Debug.Log("I'm dead");
            if (Stage > 0)
                Split();
            entity.Destroy();
        }
        #endregion
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * Speed*Stage/2;
    }

    public void SetStage(int stage)
    {
        Stage = stage;
        transform.scale = Vector3.One*SlimeSize*stage;

        Debug.Log($" SCALE ->{Stage} -> {transform.scale.x}");
    }

    public void Attack()
    {
        targetHealth.Damage(Damage);
        if(effect != null)
        {
            targetHealth.AddEffect(effect);
        }
    }

    public void Split()
    {
        for (int i = 0; i < SplitAmmount; i++)
        {
            Entity newslime = reference.Clone(true);
            Slime slimecomp = newslime.GetComponent<Slime>();
            newslime.SetActive(true);
          
            slimecomp.SetStage(Stage - 1);
        }
    }
};
