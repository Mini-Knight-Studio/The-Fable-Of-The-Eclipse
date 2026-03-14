using System;
using Loopie;

class Slime : Enemy
{
    public int Stage;
    public float SlimeSize;
    public int SplitAmmount;
    public float SplitDistance;
    protected Vector3 SplitDirection;
    protected float parentY;

    public float ViewFieldWidth;
    public float ViewFieldFar;

    public string targetEntityName = "Player";

    public float Speed;
    public float KnockbackForce;
    public float KnockbackTime;
    public int Damage;
    public float AttackReachDistance;

    public float CooldownTime;
    protected float splitLerpTimer;
    private bool isSpawning;
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
        if (Input.IsKeyDown(KeyCode.P))
        {
            health.Damage(1);
            CoroutineSystem.StartCoroutine(ApplyKnockback(KnockbackForce, GetDirectionToTarget().normalized * -1, KnockbackTime));
        }

        if (!HasAttackCooldown())
            attackBox.SetActive(true);

        if (splitLerpTimer < 1.0f)
        {
            splitLerpTimer += Time.deltaTime;
            isSpawning = true;
        }
        else
        {
            splitLerpTimer = 1.0f;
            isSpawning = false;
            transform.position = new Vector3(transform.position.x, parentY, transform.position.z);
        }

        if (isSpawning)
        {
            SplitLerp();
        }
        else
        {
            #region Movement
            if (DetectedTargetInViewField(ViewFieldWidth, ViewFieldFar * Stage) && !HasAttackCooldown())
            {
                transform.LookAt(target.transform.position, transform.Up);
                Move(transform.Forward);
            }
            #endregion
            #region Attack
            if (attackBox.IsColliding && !HasAttackCooldown())
            {
                Attack();
            }
            #endregion
            #region Health
            health.UpdateHealth();
            if (health.IsDead())
            {
                //Debug.Log("I'm dead");
                if (Stage > 1)
                    Split();
                entity.Destroy();
            }
            #endregion
        }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * Speed * Stage / 2;
    }

    public void SetStage(int stage)
    {
        Stage = stage;
        transform.scale = Vector3.One * SlimeSize * stage;

        Debug.Log($" UUID ->{entity.ID} -> {transform.scale.x}");

        //Destroy after vertical 2
        WobblyEffect slimeEffect = entity.GetComponent<WobblyEffect>();
        slimeEffect.SetBaseScale(SlimeSize * stage);
        //
    }

    public void Attack()
    {
        targetHealth.Damage(Damage);
        if (effect != null)
        {
            targetHealth.AddEffect(effect);
        }
        StartAttackCooldown(CooldownTime);
        attackBox.SetActive(false);
    }

    public void SplitLerp()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + SplitDirection.normalized * Stage * SplitDistance / 20.0f, splitLerpTimer);
        if (splitLerpTimer < 0.95f)
            collider.Enabled = false;
        else
            collider.Enabled = true;
        transform.position = new Vector3(transform.position.x, parentY, transform.position.z);
    }

    public void Split()
    {
        int random = Loopie.Random.Range(0, 360);
        for (int i = 0; i < SplitAmmount; i++)
        {
            Entity newslime = reference.Clone(true);
            newslime.transform.rotation = transform.rotation;
            newslime.transform.position = transform.position;
            Slime slimecomp = newslime.GetComponent<Slime>();
            slimecomp.splitLerpTimer = 0;
            slimecomp.SplitDirection = new Vector3(Mathf.Sin(random + 180 * i / SplitAmmount), 0, Mathf.Cos(random + 180 * i / SplitAmmount));
            slimecomp.SetStage(Stage - 1);
            slimecomp.parentY = transform.position.y;
            newslime.SetActive(true);
        }
    }
};
