using System;
using System.Collections;
using Loopie;

class Puzzle2Blocker : Component
{
    [Header("Transition Fade Reference")]
    public Entity levelFadeOut;
    private FadeInOutEvent levelFadeOutEvent;

    public Entity fallingBridge;

    public Entity colliderEnitity;
    private BoxCollider collider;

    public Entity focusPointOnPuzzle;
    public Entity focusPointOnMural;
    public Entity focusPointOnFall;

    public Entity vistaPointAfterFall;

    [Header("Settings")]
    public float finalPlatformHeight;

    public float fallDuration = 2.0f;
    public float pauseBeforeFalling = 0.5f;
    public float easeIntensity = 1.5f;

    private bool hasFallen = false;
    private bool hasDoneCinematic = false;

    private float initialPlatformHeight;

    private string cinematicIntroID = "puzzle2Intro";
    private string cinematicBlockerID = "puzzle2Blocker";

    [Header("Camera Settings")]
    public float puzzleCamFocusDuration = 1.0f;
    public float puzzleCameraZoom = 20;

    public float muralCamFocusDuration = 1.0f;
    public float muralCameraZoom = 20;

    public float fallCamFocusDuration = 1.0f;
    public float fallCameraZoom = 20;

    public float cameraShakeDuration = 2f;
    public float cameraShakeAmount = 0.5f;
    public float cameraShakeRotation = 0.5f;
    public float cameraShakeAmountVel = 10f;
    public float cameraShakeRotationVel = 10f;

    [Header("Feedback")]
    public Entity fallingParticlesEntity;
    private ParticleComponent fallingParticles;

    public Entity fallingPlatformSFXEntity;
    private AudioSource fallingPlatformSFX;

    void OnCreate()
    {
        initialPlatformHeight = entity.transform.position.y;

        if (fallingParticlesEntity != null)
        {
            fallingParticles = fallingParticlesEntity.GetComponent<ParticleComponent>();
            fallingParticles.Stop();
        }

        levelFadeOutEvent = levelFadeOut.GetComponent<FadeInOutEvent>();
        collider = colliderEnitity.GetComponent<BoxCollider>();

        vistaPointAfterFall.SetActive(false);
        
        if(DatabaseRegistry.levelsDB.Levels.IsCinematicDone(cinematicIntroID))
        {
            hasDoneCinematic = true;
        }
        if (DatabaseRegistry.levelsDB.Levels.IsCinematicDone(cinematicBlockerID))
        {
            BridgeFinalPos();
            vistaPointAfterFall.SetActive(true);
        }
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

        if (!hasDoneCinematic)
        {
            levelFadeOutEvent.OnFadeOutComplete += StartCinematic;
        }

        if (!hasFallen && collider != null)
        {
            if (collider.HasCollided)
            {
                StartCoroutine(FallBridge());
                collider.SetActive(false);
            }
        }
    }

    private void StartCinematic()
    {
        if (hasDoneCinematic) return;
        hasDoneCinematic = true;

        levelFadeOutEvent.OnFadeOutComplete -= StartCinematic;

        StartCoroutine(Puzzle2Cinematic());
    }

    IEnumerator Puzzle2Cinematic()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        Player.Instance.Camera.FocusOnPoint(focusPointOnPuzzle.transform.position, puzzleCameraZoom, 4);
        yield return new WaitForSeconds(puzzleCamFocusDuration);

        Player.Instance.Camera.FocusOnPoint(focusPointOnMural.transform.position, muralCameraZoom, 4);
        yield return new WaitForSeconds(muralCamFocusDuration);

        Player.Instance.Camera.StopFocus();
        yield return new WaitForSeconds(0.5f);

        DatabaseRegistry.levelsDB.Levels.SetCinematicDone(cinematicIntroID);

        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    IEnumerator FallBridge()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);
        hasFallen = true;

        Player.Instance.Camera.FocusOnPoint(focusPointOnFall.transform.position, fallCameraZoom, 4);
        yield return new WaitForSeconds(0.5f);

        Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount, cameraShakeRotation, cameraShakeRotationVel, cameraShakeAmountVel);

        if (fallingParticlesEntity != null) fallingParticles.Play();
        if (fallingPlatformSFXEntity != null) fallingPlatformSFX.Play();

        yield return new WaitForSeconds(pauseBeforeFalling);

        float elapsedTime = 0f;
        while (elapsedTime < fallDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fallDuration;
            float curvedT = Mathf.Pow(t, easeIntensity);

            float currentHeight = Mathf.Lerp(initialPlatformHeight, finalPlatformHeight, curvedT);
            fallingBridge.transform.position = new Vector3(fallingBridge.transform.position.x, currentHeight, fallingBridge.transform.position.z);

            yield return null;
        }

        fallingBridge.transform.position = new Vector3(fallingBridge.transform.position.x, finalPlatformHeight, fallingBridge.transform.position.z);

        if (fallingParticlesEntity != null) fallingParticles.Stop();

        yield return new WaitForSeconds(fallCamFocusDuration);

        Player.Instance.Camera.StopFocus();
        yield return null;
        vistaPointAfterFall.SetActive(true);

        DatabaseRegistry.levelsDB.Levels.SetCinematicDone(cinematicBlockerID);

        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    public void BridgeFinalPos()
    {
        fallingBridge.transform.position = new Vector3(fallingBridge.transform.position.x, finalPlatformHeight, fallingBridge.transform.position.z);
        hasFallen = true;
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