using System;
using System.Collections;
using Loopie;

class Mechanic_Idle : Component
{
    public enum MechanicType
    {
        None = 0, Torch, Grapple
    }

    [Header("MechanicType")]
    public MechanicType unlockedMechanic = MechanicType.None;

    [Header("References")]
    public Entity interactPrompt;
    public Entity ownerChest;
    public Entity popupTutorial;

    [Header("Settings")]
    public float amplitude = 0.5f;
    public float speed = 2.0f;
    public bool started = false;
    public float collectTime = 1;

    private Vector3 startLocalPos;
    private float time;
    private BoxCollider collider;
    private ParticleComponent particles;
    private bool collected = false;

    [Header("Feedback")]
    public Entity collectSFXEntity;

    private AudioSource collectSFX;

    void OnCreate()
    {
        interactPrompt.SetActive(false);

        collider = entity.GetComponent<BoxCollider>();
        collider.SetActive(false);

        particles = entity.GetComponent<ParticleComponent>();

        if (collectSFXEntity != null)
        {
            collectSFX = collectSFXEntity.GetComponent<AudioSource>();
        }

        if (unlockedMechanic == MechanicType.Torch)
        {
            if (DatabaseRegistry.playerDB.Player.hasBurner)
            {
                entity.SetActive(false);
            }
        }
        else if(unlockedMechanic == MechanicType.Grapple)
        {
            if (DatabaseRegistry.playerDB.Player.hasGrappling)
            {
                entity.SetActive(false);
            }
        }
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        if (collected)
            return;

        if(collider.IsColliding)
        {
            if (!interactPrompt.Active)
            {
                interactPrompt.SetActive(true);
            }

            if (Player.Instance.Input.interactKeyPressed)
            {
                StartCoroutine(Collect());
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

        if (!started) return;

        time += Time.deltaTime * speed;

        float offsetY = (float)Math.Sin(time) * amplitude;

        transform.local_position = new Vector3(startLocalPos.x, startLocalPos.y + offsetY, startLocalPos.z);
    }

    public void StartMoving()
    {
        startLocalPos = transform.local_position;
        time = 0f;

        started = true;

        entity.GetComponent<AudioSource>().Play();
        collider.SetActive(true);
    }

    IEnumerator Collect()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        if (collectSFXEntity != null) collectSFXEntity.GetComponent<AudioSource>().Play();

        collected = true;
        Entity player = Player.Instance.entity;
        Vector3 initialPosition = transform.position;
        Vector3 initialScale = transform.scale;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(initialPosition, player.transform.position + new Vector3(0,2,0), Mathf.Clamp01(timer / collectTime));
            transform.scale = Vector3.Lerp(initialScale, Vector3.Zero, Mathf.Clamp01(timer / collectTime));

            if (timer >= collectTime)
                break;
            else
                yield return null;
        }


        if (entity.HasComponent<AudioSource>())
        {
            entity.GetComponent<AudioSource>().Stop();
        }

        if (ownerChest != null) ownerChest.GetComponent<Chest>().RewardCollected();

        entity.SetActive(false);

        if (unlockedMechanic == MechanicType.Torch)
        {
            DatabaseRegistry.playerDB.Player.hasBurner = true;
        }
        else if (unlockedMechanic == MechanicType.Grapple)
        {
            DatabaseRegistry.playerDB.Player.hasGrappling = true;
        }

        GameManager.SetState(GameManager.GameState.DEFAULT);

        if (UIPopupManager.Instance != null)
        {
            UIPopupManager.Instance.ShowPopup(popupTutorial.Name);
        }

        yield return null;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};