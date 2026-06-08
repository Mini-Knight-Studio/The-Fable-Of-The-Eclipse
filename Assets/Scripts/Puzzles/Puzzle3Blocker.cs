using System;
using System.Collections;
using Loopie;

class Puzzle3Blocker : Component
{
    [Header("Transition Fade Reference")]
    public Entity levelFadeOut;
    private FadeInOutEvent levelFadeOutEvent;

    public Entity fallingRock;

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

    [Header("Puzzle completition")]

    public Entity rockFocusPoint;
    public float rockFocusDuration = 20f;
    public float rockCameraZoom = 20f;

    public float lowerDuration = 20f;
    public float initialRockHeight = 20f;
    public float finalRockHeight = 20f;

    public float pauseBeforeLowering = 20f;
    public float rockShakeDuration = 20f;
    public float rockShakeAmount = 20f;
    public float rockShakeRotation = 20f;
    public float rockShakeAmountVel = 20f;
    public float rockShakeRotationVel = 20f;

    public Entity loweringParticlesEntity;
    private ParticleComponent loweringParticles;

    public Entity loweringRockSFXEntity;
    private AudioSource loweringRockSFX;

    void OnCreate()
    {
        initialBlockerHeight = fallingRock.transform.position.y;

        if (risingParticlesEntity != null)
        {
            risingParticles = risingParticlesEntity.GetComponent<ParticleComponent>();
            risingParticles.Stop();
        }
        if (risingPlatformSFXEntity != null)
        {
            risingPlatformSFX = risingPlatformSFXEntity.GetComponent<AudioSource>();
        }

        if (loweringParticlesEntity != null)
        {
            loweringParticles = loweringParticlesEntity.GetComponent<ParticleComponent>();
            loweringParticles.Stop();
        }
        if (loweringRockSFXEntity != null)
        {
            loweringRockSFX = loweringRockSFXEntity.GetComponent<AudioSource>();
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

        Player.Instance.Camera.FocusOnPoint(focusPointOnMural.transform.position, muralCameraZoom, 4);
        yield return new WaitForSeconds(muralCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(focusPointOnPuzzle.transform.position, puzzleBackCameraZoom, 4);
        yield return new WaitForSeconds(puzzleBackCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(focusPointOnResetButton.transform.position, resetButtonCameraZoom, 4);
        yield return new WaitForSeconds(resetButtonCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(focusPointOnPuzzle.transform.position, puzzleResetCameraZoom, 4);
        yield return new WaitForSeconds(puzzleResetCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(rockFocusPoint.transform.position, riseCameraZoom, 4);
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
            fallingRock.transform.position = new Vector3(fallingRock.transform.position.x, currentHeight, fallingRock.transform.position.z);

            yield return null;
        }

        fallingRock.transform.position = new Vector3(fallingRock.transform.position.x, finalBlockerHeight, fallingRock.transform.position.z);

        if (risingParticles != null) risingParticles.Stop();

        yield return new WaitForSeconds(riseCamFocusDuration);

        Player.Instance.Camera.StopFocus();
        yield return new WaitForSeconds(0.5f);

        DatabaseRegistry.levelsDB.Levels.SetCinematicDone(cinematicIntroID);
        DatabaseRegistry.levelsDB.Levels.SetCinematicDone(cinematicBlockerID);

        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    public void StartCompletitionCinematic()
    {
        StartCoroutine(PuzzleCompleteCinematic());
    }

    IEnumerator PuzzleCompleteCinematic()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        if (fallingRock != null)
        {
            if (fallingRock != null)
            {
                initialRockHeight = fallingRock.transform.position.y;
            }

            Player.Instance.Camera.FocusOnPoint(rockFocusPoint.transform.position, rockCameraZoom, 4);
            yield return new WaitForSeconds(rockFocusDuration);

            if (loweringParticles != null) loweringParticles.Play();
            if (loweringRockSFX != null) loweringRockSFX.Play();

            yield return new WaitForSeconds(pauseBeforeLowering);

            Player.Instance.Camera.SetIsShaking(true, rockShakeDuration, rockShakeAmount, rockShakeRotation, rockShakeAmountVel, rockShakeRotationVel);

            float elapsedTime = 0f;
            while (elapsedTime < lowerDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / lowerDuration;
                float curvedT = Mathf.Pow(t, easeIntensity);

                float currentHeight = Mathf.Lerp(initialRockHeight, finalRockHeight, curvedT);
                fallingRock.transform.position = new Vector3(fallingRock.transform.position.x, currentHeight, fallingRock.transform.position.z);

                yield return null;
            }

            fallingRock.transform.position = new Vector3(fallingRock.transform.position.x, finalRockHeight, fallingRock.transform.position.z);
        
            if (loweringParticles != null) loweringParticles.Stop();
        }

        yield return new WaitForSeconds(1f);
        Player.Instance.Camera.StopFocus();
        yield return new WaitForSeconds(0.5f);

        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    public void BlockerFinalPos()
    {
        fallingRock.transform.position = new Vector3(fallingRock.transform.position.x, finalBlockerHeight, fallingRock.transform.position.z);
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