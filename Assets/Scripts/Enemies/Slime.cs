using System;
using Loopie;

class Slime : Enemy
{
    public string Reference;

    public int SlimeStage;
    public float SlimeStageSize;
    public int SplitAmmount;
    public float SplitDistance;
    protected Vector3 SplitDirection;
    protected float parentY;

    public float ViewFieldWidth;
    public float ViewFieldFar;

    public float Speed;
    public float KnockbackForce;
    public float KnockbackTime;
    public int Damage;
    public float AttackReachDistance;

    public float CooldownTime;
    protected float splitLerpTimer;
    private bool isSpawning;

    void OnCreate()
    {
        SetEnemy(Reference);
        SetTarget();
        SetStage(SlimeStage);
        
    }

    void OnUpdate()
    {
        UpdateEnemy();
        if (Input.IsKeyDown(KeyCode.P) || Input.IsGamepadButtonDown(GamepadButton.GAMEPAD_A))
        {
            health.Damage(1);
            StartCoroutine(movement.Push(KnockbackForce, KnockbackTime, GetDirectionToTarget() * -1));
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
                if (DetectedTargetInViewField(ViewFieldWidth, ViewFieldFar * SlimeStage) && !HasAttackCooldown())
                {
                    transform.LookAt(target.transform.position, transform.Up);
                    Move(transform.Forward);
                    ResetWander();
                }
                else
                    Wander(ViewFieldWidth, ViewFieldFar * SlimeStage, Speed);
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
                if (SlimeStage > 1)
                    Split();
                entity.Destroy();
            }
            #endregion
        }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * Speed * SlimeStage / 2;
    }

    public void SetStage(int newStage)
    {
        SlimeStage = newStage;
        transform.scale = Vector3.One * SlimeStageSize * SlimeStage;
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
        transform.position = Vector3.Lerp(transform.position, transform.position + SplitDirection.normalized * SlimeStage * SplitDistance / 20.0f, splitLerpTimer);
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
            slime_component.SetStage(SlimeStage - 1);
            slime_component.parentY = transform.position.y;
            slime_component.ResetWander();
            new_slime.SetActive(true);
        }
    }

    void OnDrawGizmo()
    {
        Vector3 leftZone = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, -ViewFieldWidth);
        Vector3 rightZone = Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, ViewFieldWidth);
        Gizmo.DrawLine(transform.position + transform.Forward * ViewFieldFar * SlimeStage, transform.position - leftZone * -1.0f * ViewFieldFar * SlimeStage, Color.White);
        Gizmo.DrawLine(transform.position + transform.Forward * ViewFieldFar * SlimeStage, transform.position - rightZone * -1.0f * ViewFieldFar * SlimeStage, Color.White);
        Gizmo.DrawLine(transform.position, transform.position + rightZone * ViewFieldFar * SlimeStage, Color.White);
        Gizmo.DrawLine(transform.position, transform.position + leftZone * ViewFieldFar * SlimeStage, Color.White);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};
