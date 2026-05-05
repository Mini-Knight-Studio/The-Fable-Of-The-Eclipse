using System;
using System.Collections;
using Loopie;

class RollingBridge : Component
{
    [Header("Identity")]
    public string bridgeID = "UNASSIGNED_BRIDGE";

    private BoxCollider collider;

    [Header("References")]
    public Entity bridgeBase;
    public Entity blockingCollider;

    [Header("Feedback")]
    public Entity audioSourceEntity;
    private AudioSource audioSource;

    public Entity audio2SourceEntity;
    private AudioSource audio2Source;
    private bool splashOnce = false;

    public Entity particlesEntity;
    private ParticleComponent particles;

    [Header("Positions")]
    public Loopie.Vector3 finalPos;
    public Loopie.Vector3 finalRotation = Loopie.Vector3.Zero;

    public Loopie.Vector3 standingPos;
    public Loopie.Vector3 standingRotation;

    [Header("Settings")]
    public float duration = 2.0f;
    public float easeIntensity = 1;
    private float elapsedTime = 0f;

    private bool animationFinished = false;
    private bool animationStarted = false;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
        audioSource = audioSourceEntity.GetComponent<AudioSource>();
        if (audio2SourceEntity != null)   audio2Source = audio2SourceEntity.GetComponent<AudioSource>();
        particles = particlesEntity.GetComponent<ParticleComponent>();
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        if (animationFinished) return;

        if (DatabaseRegistry.levelsDB.Levels.IsBridgePushed(bridgeID))
        {
            bridgeBase.transform.local_position = finalPos;
            bridgeBase.transform.local_rotation = finalRotation;
            blockingCollider.GetComponent<BoxCollider>().SetActive(false);
            animationFinished = true;
        }

        if (!animationFinished && !animationStarted && collider.IsColliding && Player.Instance.Movement.IsDashing())
        {
            animationStarted = true;

            if (audioSource != null)
                audioSource.Play();

            StartCoroutine(HandleMovement());
        }
    }

    IEnumerator HandleMovement()
    {
        elapsedTime = 0;
        while (true)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;

            float curvedT = Mathf.Lerp( 0f,1f, t, Mathf.LerpCurve.ExponentialIn, easeIntensity, Mathf.LerpStrengthMode.Power);

            Loopie.Vector3 pos = Loopie.Vector3.Lerp(standingPos, finalPos, curvedT);
            Loopie.Vector3 rot = Loopie.Vector3.Lerp(standingRotation, finalRotation, curvedT);

            bridgeBase.transform.local_position = pos;
            bridgeBase.transform.local_rotation = rot;

            if(t >= 0.8f && !splashOnce)
            {
                splashOnce = true;
                if (audio2Source != null) audio2Source.Play();
            }

            if (t >= 1f)
            {
                break;
            }
            yield return null;
        }

        animationFinished = true;
        blockingCollider.GetComponent<BoxCollider>().SetActive(false);
        DatabaseRegistry.levelsDB.Levels.SetBridgePushed(bridgeID);

        if (particles != null)
            particles.Play();
        yield return new WaitForSeconds(0.3f);
        if (particles != null)
            particles.Stop();
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}
