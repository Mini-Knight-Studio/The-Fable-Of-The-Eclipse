using System;
using System.Collections;
using Loopie;

public class PlayerTorch : PlayerComponent
{
    public float burnDuration = 2.0f;

    public Entity torchEntity;

    public Entity fireParticleEntity;
    private ParticleComponent fireParticle;

    private bool isTorching = false;
    public bool IsTorching => isTorching;

    void OnCreate()
    {
        if (torchEntity != null) torchEntity.SetActive(false);
        if (fireParticleEntity != null)
        {
            fireParticle = fireParticleEntity.GetComponent<ParticleComponent>();
        }
    }

    public void ProcessTorch()
    {
        if (isTorching || player.Grapple.IsGrappling || player.Combat.isAttacking || player.Movement.IsDashing())
            return;

        if (player.Input.torchKeyPressed /*DatabaseRegistry.playerDB.Player.hasBurner && !isTorching*/)
        {
            StartCoroutine(TorchSequence());
        }
    }

    private IEnumerator TorchSequence()
    {
        isTorching = true;

        float sequenceDuration = burnDuration;
        float torchDelay = 0.6f;

        sequenceDuration -= torchDelay;
        yield return new WaitForSeconds(torchDelay);

        if (torchEntity != null) torchEntity.SetActive(true);
        if (fireParticle != null) fireParticle.Play();

        float timer = 0.0f;
        while (timer < sequenceDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (fireParticle != null) fireParticle.Stop();
        if (torchEntity != null) torchEntity.SetActive(false);

        isTorching = false;
    }
}