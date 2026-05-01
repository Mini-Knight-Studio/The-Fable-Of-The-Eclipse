using System;
using System.Collections.Generic;
using System.Configuration;
using Loopie;

public class PlayerGrapple : PlayerComponent
{
    // --- Global Pillar Management ---
    private static List<PillarTrigger> availablePillars = new List<PillarTrigger>();
    public static void RegisterPillar(PillarTrigger p) { if (!availablePillars.Contains(p)) availablePillars.Add(p); }
    public static void UnregisterPillar(PillarTrigger p) { availablePillars.Remove(p); }

    [Header("Settings")]
    //public float maxGrappleDistance = 25.0f;
    public float maxGrappleAngle = 60.0f;
    public float grappleCooldownTimer = 0.0f;

    [Header("Rope")]
    public Entity segmentPrefab;
    public Entity hookPrefab;
    public int segmentCount = 10;
    public float ropeSagAmount = 2.0f;

    private Entity hookInstance;
    private List<Entity> ropeSegments = new List<Entity>();

    [Header("Timings")]
    public float currentWaitTime = 0.5f;
    public float grappleDuration = 0.3f;
    public float stoppingDistance = 2.0f;
    public float grappleCooldown = 2.0f;
    public float landingDuration = 0.5f;

    private bool isLaunching = false;
    private bool isTraveling = false;
    private bool isLanding = false;


    private float stateTimer = 0.0f;
    private float landingTimer = 0f;

    private Vector3 startPos = new Vector3(0, 0, 0);
    private Vector3 targetPos = new Vector3(0, 0, 0);
    private Vector3 pillarPos = new Vector3(0, 0, 0);
    private PillarTrigger activePillar;
    private PillarTrigger currentlyLookedAtPillar;

    public bool IsLaunching => isLaunching;
    public bool IsTraveling => isTraveling;
    public bool IsLanding => isLanding;

    public bool IsGrappling => isLaunching || isTraveling || isLanding;

    public void OnCreate()
    {
        ropeSegments.Clear();

        availablePillars.Clear();
    }

    public void ProcessGrappel()
    {
        if (!DatabaseRegistry.playerDB.Player.hasGrappling)
            return;

        if (player.Combat.isAttacking || player.Torch.IsTorching || player.Movement.IsDashing())
            return;

        if (grappleCooldownTimer > 0) grappleCooldownTimer -= Time.deltaTime;

        if (isLanding)
        {
            landingTimer -= Time.deltaTime;
            if (landingTimer <= 0) isLanding = false;
        }

        if (!isLaunching && !isTraveling)
        {
            UpdateTargetPrompt();

            if (Player.Instance.Input.grappleKeyPressed && grappleCooldownTimer <= 0)
            {
                TryExecuteGrapple();
            }
            return;
        }

        stateTimer += Time.deltaTime;

        if (isLaunching)
        {
            float t = stateTimer / currentWaitTime;
            if (t > 1.0f) t = 1.0f;
            UpdateRopeVisuals(t, true);

            if (stateTimer >= currentWaitTime)
            {
                isLaunching = false;
                isTraveling = true;
                stateTimer = 0.0f;
                startPos = transform.position;
                if (activePillar != null) activePillar.PlayHookParticles();
            }
        }
        else if (isTraveling)
        {
            float t = stateTimer / grappleDuration;
            if (t > 1.0f) t = 1.0f;
            UpdateRopeVisuals(1.0f, false);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            if (stateTimer >= grappleDuration)
            {
                DestroyGrappleObjects();
                FinalizeGrapple();
                isLanding = true;
                landingTimer = landingDuration;
            }
        }
    }

    private void TryExecuteGrapple()
    {
        if (!DatabaseRegistry.playerDB.Player.hasGrappling)
            return;

        PillarTrigger bestPillar = null;
        float bestScore = float.MaxValue;

        Vector3 playerPos = transform.position;
        Vector3 playerForward = transform.Forward;

        foreach (var pillar in availablePillars)
        {
            if (IsValidGrappleTarget(pillar, playerPos, playerForward, true, out float score))
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestPillar = pillar;
                }
            }
        }

        if (bestPillar != null)
        {
            RotateToTarget(bestPillar.entity.transform.position);
            ExecuteGrapple(bestPillar, bestPillar.hookTravelTime);
        }
    }

    private void UpdateTargetPrompt()
    {
        PillarTrigger bestLookedAt = null;
        float bestScore = float.MaxValue;

        Vector3 playerPos = transform.position;
        Vector3 playerForward = transform.Forward;

        foreach (var pillar in availablePillars)
        {
            if (IsValidGrappleTarget(pillar, playerPos, playerForward, false, out float score))
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestLookedAt = pillar;
                }
            }
        }

        if (bestLookedAt != null &&
            !bestLookedAt.CheckLineOfSight(playerPos))
        {
            bestLookedAt = null;
        }

        if (currentlyLookedAtPillar != bestLookedAt)
        {
            if (currentlyLookedAtPillar != null)
                currentlyLookedAtPillar.SetTargetedState(false);

            currentlyLookedAtPillar = bestLookedAt;

            if (currentlyLookedAtPillar != null)
                currentlyLookedAtPillar.SetTargetedState(true);
        }
    }

    private bool IsValidGrappleTarget(PillarTrigger pillar, Vector3 playerPos, Vector3 playerForward, bool requireLineOfSight, out float score)
    {
        score = float.MaxValue;

        if (pillar == null || pillar.entity == null)
            return false;

        Vector3 pillarPos = pillar.entity.transform.position;

        float distance = (float)Vector3.Distance(playerPos, pillarPos);
        if (distance > pillar.reachDistance)
            return false;

        Vector3 toPillar = pillarPos - playerPos;
        toPillar.y = 0f;

        Vector3 forwardFlat = playerForward;
        forwardFlat.y = 0f;

        if (toPillar.magnitude < 0.0001f || forwardFlat.magnitude < 0.0001f)
            return false;

        toPillar.Normalize();
        forwardFlat.Normalize();

        float dot = Vector3.Dot(forwardFlat, toPillar);
        dot = Math.Max(-1.0f, Math.Min(1.0f, dot));

        float angle = (float)(Math.Acos(dot) * (180.0 / Math.PI));

        if (angle > maxGrappleAngle)
            return false;

        if (requireLineOfSight && !pillar.CheckLineOfSight(playerPos))
            return false;

        score = (angle * 2.0f) + distance;

        return true;
    }

    public void ExecuteGrapple(PillarTrigger pillarScript, float waitTime)
    {
        DestroyGrappleObjects();
        CreateGrappleObjects();

        player.Feedback.PlayGrapple();

        currentWaitTime = waitTime > 0.01f ? waitTime : 0.5f;
        isLanding = false;
        isLaunching = true;
        isTraveling = false;
        stateTimer = 0.0f;
        activePillar = pillarScript;
        pillarPos = activePillar.entity.transform.position;
        startPos = transform.position;

        Vector3 dir = (pillarPos - startPos).normalized;
        targetPos = pillarPos - (dir * stoppingDistance);
        targetPos.y = transform.position.y;
    }

    private void CreateGrappleObjects()
    {
        if (segmentPrefab == null || hookPrefab == null) 
            return;

        hookInstance = hookPrefab.Clone(true);
        hookInstance.SetActive(true);

        for (int i = 0; i < segmentCount; i++)
        {
            Entity seg = segmentPrefab.Clone(true);
            seg.SetActive(true);
            ropeSegments.Add(seg);
        }
    }

    private void FinalizeGrapple()
    {
        isTraveling = false;
        isLaunching = false;
        stateTimer = 0.0f;
        grappleCooldownTimer = grappleCooldown;

        if (activePillar != null) activePillar.ResetParticles();
        activePillar = null;
    }

    private void DestroyGrappleObjects()
    {
        if (hookInstance != null)
        {
            hookInstance.Destroy();
            hookInstance = null;
        }

        for (int i = 0; i < ropeSegments.Count; i++)
        {
            if (ropeSegments[i] != null)
            {
                ropeSegments[i].Destroy();
            }
        }
        ropeSegments.Clear();
    }

    private void UpdateRopeVisuals(float progress, bool isLaunchingPhase)
    {
        if (player.HookAnchor == null || hookInstance == null) 
            return;

        Vector3 start = player.HookAnchor.transform.position;
        Vector3 end = isLaunchingPhase ? Vector3.Lerp(start, pillarPos, progress) : pillarPos;

        Vector3 midPoint = (start + end) * 0.5f;
        Vector3 manualDown = new Vector3(0.0f, -1.0f, 0.0f);
        Vector3 controlPoint = midPoint + (manualDown * (ropeSagAmount * (1.0f - progress)));

        hookInstance.transform.position = end;

        Vector3 tipDirection = (end - GetBezierPoint(start, controlPoint, end, 0.95f)).normalized;
        Vector3 hookFakeTarget = end + new Vector3(0, 1, 0);
        hookInstance.transform.LookAt(hookFakeTarget, tipDirection);

        for (int i = 0; i < ropeSegments.Count; i++)
        {
            float t1 = (float)i / (float)ropeSegments.Count;
            float t2 = (float)(i + 1) / (float)ropeSegments.Count;

            Vector3 posA = GetBezierPoint(start, controlPoint, end, t1);
            Vector3 posB = GetBezierPoint(start, controlPoint, end, t2);

            Entity seg = ropeSegments[i];
            seg.transform.position = posA;

            Vector3 direction = (posB - posA).normalized;
            Vector3 segFakeTarget = posA + new Vector3(0, 1, 0);
            seg.transform.LookAt(segFakeTarget, direction);

            float dist = (float)Vector3.Distance(posA, posB);
            seg.transform.scale = new Vector3(2, dist * 1.5f, 2);
        }
    }

    private Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float invT = 1.0f - t;
        return (p0 * (invT * invT)) + (p1 * (2.0f * invT * t)) + (p2 * (t * t));
    }

    public void RotateToTarget(Vector3 pPos)
    {
        Vector3 lookAtPos = new Vector3((float)pPos.x, (float)transform.position.y, (float)pPos.z);
        transform.LookAt(lookAtPos, Vector3.Up);
    }
}