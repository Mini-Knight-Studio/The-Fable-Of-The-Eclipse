using System;
using System.Collections;
using Loopie;

class Chest : Component
{
    [Header("References")]
    public Entity animatedMoon;
    public Entity staticMoon;
    public Entity upperPart;
    public Entity rewardItem;

    [Header("Feedback")]
    public Entity openSFXEntity;
    public Entity moonSFXEntity;

    public Entity moonParticlesEntity;
    public Entity openParticlesEntity;

    [Header("Positions & Scales")]
    public Loopie.Vector3 moonStartRot;
    public Loopie.Vector3 lidStartRot;
    public Loopie.Vector3 rewardStartPos;
    public Loopie.Vector3 rewardStartScale;

    [Header("Settings")]
    public float moonAnimDuration = 0.5f;
    public float chestAnimDuration = 0.66f;
    public float easeIntensity = 1.0f;
    public float rewardRiseDistance = 2.0f;
    public float rewardScaleMultiplier = 2.0f;
    public float yieldTime = 0.2f;

    private float elapsedTime = 0f;
    private bool isOpen = false;
    private bool animationStarted = false;
    private bool rewardCollected = false;
    private bool particlesStopped = false;

    void OnCreate()
    {
        staticMoon.SetActive(false);
        animatedMoon.SetActive(true);

        if (rewardItem != null)
        {
            rewardItem.transform.scale = Loopie.Vector3.Zero;
            rewardItem.SetActive(false);
        }

        // if database.chest is open --> ChestOpened()
    }

    void OnUpdate()
    {
        if (rewardCollected && !particlesStopped)
        {
            openParticlesEntity.GetComponent<ParticleComponent>().Stop();
            particlesStopped = true;
        }

        if (isOpen || animationStarted) return;

        if (entity.GetComponent<BoxCollider>().IsColliding && Input.IsKeyDown(KeyCode.E))
        {
            animationStarted = true;
            StartCoroutine(HandleChestAnimation());
        }
    }

    IEnumerator HandleChestAnimation()
    {
        // phase 1: rotate moon
        elapsedTime = 0f;
        Loopie.Vector3 targetMoonRot = new Loopie.Vector3(-90f, moonStartRot.y, moonStartRot.z);

        moonSFXEntity.GetComponent<AudioSource>().Play();

        moonParticlesEntity.GetComponent<ParticleComponent>().Play();

        yield return new WaitForSeconds(0.2f);

        moonParticlesEntity.GetComponent<ParticleComponent>().Stop();

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moonAnimDuration;

            animatedMoon.transform.local_rotation = Loopie.Vector3.Lerp(moonStartRot, targetMoonRot, t);

            if (t >= 1f)
            {
                animatedMoon.transform.local_rotation = targetMoonRot;
                break;
            }
            yield return null;
        }

        staticMoon.SetActive(true);
        animatedMoon.SetActive(false);

        yield return new WaitForSeconds(yieldTime);

        openParticlesEntity.GetComponent<ParticleComponent>().Play();

        openSFXEntity.GetComponent<AudioSource>().Play();

        rewardItem.SetActive(true);

        // phase 2: lid and reward
        elapsedTime = 0f;
        Loopie.Vector3 targetLidRot = new Loopie.Vector3(lidStartRot.x, lidStartRot.y, lidStartRot.z + 108.0f);

        Loopie.Vector3 targetRewardPos = new Loopie.Vector3(rewardStartPos.x, rewardStartPos.y + rewardRiseDistance, rewardStartPos.z);
        Loopie.Vector3 targetRewardScale = rewardStartScale * rewardScaleMultiplier;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / chestAnimDuration;

            float curvedT = Mathf.Pow(t, easeIntensity);

            upperPart.transform.local_rotation = Loopie.Vector3.Lerp(lidStartRot, targetLidRot, curvedT);

            if (rewardItem != null)
            {
                rewardItem.transform.local_position = Loopie.Vector3.Lerp(rewardStartPos, targetRewardPos, curvedT);
                rewardItem.transform.scale = Loopie.Vector3.Lerp(Loopie.Vector3.Zero, targetRewardScale, curvedT);
            }

            if (t >= 1f)
            {
                upperPart.transform.local_rotation = targetLidRot;
                if (rewardItem != null)
                {
                    rewardItem.transform.local_position = targetRewardPos;
                    rewardItem.transform.scale = targetRewardScale;
                }
                break;
            }
            yield return null;
        }

        isOpen = true;
        // DatabaseRegistry.playerDB.hasOpenedChest(tostring(chestId)) or whatever
    }

    void ChestOpened()
    {

    }

    public void RewardCollected()
    {
        rewardCollected = true;
        // rewardCollected on database
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}