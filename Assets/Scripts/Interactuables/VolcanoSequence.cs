using System;
using System.Collections;
using Loopie;

class VolcanoSequence : Component
{
    public bool playOnlyOnce = true;

    [Header("Title Text")]
    public string textValue = "";
    public float textDurationPercent = 0f;
    public float textStartingPercent = 0f;

    private float textDuration = 0f;
    private float textStarting = 0f;

    [Header("Preparation")]
    public Entity prepFocusTarget;
    public int cameraStartingFarPlane = 1000;
    public float prepZoom = 111f;
    public float prepCameraSpeed = 4f;
    public float prepDuration = 2.5f;

    [Header("Preparation Shake (Subtle Rumble)")]
    public float prepShakeAmount = 0.1f;
    public float prepShakeRotation = 0.05f;
    public float prepShakeAmountVel = 0.1f;
    public float prepShakeRotationVel = 0.1f;

    [Header("Pre (Crater Zoom)")]
    public Entity craterFocusTarget;
    public int cameraEndingFarPlane = 300;
    public float craterZoom = 70f;
    public float craterCameraSpeed = 6f;
    public float craterFocusDuration = 1.0f;

    [Header("Crater Zoom Shake (Building Up)")]
    public float craterShakeAmount = 0.3f;
    public float craterShakeRotation = 0.2f;
    public float craterShakeAmountVel = 0.4f;
    public float craterShakeRotationVel = 0.4f;

    [Header("Camera Curve Settings")]
    public bool useCameraCurve = true;
    public float camCurveOutwardOffset = 15f;
    public float camCurveHeightOffset = 10f;

    [Header("Explosion (Massive Earthquake)")]
    public float eruptionDuration = 4.0f;
    public float eruptionShakeAmount = 0.6f;
    public float eruptionShakeRotation = 0.4f;
    public float eruptionShakeAmountVel = 0.8f;
    public float eruptionShakeRotationVel = 0.8f;
    public float delayBetweenMeteorites = 0.1f;
    public float sequenceDistance = 0f;

    [Header("References")]
    public Entity prepParticles;
    public Entity explosionParticles;

    [Header("Meteorites")]
    public Entity meteorite1;
    public Entity meteorite2;
    public Entity meteorite3;

    [Header("Audio Feedback")]
    public Entity prepRumbleSFX;
    public Entity explosionBoomSFX;
    public Entity meteoriteLaunchSFX;

    private bool hasTriggered = false;
    public static bool SequenceFinished = false;

    void OnCreate()
    {
        textStarting = prepDuration * textStartingPercent;
        textDuration = prepDuration * textDurationPercent;
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

        Player.Instance.Camera.SetFarPlane(cameraStartingFarPlane);

        if (prepRumbleSFX != null) prepRumbleSFX.GetComponent<AudioSource>().Play();

        if (prepParticles != null)
        {
            prepParticles.SetActive(true);
            prepParticles.GetComponent<ParticleComponent>().Play();
        }

        if (prepFocusTarget != null)
        {
            Player.Instance.Camera.FocusOnHeightPoint(prepFocusTarget.transform.position, prepZoom, prepCameraSpeed);
        }

        Input.StartShake(0.2f, prepDuration);
        Player.Instance.Camera.SetIsShaking(true, prepDuration, prepShakeAmount, prepShakeRotation, prepShakeAmountVel, prepShakeRotationVel);
        
        yield return new WaitForSeconds(prepDuration);
        SimpleTextUI.Instance.Open();
        SimpleTextUI.Instance.SetText(textValue);

        yield return new WaitForSeconds(textDuration);

        SimpleTextUI.Instance.Close();

        Input.StartShake(0.5f, craterFocusDuration);
        Player.Instance.Camera.SetIsShaking(true, craterFocusDuration, craterShakeAmount, craterShakeRotation, craterShakeAmountVel, craterShakeRotationVel);

        if (craterFocusTarget != null && prepFocusTarget != null)
        {
            if (useCameraCurve)
            {
                Vector3 p0 = prepFocusTarget.transform.position;
                Vector3 p2 = craterFocusTarget.transform.position;
                Vector3 centerPoint = Vector3.Lerp(p0, p2, 0.5f);
                Vector3 p1 = centerPoint + new Vector3(camCurveOutwardOffset, camCurveHeightOffset, 0f);

                float elapsed = 0f;
                while (elapsed < craterFocusDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / craterFocusDuration);
                    float u = 1f - t;

                    Vector3 currentFocusPos = (p0 * (u * u)) + (p1 * (2f * u * t)) + (p2 * (t * t));
                    float currentZoom = Mathf.Lerp(prepZoom, craterZoom, t);

                    Player.Instance.Camera.FocusOnHeightPoint(currentFocusPos, currentZoom, craterCameraSpeed);
                    yield return null;
                }
            }
            else
            {
                Player.Instance.Camera.FocusOnHeightPoint(craterFocusTarget.transform.position, craterZoom, craterCameraSpeed);
                yield return new WaitForSeconds(craterFocusDuration);
            }
        }
        else if (craterFocusTarget != null)
        {
            Player.Instance.Camera.FocusOnHeightPoint(craterFocusTarget.transform.position, craterZoom, craterCameraSpeed);
            yield return new WaitForSeconds(craterFocusDuration);
        }


        if (prepRumbleSFX != null) prepRumbleSFX.GetComponent<AudioSource>().Stop();
        if (explosionBoomSFX != null) explosionBoomSFX.GetComponent<AudioSource>().Play();

        if (prepParticles != null) prepParticles.SetActive(false);

        if (explosionParticles != null)
        {
            explosionParticles.SetActive(true);
            explosionParticles.GetComponent<ParticleComponent>().Play();
        }

        Input.StartShake(1.0f, eruptionDuration);
        Player.Instance.Camera.SetIsShaking(true, eruptionDuration, eruptionShakeAmount, eruptionShakeRotation, eruptionShakeAmountVel, eruptionShakeRotationVel);

        if (meteorite1 != null)
        {
            meteorite1.SetActive(true);
            PlayMeteoriteLaunchSound();
            yield return new WaitForSeconds(delayBetweenMeteorites);
        }
        if (meteorite2 != null)
        {
            meteorite2.SetActive(true);
            PlayMeteoriteLaunchSound();
            yield return new WaitForSeconds(delayBetweenMeteorites);
        }
        if (meteorite3 != null)
        {
            meteorite3.SetActive(true);
            PlayMeteoriteLaunchSound();
            yield return new WaitForSeconds(delayBetweenMeteorites);
        }

        yield return new WaitForSeconds(eruptionDuration);


        if (explosionParticles != null) explosionParticles.GetComponent<ParticleComponent>().Stop();

        Player.Instance.Camera.StopFocus();
        Player.Instance.Camera.distance = originalDistance;

        yield return null;
        VolcanoSequence.SequenceFinished = true;
        GameManager.SetState(GameManager.GameState.DEFAULT);
        Player.Instance.Camera.SetFarPlane(cameraEndingFarPlane);

        if (!playOnlyOnce)
        {
            yield return new WaitForSeconds(2.0f);
            hasTriggered = false;

            if (meteorite1 != null) meteorite1.SetActive(false);
            if (meteorite2 != null) meteorite2.SetActive(false);
            if (meteorite3 != null) meteorite3.SetActive(false);
        }
    }

    private void PlayMeteoriteLaunchSound()
    {
        if (meteoriteLaunchSFX != null)
        {
            meteoriteLaunchSFX.GetComponent<AudioSource>().Play();
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}