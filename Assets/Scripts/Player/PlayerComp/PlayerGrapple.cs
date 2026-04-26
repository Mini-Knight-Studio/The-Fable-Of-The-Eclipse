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
                DestroyGrappleObjects();
                FinalizeGrapple();
                isLanding = true;
                landingTimer = landingDuration;
            }
        }
    }

    public void ExecuteGrapple(PillarTrigger pillarScript, float waitTime)
    {
        if (pillarScript == null || grappleCooldownTimer > 0) return;

        DestroyGrappleObjects();
        CreateGrappleObjects();

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
    }

    private void CreateGrappleObjects()
    {
        if (segmentPrefab == null || hookPrefab == null) return;

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
        isGrappling = false;
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
        if (player.HookAnchor == null || hookInstance == null) return;

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