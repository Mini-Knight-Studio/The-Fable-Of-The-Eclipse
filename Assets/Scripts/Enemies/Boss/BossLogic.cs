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
    public Entity middleColliderEntity;

    [Header("Environment")]
    public Entity vinesObstructionEntity; 

    [Space(10)]
    [Header("Boss Settings")]
    public int totalPhases = 3;
    [Tooltip("Time before a defeated hand regenerates if the other is ignored")]
    public float antiCampRegenTime = 20.0f;


    [Space(10)]
    [Header("Hand Settings")]
    public float H_TimeToReturnToStartPoint;
    public int HPunch_Damage;
    public float HPunch_FollowSpeed;
    public float HPunch_MoveAltitude;
    public float HPunch_HitAltitude;
    public float HPunch_FollowTime;
    public float HPunch_FollowTimeReduction;
    public float HPunch_TimeGoingUp;
    public float HPunch_TimeGoingDown;
    public float HPunch_HitTimePercentage;
    public float HPunch_DelayTimeAfterPunch;
    public float HPunch_VulnerableTime;

    public int HSpike_Damage;
    public float HSpike_HideAltitude;
    public float HSpike_ShowAltitude;
    public float HSpike_InitialDelay;
    public float HSpike_AlertTime;
    public float HSpike_SpikeShowTime;
    public float HSpike_SpikeStayTime;
    public float HSpike_SpikeHideTime;
    public float HSpike_DelayTimeAfterSpike;

    public float Head_TimeToReturnToStartPoint;
    public float Head_TimeToVulnerablePosition;
    public float Head_VulnerableAltitude;
    public Vector3 Head_VulnerableRotation;

    public float Core_VulnerabilityDuration = 5.0f;
    public float Core_RegenerationDuration = 5.0f;

    [ReadOnly][ShowInInspector] BossSide currentSide;
    [ReadOnly][ShowInInspector] int currentPhase;

    [ReadOnly][ShowInInspector] bool isDefeated;
    [ReadOnly][ShowInInspector] bool isVulnerable;
    [ReadOnly][ShowInInspector] bool isBusy;


    float leftHandDefeatTimer = 0f;
    float rightHandDefeatTimer = 0f;

    Player target;

    public bool IsDefeated() => isDefeated;
    public bool IsVulnerable() => isVulnerable;
    public BossSide GetCurrentSide() => currentSide;
    public Player GetTarget() => target;
    public int GetCurrentPhase() => currentPhase;
    bool IsFinalPhase() => currentPhase >= totalPhases - 1;

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
        if (vinesObstructionEntity != null) vinesObstructionEntity.SetActive(false);

        Player.Instance.LoseScreen.OnClosing += RestartBoss;

        UpdatePhaseSequences();
    }

    void UpdatePhaseSequences()
    {
        List<SequenceAction> phaseSequence = new List<SequenceAction>();

        if (currentPhase == 0)
        {
            // Phase 1: 1 Punch -> 1 Palm (Stuck)
            phaseSequence.Add(new SequenceAction(AttackType.Punch, 1));
            phaseSequence.Add(new SequenceAction(AttackType.Palm, 1));
        }
        else if (currentPhase == 1)
        {
            // Phase 2: 2 Punches -> Spike -> Palm (Stuck)
            phaseSequence.Add(new SequenceAction(AttackType.Punch, 2));
            phaseSequence.Add(new SequenceAction(AttackType.Spike, 1));
            phaseSequence.Add(new SequenceAction(AttackType.Palm, 1));
        }
        else
        {
            // Phase 3: 3 Punches -> Spike -> Palm (Stuck) + Arena Obstruction
            phaseSequence.Add(new SequenceAction(AttackType.Punch, 3));
            phaseSequence.Add(new SequenceAction(AttackType.Spike, 1));
            phaseSequence.Add(new SequenceAction(AttackType.Palm, 1));

            if (vinesObstructionEntity != null) vinesObstructionEntity.SetActive(true);
        }

        leftHand.SetSequence(new List<SequenceAction>(phaseSequence));
        rightHand.SetSequence(new List<SequenceAction>(phaseSequence));
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        UpdateCurrentSide();
        ManageMiddleCollider();
        if (isDefeated || isVulnerable) return;

        HandleRegenerationTimers();

        if (!isBusy)
        {
            if (leftHand.IsDefeated() && rightHand.IsDefeated())
            {
                StartCoroutine(ExposeCore());
            }
        }
    }

    void ManageMiddleCollider()
    {
        if (!Player.Instance.Grapple.IsGrappling)
        {
            middleColliderEntity.SetActive(true);
        }
        else
        {
            middleColliderEntity.SetActive(false);
        }
    }

    void HandleRegenerationTimers()
    {
        if (leftHand.IsDefeated() && !rightHand.IsDefeated() && GetCurrentSide() == BossSide.Left)
        {
            leftHandDefeatTimer += Time.deltaTime;
            if (leftHandDefeatTimer >= antiCampRegenTime)
            {
                Debug.Log("Anti-camp triggered: Left hand regenerating.");
                leftHand.Regenerate();
                leftHandDefeatTimer = 0f;
            }
        }
        else
        {
            leftHandDefeatTimer = 0f;
        }

        if (rightHand.IsDefeated() && !leftHand.IsDefeated() && GetCurrentSide() == BossSide.Right)
        {
            rightHandDefeatTimer += Time.deltaTime;
            if (rightHandDefeatTimer >= antiCampRegenTime)
            {
                Debug.Log("Anti-camp triggered: Right hand regenerating.");
                rightHand.Regenerate();
                rightHandDefeatTimer = 0f;
            }
        }
        else
        {
            rightHandDefeatTimer = 0f;
        }
    }

    IEnumerator ExposeCore()
    {
        Debug.Log("Core Exposed");
        isVulnerable = true;
        isBusy = true;

        StartCoroutine(MoveVertically(head.transform, head.startPointEntity.transform.position.y, Head_VulnerableAltitude, Head_TimeToVulnerablePosition, Mathf.LerpCurve.ExponentialOut));
        StartCoroutine(RotateInAxis(head.transform, new Vector3(0, 0, 0), Head_VulnerableRotation, Head_TimeToVulnerablePosition, new Vector3(0, 0, 1)));
        yield return new WaitForSeconds(Head_TimeToVulnerablePosition + 0.5f);

        head.headColliderEntity.SetActive(true);
        float timer = 0;
        while (timer <= Core_VulnerabilityDuration)
        {
            timer += Time.deltaTime;
            if (head.HasBeenHit())
            {
                head.headColliderEntity.SetActive(false);
                isDefeated = true;
                isVulnerable = false;

                if (IsFinalPhase())
                {
                    yield return new WaitForSeconds(2);
                    Player.Instance.CreditsScreen.OpenCreditsScreen();
                    yield break;
                }
                else
                {
                    currentPhase++;
                    UpdatePhaseSequences();

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
        Vector3 targetRotation = new Vector3(axis.x == 0 ? start.x : end.x * axis.x, axis.y == 0 ? start.y : end.y * axis.y, axis.z == 0 ? start.z : end.z * axis.z);

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

    public void RestartBoss()
    {
        StopAllOwnedCoroutines();

        currentPhase = 0;
        isDefeated = false;
        isVulnerable = false;
        isBusy = false;
        leftHandDefeatTimer = 0f;
        rightHandDefeatTimer = 0f;

        UpdatePhaseSequences();

        if (head != null) head.Restart();
        if (leftHand != null) leftHand.Restart();
        if (rightHand != null) rightHand.Restart();

        if (leftHand != null) leftHand.SetCooldown(2f);
        if (rightHand != null) rightHand.SetCooldown(2f);

        if (head != null && head.headColliderEntity != null) head.headColliderEntity.SetActive(false);
        if (vinesObstructionEntity != null) vinesObstructionEntity.SetActive(false);
    }

    void OnDestroy()
    {
        Player.Instance.LoseScreen.OnClosing -= RestartBoss;
        StopAllOwnedCoroutines();
    }
}