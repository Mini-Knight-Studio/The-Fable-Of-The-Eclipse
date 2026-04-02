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
    private Player playerCentral;
    private bool hasBeenHitThisAttack = false;

    public float Speed;
    public float KnockbackForce;
    public float KnockbackTime;
    public int Damage;
    public float AttackReachDistance;

    public float CooldownTime;
    protected float splitLerpTimer;
    private bool isSpawning;
    private Effect effect;

    // SFX
    public Entity SlimeDeath_SFX;
    public AudioSource deathSFXSource;

    public Entity SlimeImpact_SFX;
    public AudioSource impactSFXSource;

    void OnCreate()
    {
        SetEnemy("Slime_Reference");
        SetTarget(targetEntityName);
        SetStage(Stage);
        effect = entity.GetComponent<Effect>();

        SlimeDeath_SFX = Entity.FindEntityByName("SlimeDeath_SFX");
        deathSFXSource = SlimeDeath_SFX.GetComponent<AudioSource>();
        SlimeImpact_SFX = Entity.FindEntityByName("SlimeImpact_SFX");
        impactSFXSource = SlimeImpact_SFX.GetComponent<AudioSource>();

        Entity playerEntity = Entity.FindEntityByName(targetEntityName);
        if (playerEntity != null)
        {
            playerCentral = playerEntity.GetComponent<Player>();
        }
    }

    void OnUpdate()
    {
        UpdateEnemy();

        if (playerCentral != null && playerCentral.Combat != null)
        {
            BoxCollider swordCol = playerCentral.Combat.swordTrigger.GetComponent<BoxCollider>();

            if (playerCentral.Combat.isAttacking && swordCol != null && swordCol.IsColliding)
            {
                if (!hasBeenHitThisAttack)
                {
                    ReceiveDamage();
                    hasBeenHitThisAttack = true;
                }
            }

            if (!playerCentral.Combat.isAttacking)
            {
                hasBeenHitThisAttack = false;
            }
        }
        if (Input.IsKeyDown(KeyCode.P) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A))
        {

            health.Damage(1);
            StartCoroutine(ApplyKnockback(KnockbackForce, GetDirectionToTarget() * -1, KnockbackTime));

            if (health.GetActualHealth() == 0)
            {
                deathSFXSource.Play();
            }
            else 
            {
                impactSFXSource.Play();
            }
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
                ResetWanderBehaviour();
            }
            else
                Wander(ViewFieldWidth,ViewFieldFar * Stage, Speed);
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
    private void ReceiveDamage()
    {
        health.Damage(1);

        StartCoroutine(ApplyKnockback(KnockbackForce, GetDirectionToTarget() * -1, KnockbackTime));

        if (health.GetActualHealth() <= 0)
        {
            if (deathSFXSource != null) deathSFXSource.Play();
        }
        else
        {
            if (impactSFXSource != null) impactSFXSource.Play();
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


        //Destroy after vertical 2
        WobblyEffect slimeEffect = entity.GetComponent<WobblyEffect>();
        slimeEffect.SetBaseScale(SlimeSize * stage);
        //
    }

    public void Attack()
    {
        if (playerCentral != null && playerCentral.PlayerHealth != null)
        {
            playerCentral.PlayerHealth.Damage(Damage);

            if (effect != null) { playerCentral.PlayerHealth.AddEffect(effect); }

            Vector3 pushDir = (target.transform.position - transform.position).normalized;
            pushDir.y = 0;

            if (playerCentral.Movement != null) { playerCentral.Movement.ApplyKnockback(pushDir, KnockbackForce, KnockbackTime); }

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
            slimecomp.ResetWanderBehaviour();
            newslime.SetActive(true);
        }
    }

    void OnDrawGizmo()
    {
        Vector3 leftZone = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, -ViewFieldWidth);
        Vector3 rightZone = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, ViewFieldWidth);
        Gizmo.DrawLine(transform.position + transform.Forward * ViewFieldFar * Stage, transform.position - leftZone * -1.0f * ViewFieldFar * Stage, Color.White);
        Gizmo.DrawLine(transform.position + transform.Forward * ViewFieldFar * Stage, transform.position - rightZone * -1.0f * ViewFieldFar * Stage, Color.White);
        Gizmo.DrawLine(transform.position, transform.position + rightZone * ViewFieldFar * Stage, Color.White);
        Gizmo.DrawLine(transform.position, transform.position + leftZone * ViewFieldFar * Stage, Color.White);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};
