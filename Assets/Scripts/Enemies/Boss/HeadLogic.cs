using Loopie;
using System;

public class HeadLogic : Component
{
    [Header("References")]
    public Entity headColliderEntity;
    BoxCollider headCollider;
    public Entity startPointEntity;

    [Space(10)]
    [Header("Settings")]
    [ShowInInspector] float minLookAngle = -60.0f;
    [ShowInInspector] float maxLookAngle = 60.0f;
    [ShowInInspector] float rotationSpeed = 5.0f;

    [ReadOnly][ShowInInspector] bool isDefeated;
    [ReadOnly][ShowInInspector] bool isVulnerable;


    BossLogic owner;

    public bool HasBeenHit() => headCollider.HasCollided;
    public bool IsDefeated() => isDefeated;
    public bool IsVulnerable() => isVulnerable;

    void OnCreate()
    {
        headCollider = headColliderEntity.GetComponent<BoxCollider>();
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

    public void Regenerate()
    {
        isDefeated = false;
        isVulnerable = false;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}



