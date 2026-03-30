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
        if (Input.IsKeyDown(KeyCode.P) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A))
        {
            health.Damage(1);
            StartCoroutine(ApplyKnockback(KnockbackForce, GetDirectionToTarget() * -1, KnockbackTime));
        }

        if (!HasAttackCooldown())
        { 
            attackBox.entity.SetActive(true);

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
                    ResetWander();
                }
                else
                    Wander(ViewFieldWidth, ViewFieldFar * Stage, Speed);
                #endregion
                #region Attack
                if (attackBox.IsColliding && !HasAttackCooldown())
                {
                    Attack();
                }
                #endregion
            }
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
    }

    public void Attack()
    {
        targetHealth.Damage(Damage);
        if (effect != null)
        {
            targetHealth.AddEffect(effect);
        }
        StartAttackCooldown(CooldownTime);
        attackBox.entity.SetActive(false);
    }

    public void SplitLerp()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + SplitDirection.normalized * Stage * SplitDistance / 20.0f, splitLerpTimer);
        if (splitLerpTimer < 0.95f)
            collision.Enabled = false;
        else
            collision.Enabled = true;
        transform.position = new Vector3(transform.position.x, parentY, transform.position.z);
    }

    protected void Split()
    {
        int random = Loopie.Random.Range(0, 360);
        for (int i = 0; i < SplitAmmount; i++)
        {
            Entity new_slime = reference.Clone(true);
            new_slime.transform.rotation = transform.rotation;
            new_slime.transform.position = transform.position;
            Slime slime_component = new_slime.GetComponent<Slime>();
            slime_component.splitLerpTimer = 0;
            slime_component.SplitDirection = new Vector3(Mathf.Sin(random + 180 * i / SplitAmmount), 0, Mathf.Cos(random + 180 * i / SplitAmmount));
            slime_component.SetStage(Stage - 1);
            slime_component.parentY = transform.position.y;
            slime_component.ResetWander();
            new_slime.SetActive(true);
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
