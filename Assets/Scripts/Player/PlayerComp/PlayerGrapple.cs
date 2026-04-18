using System;
using System.Collections.Generic;
using Loopie;

public class PlayerGrapple : Component
{
    private Player player;

    public Entity segmentPrefab;
    public int segmentCount = 10;
    public float ropeSagAmount = 2.0f;

    private List<Entity> ropeSegments = new List<Entity>();
    public float currentWaitTime = 0.5f;
    public float grappleDuration = 0.3f;
    public float stoppingDistance = 2.0f;
    public float grappleCooldown = 2.0f;

    private bool isLaunching = false;
    private bool isGrappling = false;
    public float grappleCooldownTimer = 0.0f;

    private Vector3 startPos = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 targetPos = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 pillarPos = new Vector3(0.0f, 0.0f, 0.0f);
    private PillarTrigger activePillar;

    public void OnCreate()
    {
        player = entity.GetComponent<Player>();

        if (segmentPrefab != null)
        {
            for (int i = 0; i < segmentCount; i++)
            {
                Entity seg = segmentPrefab.Clone(false);
                seg.SetActive(false);
                ropeSegments.Add(seg);
            }
        }
    }

    public void RotateToTarget(Vector3 pPos)
    {
        Vector3 lookAtPos = new Vector3((float)pPos.x, (float)transform.position.y, (float)pPos.z);
        transform.LookAt(lookAtPos, Vector3.Up);
    }

    public void ExecuteGrapple(PillarTrigger pillarScript, float waitTime)
    {
        if (pillarScript == null) return;

        currentWaitTime = waitTime > 0.01f ? waitTime : 0.5f;
        isLaunching = true;
        isGrappling = false;
        grappleCooldownTimer = 0.0f;

        activePillar = pillarScript;
        pillarPos = activePillar.entity.transform.position;
        startPos = transform.position;

        Vector3 dir = (pillarPos - startPos).normalized;
        targetPos = pillarPos - (dir * stoppingDistance);
        targetPos.y = transform.position.y;

        for (int i = 0; i < ropeSegments.Count; i++)
        {
            if (ropeSegments[i] != null) ropeSegments[i].SetActive(true);
        }
    }

    public void OnUpdate()
    {
        if (!isLaunching && !isGrappling) return;

        grappleCooldownTimer += Time.deltaTime;

        if (isLaunching)
        {
            float t = grappleCooldownTimer / currentWaitTime;
            if (t > 1.0f) t = 1.0f;

            UpdateRopeVisuals(t, true);

            if (grappleCooldownTimer >= currentWaitTime)
            {
                isLaunching = false;
                isGrappling = true;
                grappleCooldownTimer = 0.0f;
                startPos = transform.position;
            }
        }
        else if (isGrappling)
        {
            float t = grappleCooldownTimer / grappleDuration;
            if (t > 1.0f) t = 1.0f;

            UpdateRopeVisuals(1.0f, false);

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            if (grappleCooldownTimer >= grappleDuration) FinalizeGrapple();
        }
    }

    private void UpdateRopeVisuals(float progress, bool isLaunchingPhase)
    {
        if (player == null || player.HookAnchor == null || ropeSegments.Count == 0) return;

        Vector3 start = player.HookAnchor.transform.position;
        Vector3 end = isLaunchingPhase ? Vector3.Lerp(start, pillarPos, progress) : pillarPos;

        Vector3 midPoint = (start + end) * 0.5f;
        Vector3 manualDown = new Vector3(0.0f, -1.0f, 0.0f);
        float currentSag = ropeSagAmount * (1.0f - progress);
        Vector3 controlPoint = midPoint + (manualDown * currentSag);

        for (int i = 0; i < ropeSegments.Count; i++)
        {
            float t1 = (float)i / (float)(ropeSegments.Count);
            float t2 = (float)(i + 1) / (float)(ropeSegments.Count);

            Vector3 posA = GetBezierPoint(start, controlPoint, end, t1);
            Vector3 posB = GetBezierPoint(start, controlPoint, end, t2);

            Entity seg = ropeSegments[i];
            if (seg == null) continue;

            seg.transform.position = posA;
            seg.transform.LookAt(posB, Vector3.Up);

            float dist = (float)Vector3.Distance(posA, posB);
            seg.transform.scale = new Vector3(0.15f, 0.15f, dist);
        }
    }

    private Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float invT = 1.0f - t;
        Vector3 term1 = p0 * (invT * invT);
        Vector3 term2 = p1 * (2.0f * invT * t);
        Vector3 term3 = p2 * (t * t);
        return term1 + term2 + term3;
    }

    private void FinalizeGrapple()
    {
        isGrappling = false;
        isLaunching = false;

        for (int i = 0; i < ropeSegments.Count; i++)
        {
            if (ropeSegments[i] != null) ropeSegments[i].SetActive(false);
        }

        if (activePillar != null) activePillar.ResetParticles();

        activePillar = null;
        pillarPos = new Vector3(0.0f, 0.0f, 0.0f);
    }
};