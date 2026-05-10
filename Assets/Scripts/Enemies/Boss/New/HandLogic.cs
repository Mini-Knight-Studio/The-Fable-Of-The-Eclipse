using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

public enum AttackType
{
    Punch, Spike
}

public class SequenceAction
{
    public AttackType type;
    public int count;

    int amountDone = 0;
    public SequenceAction(AttackType type, int count)
    {
        this.type = type;
        this.count = count;
    }

    public void Increment() => amountDone++;
    public void Decrement() => amountDone--;
    public bool IsCompleted() => amountDone >= count;
    public void Reset() => amountDone = 0;
}


public class HandLogic : Component
{
    [Header("References")]
    public Entity attackColliderEntity;
    public Entity burnColliderEntity;
    BoxCollider attackCollider;
    BoxCollider burnCollider;
    public Entity spikesEntity;
    public Entity spikesAttackColliderEntity;
    public BoxCollider spikesAttackCollider;
    public Entity startPointEntity;

    [Space(10)]
    [Header("Hand Settings")]
    [ShowInInspector] float followSpeed;
    [ShowInInspector] float timeToReturnToStartPoint;
    [ShowInInspector] float timeToTriggerPunch;
    [ShowInInspector] BossSide side;

    [Space(10)]
    [Header("Spike Settings")]
    public float spikeHideAttitude;
    public float spikeShowAttitude;

    [ReadOnly][ShowInInspector] bool isDefeated;
    [ReadOnly][ShowInInspector] bool isInCooldown;
    [ReadOnly][ShowInInspector] bool isVulnerable;
    [ReadOnly][ShowInInspector] bool isBusy;

    BossLogic owner;

    int sequenceMax = 3;
    int currentSequence = 0;
    int sequenceCounter = 0;

    float cooldownTimer = 0;
    float vulnerableTimer = 0;

    bool canBeStopped = true;

    List<SequenceAction> sequence = new List<SequenceAction>()
    {
        new SequenceAction(AttackType.Punch, 3),
        new SequenceAction(AttackType.Spike, 1),
        new SequenceAction(AttackType.Punch, 2)
    };
    [ReadOnly][ShowInInspector]int sequenceIndex = 0;

    public bool IsDefeated() => isDefeated;
    public bool IsInCooldown() => isInCooldown;
    public bool IsVulnerable() => isVulnerable;
    bool HasBeenHit() => burnCollider.HasCollided;
    bool HasHitTarget() => attackCollider.IsColliding;
    bool HasSpikeHitTarget() => spikesAttackCollider.IsColliding;


    void OnCreate()
    {
        attackCollider = attackColliderEntity.GetComponent<BoxCollider>();
        burnCollider = burnColliderEntity.GetComponent<BoxCollider>();
        spikesAttackCollider = spikesAttackColliderEntity.GetComponent<BoxCollider>();

        transform.position = startPointEntity.transform.position;
        isBusy = false;
    }

    void OnPostCreate() 
    { 

    }

    void OnUpdate()
    {
        ProcessStateTimer(ref cooldownTimer, ref isInCooldown);
        ProcessStateTimer(ref vulnerableTimer, ref isVulnerable);

        if(!isDefeated && isVulnerable && HasBeenHit())
        {
            Debug.Log($"Hand {entity.Name} has been hit while vulnerable");
            isDefeated = true;
            isVulnerable = false;
            canBeStopped = false;
            StopAllOwnedCoroutines();
            return;
        }

        if (isVulnerable || isDefeated || isInCooldown)
            return;

        if (isBusy)
        {
            if (canBeStopped && side != owner.GetCurrentSide())
            {
                StopAllOwnedCoroutines();
                StartCoroutine(ReturnToStartPoint());
                isBusy = false;
            }

        }
        else
        {
            if (side == owner.GetCurrentSide())
                PerformNextAction();
        }

    }

    public void SetOwner(BossLogic boss)
    {
        owner = boss;
    }

    public void PerformNextAction()
    {
        StopAllOwnedCoroutines();
        if (sequence[sequenceIndex].type == AttackType.Punch)
        {
            StartCoroutine(Punch());
        }
        else if (sequence[sequenceIndex].type == AttackType.Spike)
        {
            StartCoroutine(Spike());
        }
    }

    IEnumerator Punch()
    {
        Debug.Log($"Starting punch with {entity.Name}");
        isBusy = true;

        float timer = 0.0f;
        bool hitPlayer = false;
        while (!hitPlayer)
        {
            Vector3 targetPosition = owner.GetTarget().transform.position;
            targetPosition.y = owner.handsFollowAttitude;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );

            if(Vector3.Distance(transform.position, targetPosition) < 2f)
            {
                timer+= Time.deltaTime;
                if(timer> timeToTriggerPunch)
                    hitPlayer = true;
            }else
            {
                timer -= Time.deltaTime / 2;
            }

            yield return null;
        }
        canBeStopped = false;
        Debug.Log($"Hit player with {entity.Name}");

        StartCoroutine(MoveVertically(transform, owner.handsFollowAttitude, owner.handsFollowAttitude + 2, 0.1f, Mathf.LerpCurve.EaseOut));
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(MoveVertically(transform, owner.handsFollowAttitude + 2, owner.handsHitAttitude, 0.4f, Mathf.LerpCurve.ExponentialInOut));
        yield return new WaitForSeconds(0.3f);
        
        if(HasHitTarget())
        {
             owner.GetTarget().PlayerHealth.Damage(1);
        }
         yield return new WaitForSeconds(0.1f);
        ///CheckDamagePlayer


        //// Trigger Particles
        if (sequenceIndex == sequence.Count-1)
        {
            if (IncreaseSequenceCounter())
            {
                Debug.Log($"Set Vulnerable {entity.Name}");
                SetVulnerable(5.0f);
            }
            else
            {
                StartCoroutine(MoveVertically(transform, owner.handsHitAttitude, owner.handsFollowAttitude, 1, Mathf.LerpCurve.EaseOut));
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            IncreaseSequenceCounter();
            StartCoroutine(MoveVertically(transform, owner.handsHitAttitude, owner.handsFollowAttitude, 1, Mathf.LerpCurve.EaseOut));
            yield return new WaitForSeconds(1);
        }

        isBusy = false;
        canBeStopped = true;
    }

    IEnumerator Spike()
    {
        Debug.Log($"Starting spike with {entity.Name}");

        isBusy = true;
        StartCoroutine(ReturnToStartPoint());
        yield return new WaitForSeconds(timeToReturnToStartPoint);

        ///Trigger Animations?? Shake???

        yield return new WaitForSeconds(1.5f);
        canBeStopped = false;

        ///Start PArticles Emerge??

        StartCoroutine(MoveVertically(spikesEntity.transform, spikeHideAttitude, spikeShowAttitude, 0.5f, Mathf.LerpCurve.ExponentialOut));
        yield return new WaitForSeconds(0.3f);
        if (HasSpikeHitTarget())
            owner.GetTarget().PlayerHealth.Damage(1);
        yield return new WaitForSeconds(0.2f);


        yield return new WaitForSeconds(1.5f);

        ///Start Particles Hide??
        StartCoroutine(MoveVertically(spikesEntity.transform, spikeShowAttitude, spikeHideAttitude, 1.5f, Mathf.LerpCurve.EaseOut));
        yield return new WaitForSeconds(1.5f);
        ///Stop Particles Hide??

        yield return new WaitForSeconds(2);

        IncreaseSequenceCounter();
        SetCooldown(1);

        isBusy = false;
        canBeStopped = true;
    }

    IEnumerator ReturnToStartPoint()
    {
        Debug.Log($"Returning {entity.Name} to start point");

        Vector3 init = transform.position;
        Vector3 target = startPointEntity.transform.position;

        float timer = 0.0f;
        while (timer<timeToReturnToStartPoint)
        {

            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(init, target, timer / timeToReturnToStartPoint);
            yield return null;
        }
        transform.position = target;
        transform.position = startPointEntity.transform.position;
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

    bool IncreaseSequenceCounter()
    {
        sequence[sequenceIndex].Increment();
        if(sequence[sequenceIndex].IsCompleted())
        {
            sequence[sequenceIndex].Reset();
            IncreaseSequence();
            return true;
        }
        return false;
    }
    void IncreaseSequence()
    {
        int lastIndex = sequenceIndex;
        sequenceIndex++;
        if (sequenceIndex >= sequence.Count)
        {
            sequence[lastIndex].Reset();
            sequenceIndex = 0;
            sequence[sequenceIndex].Reset();
        }
    }

    public void SetCooldown(float cooldown)
    {
        isInCooldown = true;
        cooldownTimer = cooldown;
    }
    public void SetVulnerable(float time)
    {
        isVulnerable = true;
        vulnerableTimer = time;
    }

    void ProcessStateTimer(ref float timer, ref bool state)
    {
        if (!state) 
            return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            state = false;
            timer = 0;
        }
    }

    IEnumerator MoveVertically(Transform target, float start, float end, float duration, Mathf.LerpCurve mode)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float yPos = Mathf.Lerp(start, end, timer / duration, mode);
            target.position = new Vector3(target.position.x, yPos, target.position.z);
            yield return null;
        }
        target.position = new Vector3(target.position.x, end, target.position.z);
    }

}