using System;
using System.Collections;
using Loopie;

class Blob : Enemy
{
    [Header("Global Enemy")]
    public Entity Reference;
    public Vector2 ViewField;
    public float ForcedDetectionDistance;
    [Space(5)]
    public int Damage;
    public float ReachDistance;
    public float KnockbackForce;
    public float KnockbackTime;
    [Space(5)]
    public float PreparationTime;
    public float AttackCooldown;
    [Space(10)]
    [Header("Blob")]
    public int Stage;
    public float StageScale;
    [Space(5)]
    public int SplitAmmount;
    public float SplitDistance;

    //Private//
    [HideInInspector]
    protected float parentY;

    private Vector3 SplitDirection;
    private bool spawn;
    private bool isSpawning;
    private int LayerOverride;

    void OnCreate()
    {
        SetEnemy(Reference, AttackCooldown, PreparationTime, ReachDistance * Stage, "Blob");
        SetStage(Stage);
        int EnemyLayer = Collisions.GetLayerBit("Player");
        int PlayerHitLayer = Collisions.GetLayerBit("PlayerTrigger");
        LayerOverride = EnemyLayer | PlayerHitLayer;
        spawn = false;
        isSpawning = false;
    }

    void OnUpdate()
    {
        if (Pause.isPaused)
        {
            return;
        }

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
            
            if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(ForcedDetectionDistance))
            {
                PlayAnimation("Armature|Chase", 0.25f);
                transform.LookAt(target.transform.position, transform.Up);
                movement.Move(Stage, transform.Forward);
                ResetWander();
                #region Attack
                if (Vector3.Distance(target.transform.position, transform.position) < ReachDistance * Stage)
                {
                    StartCoroutine(DoAttack(Damage));
                }
                #endregion
            }
            else
            {
                PlayAnimation("Armature|IdleWalk", 0.25f);
                Wander(ViewField.x, ViewField.y * Stage, Stage);
            }
            #endregion
        }
        #region Health
        if (health.IsDead())
        {
            if (Stage > 1)
                Split();
            entity.Destroy();
        }
        #endregion
    }

    public void SetStage(int newStage)
    {
        Stage = newStage;
        transform.scale = Vector3.One * StageScale * Stage;
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
            transform.position = Vector3.Lerp(transform.position, transform.position + SplitDirection.normalized * Stage * SplitDistance / 20.0f, timer);
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
            Blob_component.SetStage(Stage - 1);
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
        DebugViewField(ViewField.x, ViewField.y);
        DebugForcedDetection(ForcedDetectionDistance);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};