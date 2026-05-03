using System;
using System.Collections;
using Loopie;

class PuzzleDoor : Component
{
    [Header("References")]
    public Entity rightDoor;
    public Entity leftDoor;
    public Entity staticKey;
    public Entity animatedKey;
    public Entity focusPointOnInsert;
    public Entity blockingCollider;
    public Entity interactPrompt;

    [Header("Settings")]
    public Vector3 finalRightDoorRot = Vector3.Zero;
    public Vector3 finalLefttDoorRot = Vector3.Zero;
    public float camFocusDuration = 1.0f;
    public float keyTravelDuration = 1.0f;
    public float doorOpenDuration = 2.0f;
    public float pauseBeforeOpening = 0.5f;
    public float easeIntensity = 1.5f;
    public float cameraZoom = 20;

    private bool isOpening = false;
    private bool hasOpened = false;

    private Vector3 initialRightDoorRot;
    private Vector3 initialLeftDoorRot;
    private Vector3 finalKeyScale;

    [Header("Feedback")]
    public Entity rightParticle1;
    public Entity rightParticle2;
    public Entity rightParticle3;
    public Entity leftParticle1;
    public Entity leftParticle2;
    public Entity leftParticle3;

    public Entity keyParticles;

    public Entity door1SFX;
    public Entity door2SFX;
    public Entity door3SFX;
    public Entity impactSFX;
    public Entity insertKeySFX;

    void OnCreate()
    {
        finalKeyScale = animatedKey.transform.scale;

        staticKey.SetActive(false);
        animatedKey.SetActive(false);

        initialRightDoorRot = rightDoor.transform.local_rotation;
        initialLeftDoorRot = leftDoor.transform.local_rotation;

        rightParticle1.GetComponent<ParticleComponent>().Stop();
        rightParticle2.GetComponent<ParticleComponent>().Stop();
        rightParticle3.GetComponent<ParticleComponent>().Stop();
        leftParticle1.GetComponent<ParticleComponent>().Stop();
        leftParticle2.GetComponent<ParticleComponent>().Stop();
        leftParticle3.GetComponent<ParticleComponent>().Stop();

        keyParticles.GetComponent<ParticleComponent>().Stop();

        door1SFX.GetComponent<AudioSource>().Stop();
        door2SFX.GetComponent<AudioSource>().Stop();
        door3SFX.GetComponent<AudioSource>().Stop();

        impactSFX.GetComponent<AudioSource>().Stop();
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        if (hasOpened || isOpening) return;

        if (entity.GetComponent<BoxCollider>().IsColliding /*&& player has key*/)
        {
            if (!interactPrompt.Active)
            {
                interactPrompt.SetActive(true);
            }

            if (Player.Instance.Input.interactKeyPressed)
            {
                isOpening = true;
                StartCoroutine(OpenDoors());
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

    IEnumerator OpenDoors()
    {
        animatedKey.SetActive(true);
        animatedKey.transform.position = Player.Instance.transform.position + new Vector3(0, 2, 0);
        animatedKey.transform.scale = Vector3.Zero;

        Vector3 startKeyPos = animatedKey.transform.position;
        Vector3 targetKeyPos = staticKey.transform.position;

        Vector3 startKeyScale = Vector3.Zero;
        Vector3 targetKeyScale = finalKeyScale;

        float elapsedTime = 0f;

        Player.Instance.Camera.FocusOnPoint(focusPointOnInsert.transform.position, cameraZoom, 4);

        yield return new WaitForSeconds(camFocusDuration);

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / keyTravelDuration;

            float curvedT = Mathf.Pow(t, easeIntensity);

            animatedKey.transform.position = Vector3.Lerp(startKeyPos, targetKeyPos, curvedT);
            animatedKey.transform.scale = Vector3.Lerp(startKeyScale, targetKeyScale, curvedT);

            if(t >= 0.85f && !keyParticles.GetComponent<ParticleComponent>().IsPlaying)
            {
                keyParticles.GetComponent<ParticleComponent>().Play();
                insertKeySFX.GetComponent<AudioSource>().Play();
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

        yield return new WaitForSeconds(pauseBeforeOpening * 0.05f);

        keyParticles.GetComponent<ParticleComponent>().Stop();

        yield return new WaitForSeconds(pauseBeforeOpening * 0.95f);

        door1SFX.GetComponent<AudioSource>().Play();
        door2SFX.GetComponent<AudioSource>().Play();
        door3SFX.GetComponent<AudioSource>().Play();

        elapsedTime = 0f;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / doorOpenDuration;

            float curvedT = Mathf.Pow(t, easeIntensity);

            rightDoor.transform.local_rotation = Vector3.Lerp(initialRightDoorRot, finalRightDoorRot, curvedT);

            leftDoor.transform.local_rotation = Vector3.Lerp(initialLeftDoorRot, finalLefttDoorRot, curvedT);

            if(t >= 0.1f && !rightParticle1.GetComponent<ParticleComponent>().IsPlaying)
            {
                rightParticle1.GetComponent<ParticleComponent>().Play();
                rightParticle2.GetComponent<ParticleComponent>().Play();
                rightParticle3.GetComponent<ParticleComponent>().Play();
                leftParticle1.GetComponent<ParticleComponent>().Play();
                leftParticle2.GetComponent<ParticleComponent>().Play();
                leftParticle3.GetComponent<ParticleComponent>().Play();
            }

            if (t >= 1f)
            {
                rightDoor.transform.local_rotation = finalRightDoorRot;
                leftDoor.transform.local_rotation = finalLefttDoorRot;
                door1SFX.GetComponent<AudioSource>().Stop();
                door2SFX.GetComponent<AudioSource>().Stop();
                door3SFX.GetComponent<AudioSource>().Stop();
                impactSFX.GetComponent<AudioSource>().Play();
                break;
            }
            yield return null;
        }

        Player.Instance.Camera.StopFocus();

        rightParticle1.GetComponent<ParticleComponent>().Stop();
        rightParticle2.GetComponent<ParticleComponent>().Stop();
        rightParticle3.GetComponent<ParticleComponent>().Stop();
        leftParticle1.GetComponent<ParticleComponent>().Stop();
        leftParticle2.GetComponent<ParticleComponent>().Stop();
        leftParticle3.GetComponent<ParticleComponent>().Stop();

        blockingCollider.SetActive(false);

        hasOpened = true;

        yield return null;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}