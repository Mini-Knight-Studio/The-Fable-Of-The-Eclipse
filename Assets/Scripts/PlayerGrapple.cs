using System;
using Loopie;

public class PlayerGrapple : Component
{
    public float grappleDuration = 0.3f;
    public float stoppingDistance = 2.0f;

    private bool isGrappling = false;
    private float timer = 0.0f;
    private Vector3 startPos;
    private Vector3 targetPos;

    private PillarTrigger activePillarScript;

    public void RotateToTarget(Vector3 pillarPos)
    {
        Vector3 lookAtPos = new Vector3(pillarPos.x, transform.position.y, pillarPos.z);
        transform.LookAt(lookAtPos, Vector3.Up);
    }

    public void ExecuteGrapple(PillarTrigger pillarScript)
    {
        isGrappling = true;
        timer = 0.0f;
        startPos = transform.position;
        activePillarScript = pillarScript;

        Vector3 pillarPos = pillarScript.entity.transform.position;
        Vector3 dir = (pillarPos - transform.position).normalized;

        targetPos = pillarPos - (dir * stoppingDistance);
        targetPos.y = transform.position.y;
    }

    public void OnUpdate()
    {
        if (!isGrappling) return;

        timer += Time.deltaTime;
        float t = timer / grappleDuration;

        if (t >= 1.0f)
        {
            t = 1.0f;
            isGrappling = false;
            if (activePillarScript != null) activePillarScript.ResetParticles();
        }

        transform.position = Vector3.Lerp(startPos, targetPos, t);
    }
}