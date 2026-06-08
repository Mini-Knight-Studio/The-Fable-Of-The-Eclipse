using System;
using System.Collections.Generic;
using Loopie;

class KillFall : Component
{
    public bool enabled = false;
    [Header("References")]
    public Entity killEntity;
    [Header("Detectors")]
    public Entity detectorEntity1;
    public Entity detectorEntity2;
    public Entity detectorEntity3;
    public Entity detectorEntity4;

    [Header("Timings")]
    public float detectionFrequency = 0.1f;
    public float timeToKill = 0.5f;        
    public float gracePeriod = 0.2f;       

    [Header("Configuration")]
    public float distance;
    public Vector3 direction;
    public int damage;

    float detectionTimer = 0;
    float deathTimer = 0;
    bool isCurrentlyTouching = false;

    bool canReset = true;
    float saveCheckTimer = 0;

    List<Transform> detectors;
    void OnCreate()
    {
        detectors = new List<Transform>();
        detectors.Add(detectorEntity1.transform);
        detectors.Add(detectorEntity2.transform);
        detectors.Add(detectorEntity3.transform);
        detectors.Add(detectorEntity4.transform);
    }

    void OnUpdate()
    {
        if (!enabled)
            return;
        if (killEntity == null ||
            Player.Instance.Grapple.IsGrappling ||
            Player.Instance.Movement.isGodMode ||
            Player.Instance.Movement.IsDashing())
        {
            ResetState();
            return;
        }

        if (Player.Instance.transform.position.y < killEntity.transform.position.y)
        {
            saveCheckTimer += Time.deltaTime;
            if (saveCheckTimer >= 0.5f)
            {
                saveCheckTimer = 0;
                canReset = true;
                Player.Instance.StartRespawn();
                ResetState();

            }
        }
        else
            saveCheckTimer = 0;

            // 2. Optimized Raycast (Don't raycast every single frame)
            detectionTimer += Time.deltaTime;
        if (detectionTimer >= detectionFrequency)
        {
            detectionTimer = 0;
            if (canReset)
            {
                isCurrentlyTouching = true;
                foreach (Transform t in detectors)
                {
                    Entity hit = MeshRenderer.RaycastPickEntity(t.position, direction, distance);
                    isCurrentlyTouching = (hit == killEntity);
                    if (!isCurrentlyTouching)
                        break;
                }
            }
        }

        // 3. Death Logic
        if (isCurrentlyTouching)
        {
            deathTimer += Time.deltaTime;

            // Apply "Water Physics" after the grace period
            if (deathTimer > gracePeriod)
            {
                canReset = false;
                Player.Instance.Movement.gravityActive = true;
            }

            // Kill the player
            if (deathTimer > timeToKill + gracePeriod)
            {
                KillPlayer();
            }
        }
        else
        {
            // If they stop touching, reset everything immediately
            ResetState();
        }
    }

    void KillPlayer()
    {
        canReset = true;
        Player.Instance.PlayerHealth.Damage(damage);
        Player.Instance.StartRespawn();
        ResetState();
    }

    void ResetState()
    {

        if (!canReset)
            return;
        deathTimer = 0;
        isCurrentlyTouching = false;
        // Only disable gravity if we're sure this script should be controlling it
        Player.Instance.Movement.gravityActive = false;
    }

    void OnDrawGizmo()
    {
        for (int i = 0; i < detectors.Count-1; i++)
        {
            Gizmo.DrawLine(detectors[i].position, detectors[i + 1].position, Color.Orange);
        }
        Gizmo.DrawLine(detectors[0].position, detectors[detectors.Count - 1].position, Color.Orange);
    }
}