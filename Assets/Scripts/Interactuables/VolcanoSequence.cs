using System;
using System.Collections;
using Loopie;

class VolcanoSequence : Component
{
    [Header("Debugsito")]
    public bool playOnlyOnce = true;

    [Header("Camera Settings")]
    public float zoom = 80f;
    public float cameraSmoothness = 2.5f;
    public Vector3 focusOffset = new Vector3(0, 15, 0);
    public float cameraFocusTime = 4.0f;

    [Header("Entity References")]
    public Entity focusTarget;
    public Entity eruptionParticles;
    public Entity[] meteorites;

    public float delayBetweenMeteorites = 0.5f;

    private bool hasTriggered = false;
    private BoxCollider trigger;

    void OnCreate()
    {
        trigger = entity.GetComponent<BoxCollider>();

        if (eruptionParticles != null)
            eruptionParticles.GetComponent<ParticleComponent>().Stop();

        if (meteorites != null)
        {
            foreach (var meteorite in meteorites)
            {
                if (meteorite != null) meteorite.SetActive(false);
            }
        }
    }

    void OnUpdate()
    {
        if ((playOnlyOnce && hasTriggered) || hasTriggered) return;

        if (trigger != null && trigger.HasCollided)
        {
            hasTriggered = true;
            StartCoroutine(PlayVolcanoSequence());
        }
    }

    IEnumerator PlayVolcanoSequence()
    {
        Vector3 targetPos = focusTarget != null ? focusTarget.transform.position + focusOffset : transform.position + focusOffset;
        Player.Instance.Camera.FocusOnPoint(targetPos, zoom, cameraSmoothness);

        if (eruptionParticles != null)
            eruptionParticles.GetComponent<ParticleComponent>().Play();

        if (meteorites != null)
        {
            foreach (var meteorite in meteorites)
            {
                if (meteorite != null)
                {
                    meteorite.SetActive(true);
                    yield return new WaitForSeconds(delayBetweenMeteorites);
                }
            }
        }

        Player.Instance.Camera.SetIsShaking(true, cameraFocusTime, 0.5f, 0.4f);

        yield return new WaitForSeconds(cameraFocusTime);

        Player.Instance.Camera.StopFocus();

        if (eruptionParticles != null)
            eruptionParticles.GetComponent<ParticleComponent>().Stop();

        if (!playOnlyOnce)
        {
            yield return new WaitForSeconds(2.0f);
            hasTriggered = false;
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }

    void OnDrawGizmo()
    {
        if (focusTarget != null)
        {
            Gizmo.DrawLine(transform.position, focusTarget.transform.position, Color.Orange);
            Gizmo.DrawSphere(focusTarget.transform.position, 1f, 1.0f, Color.Red);
        }
    }
}