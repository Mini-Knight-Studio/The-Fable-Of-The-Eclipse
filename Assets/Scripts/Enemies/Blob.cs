using System;
using System.Collections;
using Loopie;

class Blob : Enemy
{
    public string Reference;

    public int BlobStage;
    public float BlobStageSize;

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
        SetEnemy(Reference, AttackCooldownTime, AttackPreparationTime, AttackReachDistance * BlobStage);
        SetStage(BlobStage);
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
            
            if (DetectedTargetInViewField(ViewFieldWidth, ViewFieldFar) || DetectedTargetInDistance(TargetForcedDetectionDistance))
            {
                PlayAnimation("Armature|Chase", 0.25f);
                transform.LookAt(target.transform.position, transform.Up);
                movement.Move(BlobStage, transform.Forward);
                ResetWander();
                #region Attack
                if (Vector3.Distance(target.transform.position, transform.position) < AttackReachDistance * BlobStage)
                {
                    StartCoroutine(DoAttack(Damage));
                }
                #endregion
            }
            else
            {
                PlayAnimation("Armature|IdleWalk", 0.25f);
                Wander(ViewFieldWidth, ViewFieldFar * BlobStage, BlobStage);
            }
            #endregion
        }
        #region Health
        if (health.IsDead())
        {
            if (BlobStage > 1)
                Split();
            entity.Destroy();
        }
        #endregion
    }

    public void SetStage(int newStage)
    {
        BlobStage = newStage;
        transform.scale = Vector3.One * BlobStageSize * BlobStage;
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
            transform.position = Vector3.Lerp(transform.position, transform.position + SplitDirection.normalized * BlobStage * SplitDistance / 20.0f, timer);
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
            Entity new_Blob = reference.Clone(true);
            Blob Blob_component = new_Blob.GetComponent<Blob>();
            Blob_component.collision.AddExcludeMask(LayerOverride);
            Blob_component.SplitDirection = new Vector3(Mathf.Sin(random + 180 * i / SplitAmmount), 0, Mathf.Cos(random + 180 * i / SplitAmmount));
            Blob_component.SetStage(BlobStage - 1);
            new_Blob.transform.position = transform.position;
            new_Blob.transform.rotation = transform.rotation;
            new_Blob.Name = entity.Name;
            Blob_component.parentY = transform.position.y;
            Blob_component.spawn = true;
            Blob_component.isSpawning = true;
            Blob_component.ResetWander();
            Blob_component.StartHitCooldown(target.Combat.GetAttackDuration());
            new_Blob.SetActive(true);
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
        DebugViewField(ViewFieldWidth, ViewFieldFar);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};