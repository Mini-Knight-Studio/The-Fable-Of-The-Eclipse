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
    public float PushForceScale;
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
    private bool splitting;
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
        splitting = false;
    }

    void OnUpdate()
    {
        if (spawn)
        {
            StartCoroutine(SplitLerp());
        }

        #region Health
        if (health.IsDead())
        {
            movement.CanMove = false;
            if (!splitting)
            {
                splitting = true;
                if (Stage > 1)
                    Split();
                animator.PlayClip("Armature|SplitStart", false, 0.0f);
                feedback.PlaySound("Death");
            }
            if (animator.AnimationEnded())
                entity.Destroy();
        }
        #endregion
        if (!isSpawning && !splitting)
        {
            Hit(1, PushForceScale, "Armature|Walk");
            if(!isAttacking)
            {
                #region Movement
                if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(Stage * StageScale + ForcedDetectionDistance))
                {
                    animator.PlayClip("Armature|Chase", true, 0.25f);
                    transform.LookAt(Player.Instance.transform.position, transform.Up);
                    movement.Move(4-Stage, transform.Forward);
                    ResetWander();
                    #region Attack
                    if (Vector3.Distance(Player.Instance.transform.position, transform.position) < Stage * StageScale + (ReachDistance*0.25f) && !health.IsDead())
                    {
                        StartCoroutine(Attack(Stage * StageScale + ReachDistance, PreparationTime, AttackCooldown, Damage, "Armature|ChargeAttack", "Armature|Attack", "Armature|Chase", "Armature|Walk"));
                    }
                    #endregion
                }
                else
                {
                    animator.PlayClip("Armature|Walk", true, 0.25f);
                    Wander(ViewField, 4-Stage);
                }
                #endregion
            }
        }
    }

    public void SetStage(int newStage)
    {
        Stage = newStage;
        transform.scale = Vector3.One * StageScale * Stage;
    }

    private IEnumerator SplitLerp()
    {
        animator.PlayClip("Armature|SplitEnd", false, 0.0f);
        float timer = 0.0f;
        spawn = false;
        isSpawning = true;
        transform.position = new Vector3(transform.position.x, parentY, transform.position.z);
        collision.AddExcludeMask(LayerOverride);
        while (timer < animator.ClipDuration())
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
        collision.SetActive(false);
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
            new_Blob.SetActive(true);
        }
    }

    void OnDrawGizmo()
    {
        DebugViewField(ViewField.x, ViewField.y);
        if (!DetectedTargetInViewField(ViewField.x, ViewField.y) && !DetectedTargetInDistance(Stage * StageScale + ForcedDetectionDistance))
            DebugToTargetLine(Stage * StageScale + ForcedDetectionDistance, Color.Red);
        else
            DebugForwardLine(Stage * StageScale + ReachDistance, Color.Green);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};