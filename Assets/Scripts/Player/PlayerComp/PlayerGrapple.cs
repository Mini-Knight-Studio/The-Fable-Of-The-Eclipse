using System;
using Loopie;

public class PlayerGrapple : Component
{
    public float grappleDuration = 0.3f;
    public float stoppingDistance = 2.0f;
    public float grappleCooldown = 2.0f; // NUEVO: Tiempo de recarga

    private bool isGrappling = false;
    private float timer = 0.0f;
    public float grappleCooldownTimer = 0.0f; // NUEVO: El temporizador que leerá el HUD

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
        // Si la habilidad está en enfriamiento, no hacemos nada
        if (grappleCooldownTimer > 0) return;

        isGrappling = true;
        timer = 0.0f;
        grappleCooldownTimer = grappleCooldown; // Reiniciamos el temporizador de CD

        startPos = transform.position;
        activePillarScript = pillarScript;

        Vector3 pillarPos = pillarScript.entity.transform.position;
        Vector3 dir = (pillarPos - transform.position).normalized;

        targetPos = pillarPos - (dir * stoppingDistance);
        targetPos.y = transform.position.y;
    }

    public void OnUpdate()
    {
        // Reducimos el tiempo de cooldown en cada frame
        if (grappleCooldownTimer > 0)
        {
            grappleCooldownTimer -= Time.deltaTime;
        }

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
}; // <-- Punto y coma