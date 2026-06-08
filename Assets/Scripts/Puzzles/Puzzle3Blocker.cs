using System;
using System.Collections;
using Loopie;

class Puzzle3Blocker : Component
{
    [Header("Transition Fade Reference")]
    public Entity levelFadeOut;
    private FadeInOutEvent levelFadeOutEvent;

    public Entity risingBlocker;

    public Entity focusPointOnPuzzle;
    public Entity focusPointOnMural;
    public Entity focusPointOnResetButton;

    [Header("Settings")]
    public float finalBlockerHeight;

    public float riseDuration = 2.0f;
    public float pauseBeforeRising = 0.5f;
    public float easeIntensity = 1.5f;

    private bool hasRisen = false;
    private bool hasDoneCinematic = false;

    private float initialBlockerHeight;

    private string cinematicIntroID = "puzzle3Intro";
    private string cinematicBlockerID = "puzzle3Blocker";

    [Header("Camera Settings")]
    public float puzzleCamFocusDuration = 1.5f;
    public float puzzleCameraZoom = 20f;

    public float muralCamFocusDuration = 1.5f;
    public float muralCameraZoom = 25f;

    public float puzzleBackCamFocusDuration = 1.0f;
    public float puzzleBackCameraZoom = 15f;

    public float resetButtonCamFocusDuration = 1.5f;
    public float resetButtonCameraZoom = 30f;

    public float puzzleResetCamFocusDuration = 2.0f;
    public float puzzleResetCameraZoom = 20f;

    public float riseCamFocusDuration = 1.0f;
    public float riseCameraZoom = 20f;

    public float cameraShakeDuration = 2f;
    public float cameraShakeAmount = 0.5f;
    public float cameraShakeRotation = 0.5f;
    public float cameraShakeAmountVel = 10f;
    public float cameraShakeRotationVel = 10f;

    [Header("Feedback")]
    public Entity risingParticlesEntity;
    private ParticleComponent risingParticles;

    public Entity risingPlatformSFXEntity;
    private AudioSource risingPlatformSFX;

    void OnCreate()
    {
        initialBlockerHeight = risingBlocker.transform.position.y;

        if (risingParticlesEntity != null)
        {
            risingParticles = risingParticlesEntity.GetComponent<ParticleComponent>();
            risingParticles.Stop();
        }

        if (risingPlatformSFXEntity != null)
        {
            risingPlatformSFX = risingPlatformSFXEntity.GetComponent<AudioSource>();
        }

        levelFadeOutEvent = levelFadeOut.GetComponent<FadeInOutEvent>();

        if (DatabaseRegistry.levelsDB.Levels.IsCinematicDone(cinematicIntroID))
        {
            hasDoneCinematic = true;
        }
        if (DatabaseRegistry.levelsDB.Levels.IsCinematicDone(cinematicBlockerID))
        {
            BlockerFinalPos();
        }
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

        if (!hasDoneCinematic)
        {
            levelFadeOutEvent.OnFadeOutComplete += StartCinematic;
        }
    }

    private void StartCinematic()
    {
        if (hasDoneCinematic) return;
        hasDoneCinematic = true;

        levelFadeOutEvent.OnFadeOutComplete -= StartCinematic;

        StartCoroutine(MainCinematicSequence());
    }

    IEnumerator MainCinematicSequence()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        Player.Instance.Camera.FocusOnPoint(focusPointOnPuzzle.transform.position, puzzleCameraZoom, 4);
        yield return new WaitForSeconds(puzzleCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(focusPointOnMural.transform.position, muralCameraZoom, 4);
        yield return new WaitForSeconds(muralCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(focusPointOnPuzzle.transform.position, puzzleBackCameraZoom, 4);
        yield return new WaitForSeconds(puzzleBackCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(focusPointOnResetButton.transform.position, resetButtonCameraZoom, 4);
        yield return new WaitForSeconds(resetButtonCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(focusPointOnPuzzle.transform.position, puzzleResetCameraZoom, 4);
        yield return new WaitForSeconds(puzzleResetCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(risingBlocker.transform.position, riseCameraZoom, 4);
        yield return new WaitForSeconds(0.5f);

        Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount, cameraShakeRotation, cameraShakeRotationVel, cameraShakeAmountVel);

        if (risingParticles != null) risingParticles.Play();
        if (risingPlatformSFX != null) risingPlatformSFX.Play();

        yield return new WaitForSeconds(pauseBeforeRising);

        hasRisen = true;
        float elapsedTime = 0f;
        while (elapsedTime < riseDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / riseDuration;
            float curvedT = Mathf.Pow(t, easeIntensity);

            float currentHeight = Mathf.Lerp(initialBlockerHeight, finalBlockerHeight, curvedT);
            risingBlocker.transform.position = new Vector3(risingBlocker.transform.position.x, currentHeight, risingBlocker.transform.position.z);

            yield return null;
        }

        risingBlocker.transform.position = new Vector3(risingBlocker.transform.position.x, finalBlockerHeight, risingBlocker.transform.position.z);

        if (risingParticles != null) risingParticles.Stop();

        yield return new WaitForSeconds(riseCamFocusDuration);

        Player.Instance.Camera.StopFocus();
        yield return new WaitForSeconds(0.5f);

        DatabaseRegistry.levelsDB.Levels.SetCinematicDone(cinematicIntroID);
        DatabaseRegistry.levelsDB.Levels.SetCinematicDone(cinematicBlockerID);

        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    public void BlockerFinalPos()
    {
        risingBlocker.transform.position = new Vector3(risingBlocker.transform.position.x, finalBlockerHeight, risingBlocker.transform.position.z);
        hasRisen = true;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
        if (levelFadeOutEvent != null)
        {
            levelFadeOutEvent.OnFadeOutComplete -= StartCinematic;
        }
    }
}