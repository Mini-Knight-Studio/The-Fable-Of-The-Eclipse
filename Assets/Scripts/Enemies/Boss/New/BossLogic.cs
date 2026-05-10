using System;
using System.Collections;
using System.Collections.Generic;
using Loopie;

public enum BossState
{
    Dead, Alive, Vulnerable
}

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
    [Header("Settings")]
    public int totalPhases = 3;
    public float handsFollowAttitude = 9.0f;
    public float handsHitAttitude = 0.0f;

    [Space(10)]
    [Header("Phase Transition")]
    public float coreRegenerationDuration = 5.0f;

    [ReadOnly][ShowInInspector] BossState currentState;
    [ReadOnly][ShowInInspector] BossSide currentSide;
    [ReadOnly][ShowInInspector] int currentPhase;
    [ReadOnly][ShowInInspector] bool isBusy;

    Player target;


    public BossSide GetCurrentSide() => currentSide;
    public Player GetTarget() => target;
    bool IsFinalPhase() => currentPhase >= totalPhases;

    void OnCreate()
    {
        leftHand = leftHandEntity.GetComponent<HandLogic>();
        rightHand = rightHandEntity.GetComponent<HandLogic>();
        head = headEntity.GetComponent<HeadLogic>();

        currentState = BossState.Alive;
        currentSide = BossSide.Left;
        currentPhase = 1;
        isBusy = false;
    }

    void OnPostCreate()
    {
        target = Player.Instance;

        leftHand.SetOwner(this);
        leftHand.SetCooldown(2);
        rightHand.SetOwner(this);
        rightHand.SetCooldown(2);
        head.SetOwner(this);
    }

    void OnUpdate()
    {
        if (currentState == BossState.Dead)
            return;

        UpdateCurrentSide();

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
        yield return null;

        /// RESET OR DIE && REMOVE ZOOM
    }

    void UpdateCurrentSide()
    {
        if (target.transform.position.z < sidePivotEntity.transform.position.z)
            currentSide = BossSide.Right;
        else
            currentSide = BossSide.Left;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}