using System;
using Loopie;
public enum EnemyState
{
    WANDER,
    CHASE,
    CHARGE,
    ATTACK,
    COOLDOWN,
    SPECIAL,
}

class EnemyAnimation : Component
{
    private Animator modelAnimator;
    private string StringSubtract;

    [Header("Clips")]
    public string WalkClip;
    public string ChaseClip;
    public string ChargeAttackClip;
    public string AttackClip;
    public string CooldownClip;

    private EnemyState state;

    void OnCreate()
    {
        modelAnimator = entity.GetComponent<Animator>();
        state = EnemyState.WANDER;
    }

    void OnUpdate()
    {

    }
};