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
    public Entity spikesEntity;
    public Entity spikesAttackColliderEntity;
    public Entity startPointEntity;
    public Entity shadowEntity;
    BoxCollider attackCollider;
    BoxCollider burnCollider;
    BoxCollider spikesAttackCollider;

    [Space(10)]
    [Header("Settings")]
    [ShowInInspector] BossSide side;


    [Space(10)]
    [Header("Feedback")]
    public Entity hitFeedbackEntity;
    public float hitFeedbackDuration;
    ParticleComponent hitFeedbackParticles;
    AudioSource hitFeedbackAudio;

    public Entity spikeFeedbackEntity;
    ParticleComponent spikeFeedbackParticles;
    AudioSource spikeFeedbackAudio;

    public Entity defeatFeedbackEntity;
    ParticleComponent defeatFeedbackParticles;
    AudioSource defeatFeedbackAudio;

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

        hitFeedbackParticles = hitFeedbackEntity.GetComponent<ParticleComponent>();
        spikeFeedbackParticles = spikeFeedbackEntity.GetComponent<ParticleComponent>();
        defeatFeedbackParticles = defeatFeedbackEntity.GetComponent<ParticleComponent>();
        hitFeedbackAudio = hitFeedbackEntity.GetComponent<AudioSource>();
        spikeFeedbackAudio = spikeFeedbackEntity.GetComponent<AudioSource>();
        defeatFeedbackAudio = defeatFeedbackEntity.GetComponent<AudioSource>();

        transform.position = startPointEntity.transform.position;
        isBusy = false;
    }

    void OnPostCreate() 
    { 

    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        shadowEntity.transform.position = new Vector3(transform.position.x, shadowEntity.transform.position.y, transform.position.z);

        ProcessStateTimer(ref cooldownTimer, ref isInCooldown);
        if(ProcessStateTimer(ref vulnerableTimer, ref isVulnerable))
        {
            StopAllOwnedCoroutines();
            StartCoroutine(owner.GoToPoint(transform, transform.position, startPointEntity.transform.position, owner.H_TimeToReturnToStartPoint));
            isBusy = false;
        }

        if(!isDefeated && isVulnerable && HasBeenHit())
        {
            Debug.Log($"Hand {entity.Name} has been hit while vulnerable");
            isDefeated = true;
            isVulnerable = false;
            canBeStopped = false;

            defeatFeedbackEntity.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            defeatFeedbackAudio.Play();
            defeatFeedbackParticles.Play();

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
                StartCoroutine(owner.GoToPoint(transform, transform.position, startPointEntity.transform.position, owner.H_TimeToReturnToStartPoint));
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
            targetPosition.y = owner.HPunch_MoveAltitude;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                owner.HPunch_FollowSpeed * Time.deltaTime
            );

            if(Vector3.Distance(transform.position, targetPosition) < 2f)
            {
                timer+= Time.deltaTime;
                if(timer> owner.HPunch_FollowTime)
                    hitPlayer = true;
            }else
            {
                timer -= Time.deltaTime * owner.HPunch_FollowTimeReduction;
            }

            yield return null;
        }
        canBeStopped = false;
        Debug.Log($"Hit player with {entity.Name}");

        StartCoroutine(owner.MoveVertically(transform, owner.HPunch_MoveAltitude, owner.HPunch_MoveAltitude + 2, owner.HPunch_TimeGoingUp, Mathf.LerpCurve.EaseOut));
        yield return new WaitForSeconds(owner.HPunch_TimeGoingUp);

        StartCoroutine(owner.MoveVertically(transform, owner.HPunch_MoveAltitude + 2, owner.HPunch_HitAltitude, owner.HPunch_TimeGoingDown, Mathf.LerpCurve.ExponentialInOut));

        timer = 0;
        bool hasDoneFeedback = false;
        bool hasHitTarget = false;
        while (timer < owner.HPunch_TimeGoingDown)
        {
            timer += Time.deltaTime;
            if (!hasHitTarget)
            {
                float percentage = (timer / owner.HPunch_TimeGoingDown) * 100;

                if(percentage > owner.HPunch_HitTimePercentage)
                {
                    if(!hasHitTarget && HasHitTarget())
                    {
                        hasHitTarget = true;
                        owner.GetTarget().PlayerHealth.Damage(owner.HPunch_Damage);
                    }
                    if (!hasDoneFeedback)
                    {
                        hasDoneFeedback = true;
                        hitFeedbackEntity.transform.position = new Vector3(transform.position.x, hitFeedbackParticles.transform.position.y, transform.position.z);
                        PlayFeedback(hitFeedbackAudio, hitFeedbackParticles, hitFeedbackDuration);
                    }
                }
            }
            
            yield return null;
        }

        if (sequenceIndex == sequence.Count-1)
        {
            if (IncreaseSequenceCounter())
            {
                Debug.Log($"Set Vulnerable {entity.Name}");
                SetVulnerable(owner.HPunch_VulnerableTime);
            }
            else
            {
                StartCoroutine(owner.MoveVertically(transform, owner.HPunch_HitAltitude, owner.HPunch_MoveAltitude, 1, Mathf.LerpCurve.EaseOut));
                yield return new WaitForSeconds(owner.HPunch_DelayTimeAfterPunch);
            }
        }
        else
        {
            IncreaseSequenceCounter();
            StartCoroutine(owner.MoveVertically(transform, owner.HPunch_HitAltitude, owner.HPunch_MoveAltitude, 1, Mathf.LerpCurve.EaseOut));
            yield return new WaitForSeconds(owner.HPunch_DelayTimeAfterPunch);
        }

        if (side == owner.GetCurrentSide())
            isBusy = false;

        canBeStopped = true;
    }

    IEnumerator Spike()
    {
        Debug.Log($"Starting spike with {entity.Name}");

        isBusy = true;
        StartCoroutine(owner.GoToPoint(transform, transform.position, startPointEntity.transform.position, owner.H_TimeToReturnToStartPoint));
        yield return new WaitForSeconds(owner.H_TimeToReturnToStartPoint);

        ///Trigger Animations?? Shake???
        yield return new WaitForSeconds(owner.HSpike_InitialDelay);
        canBeStopped = false;
        PlayFeedback(spikeFeedbackAudio, spikeFeedbackParticles, owner.HSpike_AlertTime);
        yield return new WaitForSeconds(owner.HSpike_AlertTime);

        ///Start PArticles Emerge??

        bool hasHitTarget = false;

        StartCoroutine(owner.MoveVertically(spikesEntity.transform, owner.HSpike_HideAltitude, owner.HSpike_ShowAltitude, owner.HSpike_SpikeShowTime, Mathf.LerpCurve.ExponentialOut));
        yield return new WaitForSeconds(owner.HSpike_SpikeShowTime);

        if (!hasHitTarget && HasSpikeHitTarget())
        {
            hasHitTarget = true;
            owner.GetTarget().PlayerHealth.Damage(owner.HSpike_Damage);
        }

        float timer = 0;
        while (timer < owner.HSpike_SpikeStayTime)
        {
            timer+= Time.deltaTime;
            if (!hasHitTarget && HasSpikeHitTarget())
            {
                hasHitTarget = true;
                owner.GetTarget().PlayerHealth.Damage(owner.HSpike_Damage);
            }
        }

        StartCoroutine(owner.MoveVertically(spikesEntity.transform, owner.HSpike_ShowAltitude, owner.HSpike_HideAltitude,owner.HSpike_SpikeHideTime, Mathf.LerpCurve.EaseOut));
        yield return new WaitForSeconds(owner.HSpike_SpikeHideTime);

        IncreaseSequenceCounter();
        SetCooldown(owner.HSpike_DelayTimeAfterSpike);

        isBusy = false;
        canBeStopped = true;
    }

    
    public void FakeRegenerate()
    {
        defeatFeedbackAudio.Stop();
        defeatFeedbackParticles.Stop();
    }

    public void Regenerate()
    {
        FakeRegenerate();
        isDefeated = false;
        isVulnerable = false;
        isBusy = false;
        canBeStopped = true;
        isInCooldown = false;
        StopAllOwnedCoroutines();
        SetCooldown(2);
        Debug.Log($"Hand {entity.Name} has regenerated");

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

    bool ProcessStateTimer(ref float timer, ref bool state)
    {
        if (!state) 
            return false;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            state = false;
            timer = 0;
            return true;
        }
        return false;
    }


    void PlayFeedback(AudioSource audio)
    {
        if (audio != null)
            audio.Play();
    }

    void PlayFeedback(ParticleComponent particle, float duration = 1f)
    {
        if (particle != null)
            StartCoroutine(PlayParticles(particle, duration));
    }

    void PlayFeedback(AudioSource audio, ParticleComponent particle, float duration = 1f)
    {
        if (audio != null)
            PlayFeedback(audio);

        if (particle != null)
        {
            PlayFeedback(particle, duration);
        }
    }

    IEnumerator PlayParticles(ParticleComponent component, float duration)
    {
        component.Play();
        yield return new WaitForSeconds(duration);
        component.Stop();
    }

}