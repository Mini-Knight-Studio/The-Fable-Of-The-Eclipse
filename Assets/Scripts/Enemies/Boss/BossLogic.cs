using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

public enum BossSide
{
    Left, Right
}

public class BossLogic : Component
{
    [Header("References")]
    public Entity leftHandEntity;
    HandLogic leftHand;
    public Entity rightHandEntity;
    HandLogic rightHand;
    public Entity headEntity;
    HeadLogic head;
    public Entity sidePivotEntity;

    [Space(10)]
    [Header("Boss Settings")]
    public int totalPhases = 3;


    [Space(10)]
    [Header("Hand Settings")]
    [Tooltip("Time it takes for the hand to return to its start point")]public float H_TimeToReturnToStartPoint;

    [Space(10)]
    [Header("Hand Settings (PUNCH)")]
    [Tooltip("Damage that the hand will deal")]public int HPunch_Damage;
    [Tooltip("Speed that the hand will have following the target")]public float HPunch_FollowSpeed;
    [Tooltip("Altitude that the hand will move at")] public float HPunch_MoveAltitude;
    [Tooltip("Altitude that the hand will move to when hitting the target")]public float HPunch_HitAltitude;
    [Tooltip("How much time must the hand be at the top of the target")]public float HPunch_FollowTime;
    [Tooltip("Time reduction multiplier when hand is not at the top")]public float HPunch_FollowTimeReduction;
    [Tooltip("The time in which the hand will go up charging")]public float HPunch_TimeGoingUp;
    [Tooltip("The time the hand will take until hitting the ground")]public float HPunch_TimeGoingDown;
    [Tooltip("The % of the hitting motion completed before the hand registers hit (0-100)")] public float HPunch_HitTimePercentage;
    [Tooltip("The time that the hand is resting after a punch")] public float HPunch_DelayTimeAfterPunch;
    [Tooltip("The time that the hand is vulnerable after last punch attack")] public float HPunch_VulnerableTime;

    [Space(10)]
    [Header("Hand Settings (SPIKES)")]
    [Tooltip("Damage that the hand will deal")] public int HSpike_Damage;
    [Tooltip("Altitude that the spikes will hide at")]public float HSpike_HideAltitude;
    [Tooltip("Altitude that the spikes will show at")]public float HSpike_ShowAltitude;
    [Tooltip("The delay time after starting the attack")] public float HSpike_InitialDelay;
    [Tooltip("The alert time before the spikes attack")] public float HSpike_AlertTime;
    [Tooltip("The time that the spikes takes to emerge")] public float HSpike_SpikeShowTime;
    [Tooltip("The time that the spikes stay up")] public float HSpike_SpikeStayTime;
    [Tooltip("The time that the spikes takes to hide")] public float HSpike_SpikeHideTime;
    [Tooltip("The time that the hand is resting after a spike attack")] public float HSpike_DelayTimeAfterSpike;



    [Space(10)]
    [Header("Head Settings")]
    [Tooltip("Time it takes for the head to return to its start point")]public float Head_TimeToReturnToStartPoint;
    [Tooltip("Time it takes for the head to move to vulnerable position")]public float Head_TimeToVulnerablePosition;
    [Tooltip("Altitude that the head will move to when vulnerable")]public float Head_VulnerableAltitude;
    [Tooltip("Rotation that the head will have when vulnerable")]public Vector3 Head_VulnerableRotation;

    [Space(10)]
    [Header("Phase Transition")]
    [Tooltip("Time it takes to recover from vulnerability")]public float Core_VulnerabilityDuration = 5.0f;
    [Tooltip("Time it takes to change phase when recovered")]public float Core_RegenerationDuration = 5.0f;

    [ReadOnly][ShowInInspector] BossSide currentSide;
    [ReadOnly][ShowInInspector] int currentPhase;

    [ReadOnly][ShowInInspector] bool isDefeated;
    [ReadOnly][ShowInInspector] bool isVulnerable;
    [ReadOnly][ShowInInspector] bool isBusy;

    Player target;

    public bool IsDefeated() => isDefeated;
    public bool IsVulnerable() => isVulnerable;
    public BossSide GetCurrentSide() => currentSide;
    public Player GetTarget() => target;
    bool IsFinalPhase() => currentPhase >= totalPhases;

    void OnCreate()
    {
        leftHand = leftHandEntity.GetComponent<HandLogic>();
        rightHand = rightHandEntity.GetComponent<HandLogic>();
        head = headEntity.GetComponent<HeadLogic>();

        currentSide = BossSide.Left;
        currentPhase = 0;
        isBusy = false;
        isDefeated = false;
        isVulnerable = false;
    }

    void OnPostCreate()
    {
        target = Player.Instance;

        leftHand.SetOwner(this);
        leftHand.SetCooldown(2);
        rightHand.SetOwner(this);
        rightHand.SetCooldown(2);
        head.SetOwner(this);

        head.headColliderEntity.SetActive(false);
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        UpdateCurrentSide();
        if (isDefeated || isVulnerable)
            return;

        if (!isBusy)
        {
            if(leftHand.IsDefeated() && rightHand.IsDefeated())
            {
                /// Core Exposed

                StartCoroutine(ExposeCore());
            }                
        }
    }

    IEnumerator ExposeCore()
    {
        //// ZOOM TO HEAD
        Debug.Log("Core Exposed");
        isVulnerable = true;
        isBusy = true;

        StartCoroutine(MoveVertically(head.transform, head.startPointEntity.transform.position.y, Head_VulnerableAltitude, Head_TimeToVulnerablePosition, Mathf.LerpCurve.ExponentialOut));
        StartCoroutine(RotateInAxis(head.transform, new Vector3(0,0,0), Head_VulnerableRotation, Head_TimeToVulnerablePosition, new Vector3(0,0,1)));
        yield return new WaitForSeconds(Head_TimeToVulnerablePosition+0.5f);

        head.headColliderEntity.SetActive(true);
        float timer = 0;
        while (timer <= Core_VulnerabilityDuration)
        {
            timer+=Time.deltaTime;
            if(head.HasBeenHit())
            {
                head.headColliderEntity.SetActive(false);
                isDefeated = true;
                isVulnerable = false;

                currentPhase++;
                if (IsFinalPhase())
                {
                    /// DIE
                    yield break;
                }
                else
                {
                    /// REGENERATE CORE
                    
                    head.headColliderEntity.SetActive(false);
                    StartCoroutine(GoToPoint(leftHand.transform, leftHand.transform.position, leftHand.startPointEntity.transform.position, H_TimeToReturnToStartPoint));
                    leftHand.FakeRegenerate();
                    StartCoroutine(GoToPoint(rightHand.transform, rightHand.transform.position, rightHand.startPointEntity.transform.position, H_TimeToReturnToStartPoint));
                    rightHand.FakeRegenerate();

                    StartCoroutine(RotateInAxis(head.transform, Head_VulnerableRotation, new Vector3(0, 0, 0), Head_TimeToReturnToStartPoint, new Vector3(0, 0, 1)));
                    StartCoroutine(GoToPoint(head.transform, head.transform.position, head.startPointEntity.transform.position, Head_TimeToReturnToStartPoint));

                    yield return new WaitForSeconds(Core_RegenerationDuration);

                    head.Regenerate();
                    leftHand.Regenerate();
                    rightHand.Regenerate();
                    isDefeated = false;
                    yield break;
                }
            }
            yield return null;
        }

        head.headColliderEntity.SetActive(false);
        StartCoroutine(GoToPoint(leftHand.transform, leftHand.transform.position, leftHand.startPointEntity.transform.position, H_TimeToReturnToStartPoint));
        leftHand.FakeRegenerate();
        StartCoroutine(GoToPoint(rightHand.transform, rightHand.transform.position, rightHand.startPointEntity.transform.position, H_TimeToReturnToStartPoint));
        rightHand.FakeRegenerate();

        StartCoroutine(RotateInAxis(head.transform, Head_VulnerableRotation, new Vector3(0, 0, 0), Head_TimeToReturnToStartPoint, new Vector3(0, 0, 1)));
        StartCoroutine(GoToPoint(head.transform, head.transform.position, head.startPointEntity.transform.position, Head_TimeToReturnToStartPoint));

        yield return new WaitForSeconds(Core_RegenerationDuration);

        head.Regenerate();
        leftHand.Regenerate();
        rightHand.Regenerate();
        isDefeated = false;
        isBusy = false;

        /// RESET OR DIE && REMOVE ZOOM
    }

    void UpdateCurrentSide()
    {
        if (target.transform.position.z < sidePivotEntity.transform.position.z)
            currentSide = BossSide.Right;
        else
            currentSide = BossSide.Left;
    }

    public IEnumerator MoveVertically(Transform target, float start, float end, float duration, Mathf.LerpCurve mode)
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
    public IEnumerator RotateInAxis(Transform target, Vector3 start, Vector3 end, float duration, Vector3 axis)
    {
        float timer = 0.0f;
        
        Vector3 targetRotation =  new Vector3(axis.x == 0 ? start.x : end.x*axis.x, axis.y == 0 ? start.y : end.y*axis.y, axis.z == 0 ? start.z : end.z*axis.z);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            target.rotation = Vector3.Lerp(start, targetRotation, timer / duration);

            yield return null;
        }
        target.rotation = targetRotation;
    }

    public IEnumerator GoToPoint(Transform target, Vector3 start, Vector3 end, float duration)
    {
        Debug.Log($"Returning {target.entity.Name} to start point");

        Vector3 init = start;
        Vector3 targetPosition = end;

        float timer = 0.0f;
        while (timer < duration)
        {

            timer += Time.deltaTime;
            target.position = Vector3.Lerp(init, targetPosition, timer / duration);
            yield return null;
        }
        target.position = targetPosition;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}