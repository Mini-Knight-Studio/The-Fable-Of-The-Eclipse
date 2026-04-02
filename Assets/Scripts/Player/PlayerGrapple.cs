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
    private float currentWaitTime = 0.5f;
    public float grappleDuration = 0.3f;
    public float stoppingDistance = 2.0f;

    private bool isLaunching = false;
    private bool isGrappling = false;
    private float timer = 0.0f;

    private Vector3 startPos = new Vector3(0.0f, 0.0f, 0.0f);
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
        currentWaitTime = waitTime > 0.01f ? waitTime : 0.5f;
        isLaunching = true;
        isGrappling = false;
        timer = 0.0f;

        startPos = transform.position;
        activePillar = pillarScript;
        pillarPos = pillarScript.entity.transform.position;

        foreach (var seg in ropeSegments) seg.SetActive(true);
    }

    public void OnUpdate()
    {
        if (!isLaunching && !isGrappling) return;

        timer += Time.deltaTime;

        if (isLaunching)
        {
            float t = timer / currentWaitTime;
            if (t > 1.0f) t = 1.0f;

            UpdateRopeVisuals(t, true);

            if (timer >= currentWaitTime)
            {
                isLaunching = false;
                isGrappling = true;
                timer = 0.0f;
                startPos = transform.position;
            }
        }
        else if (isGrappling)
        {
            float t = timer / grappleDuration;
            if (t > 1.0f) t = 1.0f;

            UpdateRopeVisuals(1.0f, false);

            Vector3 dir = (pillarPos - startPos).normalized;
            Vector3 targetPos = pillarPos - (dir * stoppingDistance);
            targetPos.y = transform.position.y;

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            if (timer >= grappleDuration) FinalizeGrapple();
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
            float segmentT = (float)i / (float)(ropeSegments.Count - 1);
            float invT = 1.0f - segmentT;

            Vector3 term1 = start * (invT * invT);
            Vector3 term2 = controlPoint * (2.0f * invT * segmentT);
            Vector3 term3 = end * (segmentT * segmentT);

            Vector3 pos = term1 + term2 + term3;
            ropeSegments[i].transform.position = pos;

            if (i < ropeSegments.Count - 1)
            {
                float nextT = (float)(i + 1) / (float)(ropeSegments.Count - 1);
                float nextInvT = 1.0f - nextT;

                Vector3 nTerm1 = start * (nextInvT * nextInvT);
                Vector3 nTerm2 = controlPoint * (2.0f * nextInvT * nextT);
                Vector3 nTerm3 = end * (nextT * nextT);

                Vector3 nextPos = nTerm1 + nTerm2 + nTerm3;
                ropeSegments[i].transform.LookAt(nextPos, Vector3.Up);

                float dist = (float)Vector3.Distance(pos, nextPos);
                ropeSegments[i].transform.scale = new Vector3(0.1f, 0.1f, dist);
            }
        }
    }

    private void FinalizeGrapple()
    {
        isGrappling = false;
        foreach (var seg in ropeSegments) if (seg != null) seg.SetActive(false);
        if (activePillar != null) activePillar.ResetParticles();
    }
}