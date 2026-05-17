using System;
using System.Collections;
using Loopie;

class Puzzle1Blocker : Component
{
    [Header("Transition Fade Reference")]
    public Entity levelFadeOut;
    private FadeInOutEvent levelFadeOutEvent;

    [Header("Settings")]
    public float finalVinesHeight;
    public float camFocusDuration = 1.0f;
    public float raiseDuration = 2.0f;
    public float pauseBeforeRaising = 0.5f;
    public float easeIntensity = 1.5f;
    public float cameraZoom = 20;

    private bool isBurnt = false;
    private bool hasRisen = false;

    private float initialVine;

    [Header("Feedback")]
    public Entity risingParticles;
    public Entity vinesRiseSFX;

    void OnCreate()
    {
        initialVine = entity.transform.position.y;

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Stop();

        if (DatabaseRegistry.puzzlesDB.Puzzles.Puzzle1Completed)
        {
            isBurnt = true;
        }

        levelFadeOutEvent = levelFadeOut.GetComponent<FadeInOutEvent>();
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

        if (isBurnt) return;
        
        if(!hasRisen)
            levelFadeOutEvent.OnFadeOutComplete += StartRising;
    }

    private void StartRising()
    {
        StartCoroutine(RaiseVines());
    }

    IEnumerator RaiseVines()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);
        hasRisen = true;

        Player.Instance.Camera.FocusOnPoint(entity.transform.position, cameraZoom, 4);

        yield return new WaitForSeconds(0.5f);

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

        yield return null;
        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
        if (levelFadeOutEvent == null)
            return;
        levelFadeOutEvent.OnFadeOutComplete -= StartRising;
    }
}