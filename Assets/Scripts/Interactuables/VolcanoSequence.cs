using System;
using System.Collections;
using Loopie;

class VolcanoSequence : Component
{
    [Header("Flow Control")]
    public bool playOnlyOnce = true;

    [Header("Preparation")]
    public Entity prepFocusTarget;
    public float prepZoom = 111f;
    public float prepCameraSpeed = 4f;
    public float prepDuration = 2.5f;

    [Header("Explosion")]
    public Entity craterFocusTarget;
    public float craterZoom = 70f;
    public float craterCameraSpeed = 6f;
    public float craterFocusDuration = 1.0f;

    [Header("Timings")]
    public float eruptionDuration = 4.0f;
    public float delayBetweenMeteorites = 0.1f;
    public float sequenceDistance = 0f;

    [Header("References")]
    public Entity prepParticles;
    public Entity explosionParticles;

    [Header("Meteorites")]
    public Entity meteorite1;
    public Entity meteorite2;
    public Entity meteorite3;

    private bool hasTriggered = false;

    void OnCreate()
    {
        if (prepParticles != null) prepParticles.GetComponent<ParticleComponent>().Stop();
        if (explosionParticles != null) explosionParticles.GetComponent<ParticleComponent>().Stop();

        if (meteorite1 != null) meteorite1.SetActive(false);
        if (meteorite2 != null) meteorite2.SetActive(false);
        if (meteorite3 != null) meteorite3.SetActive(false);
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        if (hasTriggered) return;

        if (entity.GetComponent<BoxCollider>().IsColliding)
        {
            hasTriggered = true;
            StartCoroutine(PlayVolcanoSequence());
        }
    }

    IEnumerator PlayVolcanoSequence()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        float originalDistance = Player.Instance.Camera.distance;
        Player.Instance.Camera.distance = sequenceDistance;

        if (prepParticles != null)
        {
            prepParticles.SetActive(true);
            prepParticles.GetComponent<ParticleComponent>().Play();
        }

        if (prepFocusTarget != null)
        {
            Player.Instance.Camera.FocusOnHeightPoint(prepFocusTarget.transform.position, prepZoom, prepCameraSpeed);
        }

        yield return new WaitForSeconds(prepDuration);


        if (craterFocusTarget != null)
        {
            Player.Instance.Camera.FocusOnHeightPoint(craterFocusTarget.transform.position, craterZoom, craterCameraSpeed);
        }

        yield return new WaitForSeconds(craterFocusDuration);


        if (prepParticles != null) prepParticles.SetActive(false);

        if (explosionParticles != null)
        {
            explosionParticles.SetActive(true);
            explosionParticles.GetComponent<ParticleComponent>().Play();
        }

        Player.Instance.Camera.SetIsShaking(true, eruptionDuration, 0.5f, 0.4f);

        if (meteorite1 != null) { meteorite1.SetActive(true); yield return new WaitForSeconds(delayBetweenMeteorites); }
        if (meteorite2 != null) { meteorite2.SetActive(true); yield return new WaitForSeconds(delayBetweenMeteorites); }
        if (meteorite3 != null) { meteorite3.SetActive(true); yield return new WaitForSeconds(delayBetweenMeteorites); }

        yield return new WaitForSeconds(eruptionDuration);

        if (explosionParticles != null) explosionParticles.GetComponent<ParticleComponent>().Stop();

        Player.Instance.Camera.StopFocus();
        Player.Instance.Camera.distance = originalDistance;

        yield return null;

        GameManager.SetState(GameManager.GameState.DEFAULT);

        if (!playOnlyOnce)
        {
            yield return new WaitForSeconds(2.0f);
            hasTriggered = false;

            if (meteorite1 != null) meteorite1.SetActive(false);
            if (meteorite2 != null) meteorite2.SetActive(false);
            if (meteorite3 != null) meteorite3.SetActive(false);
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}