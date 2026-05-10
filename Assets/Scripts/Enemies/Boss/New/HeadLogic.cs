using System;
using System.Collections;
using Loopie;

public class HeadLogic : Component
{
    [Header("References")]
    public Entity headColliderEntity;
    BoxCollider headCollider;
    public Entity startPointEntity;

    [ReadOnly][ShowInInspector] bool isDefeated;
    [ReadOnly][ShowInInspector] bool isVulnerable;
    [ReadOnly][ShowInInspector] bool isIgnited;

    BossLogic owner;

    public bool HasBeenHit() => headCollider.HasCollided;
    public bool IsDefeated() => isDefeated;
    public bool IsVulnerable() => isVulnerable;
    public bool IsIgnited() => isIgnited;

    void OnCreate()
    {
        headCollider = headColliderEntity.GetComponent<BoxCollider>();
    }

    void OnPostCreate() 
    { 
        transform.position = startPointEntity.transform.position;
    }

    public void SetOwner(BossLogic boss)
    {
        owner = boss;
    }

    public void Regenerate()
    {
        isDefeated = false;
        isVulnerable = false;
        isIgnited = false;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}