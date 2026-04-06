using System;
using System.Collections;
using Loopie;

class Slime : Enemy
{
    public string Reference;

    public int SlimeStage;
    public float SlimeStageSize;

    public int SplitAmmount;
    public float SplitDistance;
    private Vector3 SplitDirection;
    private bool spawn;
    private bool isSpawning;

    protected float parentY;

    public float ViewFieldWidth;
    public float ViewFieldFar;

    public float KnockbackForce;
    public float KnockbackTime;

    public int Damage;
    public float AttackReachDistance;
    public float AttackCooldownTime;
    public float AttackPreparationTime;

    public float TargetForcedDetectionDistance;

    private int LayerOverride;

    void OnCreate()
    {
        SetEnemy(Reference, AttackCooldownTime, AttackPreparationTime, AttackReachDistance * SlimeStage);
        SetStage(SlimeStage);
        int EnemyLayer = Collisions.GetLayerBit("Player");
        int PlayerHitLayer = Collisions.GetLayerBit("PlayerTrigger");
        LayerOverride = EnemyLayer | PlayerHitLayer;
        spawn = false;
        isSpawning = false;
    }

    void OnUpdate()
    {
        //Temporal
        TestKeys();
        //
        if (spawn)
        {
            StartCoroutine(SplitLerp());
        }

        UpdateEnemy();

        if (!isSpawning && !isAttacking)
        {
            attackBox.entity.SetActive(true);
            #region Movement
            if (DetectedTargetInViewField(ViewFieldWidth, ViewFieldFar * SlimeStage) || DetectedTargetInDistance(TargetForcedDetectionDistance * SlimeStage))
            {
                transform.LookAt(target.transform.position, transform.Up);
                movement.Move(SlimeStage, transform.Forward);
                ResetWander();
                #region Attack
                if (Vector3.Distance(target.transform.position, transform.position) < AttackReachDistance * SlimeStage)
                {
                    StartCoroutine(DoAttack(Damage));
                }
                #endregion
            }
            else
                Wander(ViewFieldWidth, ViewFieldFar * SlimeStage, SlimeStage);
            #endregion

        }
        #region Health
        if (health.IsDead())
        {
            if (SlimeStage > 1)
                Split();
            entity.Destroy();
        }
        #endregion
    }

    public void SetStage(int newStage)
    {
        SlimeStage = newStage;
        transform.scale = Vector3.One * SlimeStageSize * SlimeStage;
    }

    private IEnumerator SplitLerp()
    {
        float timer = 0.0f;
        spawn = false;
        isSpawning = true;
        transform.position = new Vector3(transform.position.x, parentY, transform.position.z);
        collision.AddExcludeMask(LayerOverride);
        while (timer < 1.0f)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, transform.position + SplitDirection.normalized * SlimeStage * SplitDistance / 20.0f, timer);
            yield return null;
        }
        collision.RemoveExcludeMask(LayerOverride);
        isSpawning = false;
    }

    protected void Split()
    {
        collision.AddExcludeMask(LayerOverride);
        int random = Loopie.Random.Range(0, 360);
        for (int i = 0; i < SplitAmmount; i++)
        {
            Entity new_slime = reference.Clone(true);
            Slime slime_component = new_slime.GetComponent<Slime>();
            slime_component.collision.AddExcludeMask(LayerOverride);
            slime_component.SplitDirection = new Vector3(Mathf.Sin(random + 180 * i / SplitAmmount), 0, Mathf.Cos(random + 180 * i / SplitAmmount));
            slime_component.SetStage(SlimeStage - 1);
            new_slime.transform.position = transform.position;
            new_slime.transform.rotation = transform.rotation;
            new_slime.Name = entity.Name;
            slime_component.parentY = transform.position.y;
            slime_component.spawn = true;
            slime_component.isSpawning = true;
            slime_component.ResetWander();
            slime_component.StartHitCooldown(target.Combat.GetAttackDuration());
            new_slime.SetActive(true);
        }
    }

    private void TestKeys()
    {
        if (Input.IsKeyDown(KeyCode.P))
        {
            Hit(1);
            StartCoroutine(movement.Push(KnockbackForce, KnockbackTime, GetDirectionToTarget() * -1));
        }
    }

    void OnDrawGizmo()
    {
        DebugViewField(ViewFieldWidth, ViewFieldFar * SlimeStage);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};