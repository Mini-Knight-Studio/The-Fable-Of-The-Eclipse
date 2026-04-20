using System;
using System.Collections.Generic;
using Loopie;

public class PlayerGrapple : PlayerComponent
{
    public Entity segmentPrefab;
    public Entity hookPrefab;
    public int segmentCount = 10;
    public float ropeSagAmount = 2.0f;

    private Entity hookInstance;
    private List<Entity> ropeSegments = new List<Entity>();

    public float currentWaitTime = 0.5f;
    public float grappleDuration = 0.3f;
    public float stoppingDistance = 2.0f;
    public float grappleCooldown = 2.0f;
    public float landingDuration = 0.5f;

    private bool isLaunching = false;
    private bool isGrappling = false;
    private bool isLanding = false;

    // Made public again for SkillsHUD.cs
    public float grappleCooldownTimer = 0.0f;

    private float stateTimer = 0.0f;
    private float landingTimer = 0f;

    private Vector3 startPos = new Vector3(0, 0, 0);
    private Vector3 targetPos = new Vector3(0, 0, 0);
    private Vector3 pillarPos = new Vector3(0, 0, 0);
    private PillarTrigger activePillar;

    public bool IsLaunching => isLaunching;
    public bool IsGrappling => isGrappling;
    public bool IsLanding => isLanding;

    public void OnCreate()
    {
        ropeSegments.Clear();

        if (segmentPrefab == null || hookPrefab == null) return;

        // 1. Clone the hook. 
        // If Clone(false) only clones the parent, we ensure it's active 
        // so the engine processes its children.
        hookInstance = hookPrefab.Clone(false);
        hookInstance.SetActive(false);

        // 2. Spawn segments
        for (int i = 0; i < segmentCount; i++)
        {
            Entity seg = segmentPrefab.Clone(false);
            seg.SetActive(false);
            ropeSegments.Add(seg);
        }
    }

    // Restored for PilarTrigger.cs
    public void RotateToTarget(Vector3 pPos)
    {
        Vector3 lookAtPos = new Vector3((float)pPos.x, (float)transform.position.y, (float)pPos.z);
        transform.LookAt(lookAtPos, Vector3.Up);
    }

    public void OnUpdate()
    {
        if (grappleCooldownTimer > 0) grappleCooldownTimer -= Time.deltaTime;
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;
            if (landingTimer <= 0) isLanding = false;
        }

        if (!isLaunching && !isGrappling) return;

        stateTimer += Time.deltaTime;

        if (isLaunching)
        {
            float t = stateTimer / currentWaitTime;
            if (t > 1.0f) t = 1.0f;
            UpdateRopeVisuals(t, true);

            if (stateTimer >= currentWaitTime)
            {
                isLaunching = false;
                isGrappling = true;
                stateTimer = 0.0f;
                startPos = transform.position;
            }
        }
        else if (isGrappling)
        {
            float t = stateTimer / grappleDuration;
            if (t > 1.0f) t = 1.0f;
            UpdateRopeVisuals(1.0f, false);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            if (stateTimer >= grappleDuration)
            {
                FinalizeGrapple();
                isLanding = true;
                landingTimer = landingDuration;
            }
        }
    }

    private void UpdateRopeVisuals(float progress, bool isLaunchingPhase)
    {
        if (player.HookAnchor == null || hookInstance == null) return;

        Vector3 start = player.HookAnchor.transform.position;
        Vector3 end = isLaunchingPhase ? Vector3.Lerp(start, pillarPos, progress) : pillarPos;

        Vector3 midPoint = (start + end) * 0.5f;
        Vector3 manualDown = new Vector3(0.0f, -1.0f, 0.0f);
        Vector3 controlPoint = midPoint + (manualDown * (ropeSagAmount * (1.0f - progress)));

        // --- 1. Position Hook Tip (FIXED) ---
        hookInstance.transform.position = end;

        // Calculate the direction the tip should face
        Vector3 tipDirection = (end - GetBezierPoint(start, controlPoint, end, 0.95f)).normalized;

        // THE TRICK: Use the same Maya Y-Up compensation we used for the segments
        // We point the Z-axis at a 'fake' point above, forcing the Maya Y-axis 
        // (the length of the hook) to align with tipDirection.
        Vector3 hookFakeTarget = end + new Vector3(0, 1, 0);
        hookInstance.transform.LookAt(hookFakeTarget, tipDirection);

        // --- 2. Position Chain (Already fixed) ---
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
            seg.transform.scale = new Vector3(2, dist, 2);
        }
    }

    public void ExecuteGrapple(PillarTrigger pillarScript, float waitTime)
    {
        if (pillarScript == null || grappleCooldownTimer > 0) return;

        currentWaitTime = waitTime > 0.01f ? waitTime : 0.5f;
        isLanding = false;
        isLaunching = true;
        isGrappling = false;
        stateTimer = 0.0f;
        activePillar = pillarScript;
        pillarPos = activePillar.entity.transform.position;
        startPos = transform.position;

        Vector3 dir = (pillarPos - startPos).normalized;
        targetPos = pillarPos - (dir * stoppingDistance);
        targetPos.y = transform.position.y;

        if (hookInstance != null)
        {
            hookInstance.SetActive(true);
            hookInstance.transform.scale = new Vector3(1, 1, 1);
        }
        for (int i = 0; i < ropeSegments.Count; i++) ropeSegments[i].SetActive(true);
    }
    private void SetEntityHierarchyActive(Entity ent, bool active)
    {
        if (ent == null) return;
        ent.SetActive(active);

    }
    private void FinalizeGrapple()
    {
        isGrappling = false;
        isLaunching = false;
        stateTimer = 0.0f;
        grappleCooldownTimer = grappleCooldown;

        hookInstance.SetActive(false);
        for (int i = 0; i < ropeSegments.Count; i++) ropeSegments[i].SetActive(false);

        if (activePillar != null) activePillar.ResetParticles();
        activePillar = null;
    }

    private Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float invT = 1.0f - t;
        return (p0 * (invT * invT)) + (p1 * (2.0f * invT * t)) + (p2 * (t * t));
    }
}