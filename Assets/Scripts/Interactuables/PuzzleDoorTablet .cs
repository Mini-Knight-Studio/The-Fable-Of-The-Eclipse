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
    public Entity movingPlatform;

    [Header("Settings")]
    public float finalTempleHeight;
    public float camFocusDuration = 1.0f;
    public float keyTravelDuration = 1.0f;
    public float raiseDuration = 2.0f;
    public float pauseBeforeRaising = 0.5f;
    public float easeIntensity = 1.5f;
    public bool modifyParticles = true;
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

    public float movingPlatformFinalHeight;

    [Header("Feedback")]
    public Entity keyParticles;
    public Entity risingParticles;
    public Entity risingParticles2;
    public Entity risingParticles3;
    public Entity risingParticles4;

    public Entity insertKeySFX;
    public Entity doorStartSFX;
    public Entity doorRiseSFX;
    public Entity doorEndSFX;
    public Entity drippingSFX;

    void OnCreate()
    {
        finalKeyScale = animatedKey.transform.scale;

        staticKey.SetActive(false);
        animatedKey.SetActive(false);

        initialTempleHeight = raisingTemple.transform.position.y;

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Stop();
        if (risingParticles2 != null) risingParticles2.GetComponent<ParticleComponent>().Stop();
        if (risingParticles3 != null) risingParticles3.GetComponent<ParticleComponent>().Stop();
        if (risingParticles4 != null) risingParticles4.GetComponent<ParticleComponent>().Stop();
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
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

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
        GameManager.SetState(GameManager.GameState.PAUSE);
        animatedKey.SetActive(true);
        animatedKey.transform.position = Player.Instance.transform.position + new Vector3(0, 2, 0);
        animatedKey.transform.scale = Vector3.Zero;

        Vector3 startKeyPos = animatedKey.transform.position;
        Vector3 targetKeyPos = staticKey.transform.position;
        Vector3 startKeyScale = Vector3.Zero;
        Vector3 targetKeyScale = finalKeyScale;

        Player.Instance.Camera.FocusOnPoint(focusPointOnInsert.transform.position, cameraZoom / 2f, 6);


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
                if (doorStartSFX != null) doorStartSFX.GetComponent<AudioSource>().Play();
            }
            if (t >= 1f)
            {
                animatedKey.transform.position = targetKeyPos;
                animatedKey.transform.scale = targetKeyScale;
                break;
            }
            yield return null;
        }

        if (drippingSFX != null) drippingSFX.GetComponent<AudioSource>().Play();

        animatedKey.SetActive(false);
        staticKey.SetActive(true);

        Input.StartShake(.7f, raiseDuration);

        Player.Instance.Camera.SetIsShaking(true, raiseDuration + pauseBeforeRaising + 1, cameraShakeAmount, cameraShakeRotation, cameraShakeAmountVel, cameraShakeRotationVel);

        yield return new WaitForSeconds(0.5f);
        if (keyParticles != null) keyParticles.GetComponent<ParticleComponent>().Stop();

        Player.Instance.Camera.FocusOnPoint(focusPointOnRaise.transform.position, cameraZoom, 4);

        yield return new WaitForSeconds(0.5f);

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Play();
        if (risingParticles2 != null) risingParticles2.GetComponent<ParticleComponent>().Play();
        if (risingParticles3 != null) risingParticles3.GetComponent<ParticleComponent>().Play();
        if (risingParticles4 != null) risingParticles4.GetComponent<ParticleComponent>().Play();

        if (doorRiseSFX != null) doorRiseSFX.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(pauseBeforeRaising);

        elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / raiseDuration;
            float curvedT = Mathf.Pow(t, easeIntensity);

            float currentHeight = Mathf.Lerp(initialTempleHeight, finalTempleHeight, curvedT);
            raisingTemple.transform.position = new Vector3(raisingTemple.transform.position.x, currentHeight, raisingTemple.transform.position.z);

            float particleT = Mathf.Min(1.0f, t * 2f);
            float particleCurvedT = Mathf.Pow(particleT, easeIntensity);

            float currentSpreadX = Mathf.Lerp(1.5f, 5.5f, particleCurvedT);
            float currentSpreadZ = Mathf.Lerp(1.5f, 5.5f, particleCurvedT);
            Vector3 variation = new Vector3(currentSpreadX, 0, currentSpreadZ);

            if(modifyParticles)
            {
                if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().SetPositionVariation(0, variation);
                if (risingParticles2 != null) risingParticles2.GetComponent<ParticleComponent>().SetPositionVariation(0, variation);
                if (risingParticles3 != null) risingParticles3.GetComponent<ParticleComponent>().SetPositionVariation(0, variation);
                if (risingParticles4 != null) risingParticles4.GetComponent<ParticleComponent>().SetPositionVariation(0, variation);
            }

            if (t >= 1f)
            {
                raisingTemple.transform.position = new Vector3(raisingTemple.transform.position.x, finalTempleHeight, raisingTemple.transform.position.z);
                if (doorEndSFX != null) doorEndSFX.GetComponent<AudioSource>().Play();
                break;
            }
            yield return null;
        }

        if (risingParticles != null) risingParticles.GetComponent<ParticleComponent>().Stop();
        if (risingParticles2 != null) risingParticles2.GetComponent<ParticleComponent>().Stop();
        if (risingParticles3 != null) risingParticles3.GetComponent<ParticleComponent>().Stop();
        if (risingParticles4 != null) risingParticles4.GetComponent<ParticleComponent>().Stop();
        Input.StartShake(1, .1f);

        Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount * 2, cameraShakeRotation * 2, cameraShakeAmountVel, cameraShakeRotationVel);
        yield return new WaitForSeconds(1.0f);

        Player.Instance.Camera.StopFocus();

        yield return new WaitForSeconds(1.0f);

        elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 3.0f;
            float curvedT = Mathf.Pow(t, easeIntensity);

            float currentHeight = Mathf.Lerp(movingPlatform.transform.position.y, movingPlatformFinalHeight, curvedT);
            movingPlatform.transform.position = new Vector3(movingPlatform.transform.position.x, currentHeight, movingPlatform.transform.position.z);

            if (t >= 1f)
            {
                movingPlatform.transform.position = new Vector3(movingPlatform.transform.position.x, movingPlatformFinalHeight, movingPlatform.transform.position.z);
                break;
            }
            yield return null;
        }


        hasRisen = true;
        DatabaseRegistry.levelsDB.Levels.SetPuzzleDoorOpened(puzzleDoorID);

        yield return null;
        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    void TempleRisen()
    {
        staticKey.SetActive(true);
        animatedKey.SetActive(false);

        raisingTemple.transform.position = new Vector3(raisingTemple.transform.position.x, finalTempleHeight, raisingTemple.transform.position.z);
        movingPlatform.transform.position = new Vector3(movingPlatform.transform.position.x, movingPlatformFinalHeight, movingPlatform.transform.position.z);

        if (drippingSFX != null) drippingSFX.GetComponent<AudioSource>().Play();

        interactPrompt.SetActive(false);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}