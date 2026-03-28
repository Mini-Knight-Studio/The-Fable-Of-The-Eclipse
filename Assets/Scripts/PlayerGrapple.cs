using System;
using Loopie;

public class PlayerGrapple : Component
{
    public float grappleDuration = 0.3f;
    public float stoppingDistance = 2.0f;

    private Entity currentPillar = null;
    private bool isGrappling = false;
    private float grappleTimer = 0.0f;
    private Vector3 grappleStartPos;
    private Vector3 grappleTargetPos;

    public void SetGrappleTarget(Entity pillar) { currentPillar = pillar; }
    public void ClearGrappleTarget(Entity pillar) { if (currentPillar == pillar) currentPillar = null; }

    public void OnUpdate()
    {
        if (isGrappling)
        {
            PerformGrappleMovement();
            return;
        }

        if (currentPillar != null && Input.IsKeyPressed(KeyCode.I))
        {
            StartGrapple();
        }
    }

    private void StartGrapple()
    {
        isGrappling = true;
        grappleTimer = 0.0f;
        grappleStartPos = transform.position;

        Vector3 dirToPillar = (currentPillar.transform.position - transform.position).normalized;

        grappleTargetPos = currentPillar.transform.position - (dirToPillar * stoppingDistance);

        grappleTargetPos.y = transform.position.y;

        transform.LookAt(grappleTargetPos, Vector3.Up);

        Debug.Log("Grappling");
    }

    private void PerformGrappleMovement()
    {
        grappleTimer += Time.deltaTime;
        float t = grappleTimer / grappleDuration;

        if (t >= 1.0f)
        {
            t = 1.0f;
            isGrappling = false;
        }
        transform.position = Vector3.Lerp(grappleStartPos, grappleTargetPos, t);
    }
}