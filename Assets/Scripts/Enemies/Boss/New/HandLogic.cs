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
    public Entity shadowEntity;

    [Space(10)]
    [Header("Hand Settings")]
    [ShowInInspector] BossSide side;

    [Space(10)]
    [Header("Feedback")]
    public Entity hitFeedbackEntity;
    public float hitFeedbackDuration;
    ParticleComponent hitFeedbackParticles;
    AudioSource hitFeedbackAudio;
    public Entity spikeFeedbackEntity;
    public float spikeFeedbackDuration;
    ParticleComponent spikeFeedbackParticles;
    AudioSource spikeFeedbackAudio;

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
        hitFeedbackAudio = hitFeedbackEntity.GetComponent<AudioSource>();
        spikeFeedbackAudio = spikeFeedbackEntity.GetComponent<AudioSource>();

        transform.position = startPointEntity.transform.position;
        isBusy = false;
    }

    void OnPostCreate() 
    { 

    }

    void OnUpdate()
    {

        shadowEntity.transform.position = new Vector3(transform.position.x, shadowEntity.transform.position.y, transform.position.z);

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
                StartCoroutine(owner.GoToPoint(transform, transform.position, startPointEntity.transform.position, owner.handTimeToReturnToStartPoint));
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
            targetPosition.y = owner.handsFollowAltitude;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                owner.handFollowSpeed * Time.deltaTime
            );

            if(Vector3.Distance(transform.position, targetPosition) < 2f)
            {
                timer+= Time.deltaTime;
                if(timer> owner.handTimeToTriggerPunch)
                    hitPlayer = true;
            }else
            {
                timer -= Time.deltaTime / 2;
            }

            yield return null;
        }
        canBeStopped = false;
        Debug.Log($"Hit player with {entity.Name}");

        StartCoroutine(owner.MoveVertically(transform, owner.handsFollowAltitude, owner.handsFollowAltitude + 2, 0.1f, Mathf.LerpCurve.EaseOut));
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(owner.MoveVertically(transform, owner.handsFollowAltitude + 2, owner.handsHitAltitude, 0.4f, Mathf.LerpCurve.ExponentialInOut));
        yield return new WaitForSeconds(0.3f);

        hitFeedbackEntity.transform.position = new Vector3(transform.position.x, hitFeedbackParticles.transform.position.y, transform.position.z);
        PlayFeedback(hitFeedbackAudio, hitFeedbackParticles, hitFeedbackDuration);

        if (HasHitTarget())
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
                StartCoroutine(owner.MoveVertically(transform, owner.handsHitAltitude, owner.handsFollowAltitude, 1, Mathf.LerpCurve.EaseOut));
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            IncreaseSequenceCounter();
            StartCoroutine(owner.MoveVertically(transform, owner.handsHitAltitude, owner.handsFollowAltitude, 1, Mathf.LerpCurve.EaseOut));
            yield return new WaitForSeconds(1);
        }

        isBusy = false;
        canBeStopped = true;
    }

    IEnumerator Spike()
    {
        Debug.Log($"Starting spike with {entity.Name}");

        isBusy = true;
        StartCoroutine(owner.GoToPoint(transform, transform.position, startPointEntity.transform.position, owner.handTimeToReturnToStartPoint));
        yield return new WaitForSeconds(owner.handTimeToReturnToStartPoint);

        ///Trigger Animations?? Shake???
        yield return new WaitForSeconds(0.75f);
        canBeStopped = false;
        PlayFeedback(spikeFeedbackAudio, spikeFeedbackParticles, spikeFeedbackDuration);
        yield return new WaitForSeconds(0.75f);

        ///Start PArticles Emerge??

        StartCoroutine(owner.MoveVertically(spikesEntity.transform, owner.spikeHideAltitude, owner.spikeShowAltitude, 0.5f, Mathf.LerpCurve.ExponentialOut));
        yield return new WaitForSeconds(0.3f);
        if (HasSpikeHitTarget())
            owner.GetTarget().PlayerHealth.Damage(1);
        
        yield return new WaitForSeconds(0.2f);


        yield return new WaitForSeconds(1.5f);

        ///Start Particles Hide??
        StartCoroutine(owner.MoveVertically(spikesEntity.transform, owner.spikeShowAltitude, owner.spikeHideAltitude, 1.5f, Mathf.LerpCurve.EaseOut));
        yield return new WaitForSeconds(1.5f);
        ///Stop Particles Hide??

        yield return new WaitForSeconds(2);

        IncreaseSequenceCounter();
        SetCooldown(1);

        isBusy = false;
        canBeStopped = true;
    }

    

    public void Regenerate()
    {
        isDefeated = false;
        isVulnerable = false;
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