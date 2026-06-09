using Loopie;
using System;

public class HeadLogic : Component
{
    [Header("References")]
    public Entity headColliderEntity;
    BoxCollider headCollider;
    public Entity startPointEntity;
    public Entity headAniamtorEntity;
    public Entity pulseEntity;

    Animator headAniamtor;
    EmissivePulse emmisivePusle;

    [Space(10)]
    [Header("Settings")]
    [ShowInInspector] float minLookAngle = -60.0f;
    [ShowInInspector] float maxLookAngle = 60.0f;
    [ShowInInspector] float rotationSpeed = 5.0f;

    [ReadOnly][ShowInInspector] bool isDefeated;
    [ReadOnly][ShowInInspector] bool isVulnerable;


    BossLogic owner;

    public bool HasBeenHit()
    {
        if (Player.Instance.Combat.TemporalFunctionIsAttacking())
            if (headCollider.HasCollided)
                return true;
        return false;
    }
    public bool IsDefeated() => isDefeated;
    public bool IsVulnerable() => isVulnerable;

    void OnCreate()
    {
        headCollider = headColliderEntity.GetComponent<BoxCollider>();
        headAniamtor = headAniamtorEntity.GetComponent<Animator>();
        emmisivePusle = pulseEntity.GetComponent<EmissivePulse>();

        //SetVulnerable(false);
        headAniamtor.Play("Armature|Idle");
    }

    void OnPostCreate() 
    { 
        transform.position = startPointEntity.transform.position;
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        if (owner.IsDefeated() || owner.IsVulnerable())
            return;

        LookAtTarget();
    }

    public void SetOwner(BossLogic boss)
    {
        owner = boss;
    }

    void LookAtTarget()
    {
        //Vector3 targetPos = owner.GetTarget().transform.position;
        //Vector3 myPos = transform.position;
        //targetPos.y = myPos.y;

        //transform.LookAt(targetPos, Vector3.Up);
    }

    public void SetVulnerable(bool state)
    {
        emmisivePusle.Enabled = state;
        if(!state)
            emmisivePusle.Reset();
    }

    public void Regenerate()
    {
        isDefeated = false;
        isVulnerable = false;
        SetVulnerable(false);
    }

    public void Restart()
    {
        StopAllOwnedCoroutines();

        isDefeated = false;
        isVulnerable = false;

        // Reset position and rotation
        transform.position = startPointEntity.transform.position;
        transform.rotation = Vector3.Zero;

        headColliderEntity.SetActive(false);

        SetVulnerable(false);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}



