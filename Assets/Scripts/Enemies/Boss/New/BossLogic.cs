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
    public float handFollowSpeed;
    public float handTimeToReturnToStartPoint;
    public float handTimeToTriggerPunch;
    public float handsFollowAltitude = 9.0f;
    public float handsHitAltitude = 0.0f;

    [Space(10)]
    [Header("Spikes Settings")]
    public float spikeHideAltitude;
    public float spikeShowAltitude;

    [Space(10)]
    [Header("Head Settings")]
    public float headTimeToReturnToStartPoint;
    public float headLookAtSpeed;
    public float headVulnerableAttitude;
    public Vector3 headVulnerableRotation;

    [Space(10)]
    [Header("Phase Transition")]
    public float coreVulnerabilityDuration = 5.0f;
    public float coreRegenerationDuration = 5.0f;

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
        if (isDefeated || isVulnerable)
            return;

        UpdateCurrentSide();

        if (!isBusy)
        {
            if(leftHand.IsDefeated() && rightHand.IsDefeated())
            {
                /// Core Exposed

                Debug.Log("test");
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

        StartCoroutine(MoveVertically(head.transform, head.startPointEntity.transform.position.y, headVulnerableAttitude, 1.5f, Mathf.LerpCurve.ExponentialOut));
        StartCoroutine(RotateInAxis(head.transform, new Vector3(0,0,0), headVulnerableRotation, 1.5f, new Vector3(0,0,1)));
        yield return new WaitForSeconds(2);

        head.headColliderEntity.SetActive(true);
        float timer = 0;
        while (timer <= coreVulnerabilityDuration)
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
                    StartCoroutine(GoToPoint(leftHand.transform, leftHand.transform.position, leftHand.startPointEntity.transform.position, handTimeToReturnToStartPoint));
                    leftHand.FakeRegenerate();
                    StartCoroutine(GoToPoint(rightHand.transform, rightHand.transform.position, rightHand.startPointEntity.transform.position, handTimeToReturnToStartPoint));
                    rightHand.FakeRegenerate();

                    StartCoroutine(RotateInAxis(head.transform, headVulnerableRotation, new Vector3(0, 0, 0), 1.5f, new Vector3(0, 0, 1)));
                    StartCoroutine(GoToPoint(head.transform, head.transform.position, head.startPointEntity.transform.position, headTimeToReturnToStartPoint));

                    yield return new WaitForSeconds(coreRegenerationDuration);

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
        StartCoroutine(GoToPoint(leftHand.transform, leftHand.transform.position, leftHand.startPointEntity.transform.position, handTimeToReturnToStartPoint));
        leftHand.FakeRegenerate();
        StartCoroutine(GoToPoint(rightHand.transform, rightHand.transform.position, rightHand.startPointEntity.transform.position, handTimeToReturnToStartPoint));
        rightHand.FakeRegenerate();

        StartCoroutine(RotateInAxis(head.transform, headVulnerableRotation, new Vector3(0, 0, 0), 1.5f, new Vector3(0, 0, 1)));
        StartCoroutine(GoToPoint(head.transform, head.transform.position, head.startPointEntity.transform.position, headTimeToReturnToStartPoint));

        yield return new WaitForSeconds(coreRegenerationDuration);

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