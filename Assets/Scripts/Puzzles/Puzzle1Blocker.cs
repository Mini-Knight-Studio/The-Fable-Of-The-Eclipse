using System;
using System.Collections;
using Loopie;

class Puzzle1Blocker : Component
{
    [Header("Transition Fade Reference")]
    public Entity levelFadeOut;
    private FadeInOutEvent levelFadeOutEvent;

    [Header("Focus Points")]
    public Entity focusPointPuzzle;
    public float focusPuzzleZoom = 20f;
    public float focusPuzzleDuration = 1.0f;

    public Entity focusPointMurals;
    public float focusMuralsZoom = 20f;
    public float focusMuralsDuration = 1.0f;

    [Header("Settings")]
    public float finalVinesHeight;
    public float camFocusDuration = 1.0f;
    public float raiseDuration = 2.0f;
    public float pauseBeforeRaising = 0.5f;
    public float easeIntensity = 1.5f;
    public float cameraZoom = 20;

    private bool isBurnt = false;
    private bool hasRisen = false;
    private bool hasDoneIntroCinematic = false;

    private float initialVine;
    private string cinematicIntroID = "puzzle1IntroDone";

    [Header("Feedback")]
    public Entity risingParticles;
    public Entity vinesRiseSFX;

    void OnCreate()
    {
        initialVine = entity.transform.position.y;

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Stop();

        levelFadeOutEvent = levelFadeOut.GetComponent<FadeInOutEvent>();

        if (DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed)
        {
            isBurnt = true;
            hasRisen = true;

            VinesFinalPos();
        }

        if (DatabaseRegistry.levelsDB.Levels.IsCinematicDone(cinematicIntroID))
        {
            hasDoneIntroCinematic = true;

            if (!isBurnt)
            {
                hasRisen = true;
                VinesFinalPos();
            }
        }
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

        if (isBurnt || hasDoneIntroCinematic) return;

        if (!hasRisen)
        {
            levelFadeOutEvent.OnFadeOutComplete += StartRising;
        }
    }

    private void StartRising()
    {
        if (hasRisen) return;

        if (levelFadeOutEvent != null)
        {
            levelFadeOutEvent.OnFadeOutComplete -= StartRising;
        }

        StartCoroutine(RaiseVines());
    }

    IEnumerator RaiseVines()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);
        hasRisen = true;
        hasDoneIntroCinematic = true;

        if (focusPointPuzzle != null)
        {
            Player.Instance.Camera.FocusOnPoint(focusPointPuzzle.transform.position, focusPuzzleZoom, 4);
            yield return new WaitForSeconds(focusPuzzleDuration);
        }

        if (focusPointMurals != null)
        {
            Player.Instance.Camera.FocusOnPoint(focusPointMurals.transform.position, focusMuralsZoom, 4);
            yield return new WaitForSeconds(focusMuralsDuration);
        }

        Player.Instance.Camera.FocusOnPoint(entity.transform.position, cameraZoom, 4);
        yield return new WaitForSeconds(camFocusDuration);

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Play();
        if (vinesRiseSFX != null) vinesRiseSFX.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(pauseBeforeRaising);

        float elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / raiseDuration;
            float curvedT = Mathf.Pow(t, easeIntensity);

            float currentHeight = Mathf.Lerp(initialVine, finalVinesHeight, curvedT);
            entity.transform.position = new Vector3(entity.transform.position.x, currentHeight, entity.transform.position.z);

            if (t >= 1f)
            {
                entity.transform.position = new Vector3(entity.transform.position.x, finalVinesHeight, entity.transform.position.z);
                break;
            }
            yield return null;
        }

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Stop();

        yield return new WaitForSeconds(1.0f);

        Player.Instance.Camera.StopFocus();

        DatabaseRegistry.levelsDB.Levels.SetCinematicDone(cinematicIntroID);

        yield return null;
        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    public void VinesFinalPos()
    {
        entity.transform.position = new Vector3(entity.transform.position.x, finalVinesHeight, entity.transform.position.z);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
        if (levelFadeOutEvent != null)
        {
            levelFadeOutEvent.OnFadeOutComplete -= StartRising;
        }
    }
}