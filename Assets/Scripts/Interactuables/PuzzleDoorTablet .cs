using System;
using System.Collections;
using Loopie;

class PuzzleDoorTablet : Component
{
    [Header("Identity")]
    public string puzzleDoorID = "UNASSIGNED_PUZZLEDOOR";
    public string requiredKeyID = "UNASSIGNED_KEY";

    [Header("References")]
    public Entity raisingTemple;
    public Entity staticKey;
    public Entity animatedKey;
    public Entity focusPointOnInsert;
    public Entity focusPointOnRaise;
    public Entity interactPrompt;

    [Header("Settings")]
    public float finalTempleHeight;
    public float camFocusDuration = 1.0f;
    public float keyTravelDuration = 1.0f;
    public float raiseDuration = 2.0f;
    public float pauseBeforeRaising = 0.5f;
    public float easeIntensity = 1.5f;
    public float cameraZoom = 20;

    public float cameraShakeDuration = 0.5f;
    public float cameraShakeAmount = 0.3f;
    public float cameraShakeRotation = 0.3f;
    public float cameraShakeAmountVel = 0.3f;
    public float cameraShakeRotationVel = 0.3f;

    private bool isRisen = false;
    private bool hasRisen = false;

    private float initialTempleHeight;
    private Vector3 finalKeyScale;

    [Header("Feedback")]
    public Entity keyParticles;
    public Entity risingParticles;

    public Entity insertKeySFX;

    void OnCreate()
    {
        finalKeyScale = animatedKey.transform.scale;

        staticKey.SetActive(false);
        animatedKey.SetActive(false);

        initialTempleHeight = raisingTemple.transform.position.y;

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Stop();
        if (keyParticles != null) keyParticles.GetComponent<ParticleComponent>().Stop();

        if (insertKeySFX != null) insertKeySFX.GetComponent<AudioSource>().Stop();

        if (DatabaseRegistry.levelsDB.Levels.IsPuzzleDoorOpened(puzzleDoorID))
        {
            hasRisen = true;
            TempleRisen();
        }
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        if (hasRisen || isRisen) return;

        if (entity.GetComponent<BoxCollider>().IsColliding && DatabaseRegistry.levelsDB.Levels.IsRewardCollected(requiredKeyID))
        {
            if (!interactPrompt.Active)
            {
                interactPrompt.SetActive(true);
            }

            if (Player.Instance.Input.interactKeyPressed)
            {
                isRisen = true;
                StartCoroutine(RaiseTemple());
                interactPrompt.SetActive(false);
            }
        }
        else
        {
            if (interactPrompt.Active)
            {
                interactPrompt.SetActive(false);
            }
        }
    }

    IEnumerator RaiseTemple()
    {
        animatedKey.SetActive(true);
        animatedKey.transform.position = Player.Instance.transform.position + new Vector3(0, 2, 0);
        animatedKey.transform.scale = Vector3.Zero;

        Vector3 startKeyPos = animatedKey.transform.position;
        Vector3 targetKeyPos = staticKey.transform.position;
        Vector3 startKeyScale = Vector3.Zero;
        Vector3 targetKeyScale = finalKeyScale;

        Player.Instance.Camera.FocusOnPoint(focusPointOnInsert.transform.position, cameraZoom / 2f, 4);

        yield return new WaitForSeconds(0.25f);

        float elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / keyTravelDuration;
            float curvedT = Mathf.Pow(t, easeIntensity);

            animatedKey.transform.position = Vector3.Lerp(startKeyPos, targetKeyPos, curvedT);
            animatedKey.transform.scale = Vector3.Lerp(startKeyScale, targetKeyScale, curvedT);

            if (t >= 0.85f && !keyParticles.GetComponent<ParticleComponent>().IsPlaying)
            {
                keyParticles.GetComponent<ParticleComponent>().Play();
                if (insertKeySFX != null) insertKeySFX.GetComponent<AudioSource>().Play();
            }
            if (t >= 1f)
            {
                animatedKey.transform.position = targetKeyPos;
                animatedKey.transform.scale = targetKeyScale;
                break;
            }
            yield return null;
        }

        animatedKey.SetActive(false);
        staticKey.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        if (keyParticles != null) keyParticles.GetComponent<ParticleComponent>().Stop();

        Player.Instance.Camera.FocusOnPoint(focusPointOnRaise.transform.position, cameraZoom, 4);

        yield return new WaitForSeconds(0.5f);

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Play();
        Player.Instance.Camera.SetIsShaking(true, raiseDuration, cameraShakeAmount, cameraShakeRotation, cameraShakeAmountVel, cameraShakeRotationVel);

        yield return new WaitForSeconds(pauseBeforeRaising);

        elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / raiseDuration;
            float curvedT = Mathf.Pow(t, easeIntensity);

            float currentHeight = Mathf.Lerp(initialTempleHeight, finalTempleHeight, curvedT);
            raisingTemple.transform.position = new Vector3(raisingTemple.transform.position.x, currentHeight, raisingTemple.transform.position.z);

            if (t >= 1f)
            {
                raisingTemple.transform.position = new Vector3(raisingTemple.transform.position.x, finalTempleHeight, raisingTemple.transform.position.z);
                break;
            }
            yield return null;
        }

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Stop();
        Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount*2, cameraShakeRotation*2, cameraShakeAmountVel, cameraShakeRotationVel);

        yield return new WaitForSeconds(1.0f);

        Player.Instance.Camera.StopFocus();

        hasRisen = true;
        DatabaseRegistry.levelsDB.Levels.SetPuzzleDoorOpened(puzzleDoorID);

        yield return null;
    }

    void TempleRisen()
    {
        staticKey.SetActive(true);
        animatedKey.SetActive(false);

        raisingTemple.transform.position = new Vector3(raisingTemple.transform.position.x, finalTempleHeight, raisingTemple.transform.position.z);

        interactPrompt.SetActive(false);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}