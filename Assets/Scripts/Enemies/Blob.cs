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
    public float AttackDistance;
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
        if (Pause.isPaused)
        {
            return;
        }
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
                animator.PlayClip("Armature|Split", false, 0.0f, false, true);
                feedback.PlaySound("Death");
            }
            if (animator.AnimationEnded())
                entity.Destroy();
        }
        #endregion
        if (!isSpawning && !splitting && !health.IsDead())
        {
            Hit(1, PushForceScale, "Armature|GetHit");
            movement.CanMove = (animator.CurrentClip() == "Armature|GetHit" || isAttacking)? false : true;
            if(!isAttacking)
            {
                #region Movement
                if (DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(GetEntityForwardBase() + ForcedDetectionDistance))
                {
                    animator.PlayClip("Armature|Chase", true, 0.25f);
                    transform.LookAt(Player.Instance.transform.position, transform.Up);
                    movement.Move(4-Stage, transform.Forward);
                    ResetWander();
                    #region Attack
                    if (Vector3.Distance(Player.Instance.transform.position, transform.position) < GetEntityForwardBase() + ReachDistance)
                    {
                        attackCoroutine = StartCoroutine(Attack(AttackDistance, PreparationTime, AttackCooldown,0, Damage*Stage, "Armature|ChargeAttack", "Armature|Attack", "Armature|Stunt", "Armature|Walk"));
                    }
                    #endregion
                }
                else if (!health.IsDead())
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

    public override void Hit(int points, float force_scale, string hit_clip)
    {
        if (OnHitCooldown() || !health.canBeDamaged) return;
        if (Player.Instance.Combat.TemporalFunctionIsAttacking())
        {
            if (hitbox.HasCollided)
            {
                StopCoroutine(attackCoroutine);
                isAttacking = false;
                movement.CanMove = true;
                base.Hit(points, force_scale, hit_clip);
            }
        }
    }

    private IEnumerator SplitLerp()
    {
        animator.PlayClip("Armature|Spawn", false, 0.0f, false, true);
        float timer = 0.0f;
        spawn = false;
        isSpawning = true;
        transform.position = new Vector3(transform.position.x, parentY, transform.position.z);
        collision.AddExcludeMask(LayerOverride);
        Vector3 startPosition = transform.position;
        float animationTime = animator.ClipDuration();
        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, startPosition + SplitDirection.normalized * Stage * SplitDistance, timer / animationTime);
            yield return null;
        }

        transform.LookAt(Player.Instance.transform.position, Vector3.Up);
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
            Blob_component.feedback.SetParticlesState("Hurt", false);

            Blob_component.entity.transform.rotation = Vector3.Zero;
            Blob_component.hitbox.entity.transform.rotation = Vector3.Zero;
            Blob_component.animator.model.transform.rotation = Vector3.Zero;
            Blob_component.feedback.FeedbackEntity.transform.rotation = Vector3.Right * 90;
            new_Blob.SetActive(true);
        }
    }

    void OnDrawGizmo()
    {
        DebugViewField(ViewField.x, ViewField.y);
        #region Target Not Detected
        if (!DetectedTargetInViewField(ViewField.x, ViewField.y) && !DetectedTargetInDistance(GetEntityForwardBase() + ForcedDetectionDistance))
            Gizmo.DrawCircle(transform.position, GetEntityForwardBase() + ForcedDetectionDistance, Vector3.Up, 32, Color.White);
        #endregion
        if(DetectedTargetInViewField(ViewField.x, ViewField.y) || DetectedTargetInDistance(GetEntityForwardBase() + ForcedDetectionDistance))
        {
            if (ReachDistance < AttackDistance)
            {
                DebugForwardLine(GetEntityForwardBase() + (ReachDistance), Color.Orange);
                DebugForwardLine(GetEntityForwardBase() + (AttackDistance), Color.Red, GetEntityForwardBase() + (ReachDistance));
            }
            else if (ReachDistance > AttackDistance)
            {
                DebugForwardLine(GetEntityForwardBase() + (ReachDistance), Color.Orange, GetEntityForwardBase() + (AttackDistance));
                DebugForwardLine(GetEntityForwardBase() + (AttackDistance), Color.Red);
            }
            else
            {
                DebugForwardLine(GetEntityForwardBase() + (ReachDistance), Color.Red);
            }
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};