using System;
using System.Collections;
using Loopie;

class MovingPillarSimonSays : Component
{
    [Header("References")]
    public Entity torch;
    public Entity interactPrompt;

    private ParticleComponent torchParticles;
    private BoxCollider torchCollider;
    private MovingPillar movingPillar;

    [Header("Settings")]
    private bool enabled = false;

    public bool active = false;
    public bool wasPressed = false;

    private bool activated = false;
    private bool locked = true;

    [Header("Feedback")]
    public Entity igniteSFXEntity;
    public Entity fireSFXEntity;
    public Entity pillarModel;

    private Material pillarMaterial;
    private float defaultIntensity = 0.5f;

    void OnCreate()
    {
        movingPillar = entity.GetComponent<MovingPillar>();

        if (torch != null)
        {
            torchParticles = torch.GetComponent<ParticleComponent>();
            torchParticles.Stop();
            torchCollider = torch.GetComponent<BoxCollider>();
        }

        if (pillarModel != null)
        {
            pillarMaterial = pillarModel.GetComponent<MeshRenderer>().GetInstancedMaterial();
            defaultIntensity = pillarMaterial.GetFloat("u_EmissiveIntensity");
        }
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        wasPressed = false;

        if (movingPillar == null) return;
        if (!movingPillar.onGoalPosition) return;

        if (!enabled)
        {
            if (torch != null) torch.SetActive(true);

            if (torchParticles != null && !active)
            {
                torchParticles.Stop();
            }

            enabled = true;
            StartCoroutine(EmissiveShine());
        }

        HandleActivation();
    }

    void HandleActivation()
    {
        if (active && !activated) return;

        if (torchCollider != null && torchCollider.IsColliding && !locked && Player.Instance.Input.interactKeyPressed)
        {
            wasPressed = true;
            active = true;
        }

        if (active && !activated)
        {
            if (torchParticles != null)
            {
                torchParticles.Play();
                fireSFXEntity.GetComponent<AudioSource>().Play();
                igniteSFXEntity.GetComponent<AudioSource>().Play();
                movingPillar.onGoalSFX.GetComponent<AudioSource>().Play();
            }
            activated = true;
        }
        else if (!active && activated)
        {
            if (torchParticles != null)
            {
                torchParticles.Stop();
                fireSFXEntity.GetComponent<AudioSource>().Stop();
            }
            activated = false;
        }
    }

    public void ForceActive()
    {
        active = true;

        if (!activated)
        {
            if (torchParticles != null)
            {
                torchParticles.Play();
                movingPillar.onGoalSFX.GetComponent<AudioSource>().Play();
                fireSFXEntity.GetComponent<AudioSource>().Play();
                igniteSFXEntity.GetComponent<AudioSource>().Play();
            }
            activated = true;
        }
    }

    public void CompletePillarSimonSaysAuto()
    {
        enabled = true;
        active = true;
        activated = true;
        locked = false;

        if (torch != null)
        {
            torch.SetActive(true);
        }

        if (torchParticles != null)
        {
            torchParticles.Play();
        }

        igniteSFXEntity.GetComponent<AudioSource>().Volume = 0.1f;
        igniteSFXEntity.GetComponent<AudioSource>().Play();

        StartCoroutine(EmissiveShine());
    }

    public void ResetState()
    {
        active = false;
        wasPressed = false;

        if (activated)
        {
            if (torchParticles != null)
            {
                torchParticles.Stop();
                fireSFXEntity.GetComponent<AudioSource>().Stop();
            }
            activated = false;
        }
    }

    public void Unlock()
    {
        locked = false;
    }

    public void Lock()
    {
        locked = true;
    }

    IEnumerator EmissiveShine()
    {
        if (pillarMaterial == null) yield break;

        float currentIntensity = defaultIntensity;
        float transitionSpeed = 2f;
        float pulseSpeed = 2f;
        float minPulse = 0.8f;
        float maxPulse = 1.2f;
        float pulseTimer = 0f;

        while (true)
        {
            float targetIntensity = activated ? maxPulse : defaultIntensity;

            if (activated)
            {
                pulseTimer += Time.deltaTime;
                float pulseFactor = (Mathf.Sin(pulseTimer * pulseSpeed) + 1f) / 2f;
                targetIntensity = Mathf.Lerp(minPulse, maxPulse, pulseFactor);
            }
            else
            {
                pulseTimer = 0f;
            }

            currentIntensity = Mathf.MoveTowards(currentIntensity, targetIntensity, transitionSpeed * Time.deltaTime);
            pillarMaterial.SetFloat("u_EmissiveIntensity", currentIntensity);

            yield return null;
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}