using System;
using System.Collections;
using Loopie;

class Key_Idle : Component
{
    [Header("References")]
    public Entity interactPrompt;
    public Entity ownerChest;
    public Entity focusPointOnCollect;

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

    void OnCreate()
    {
        interactPrompt.SetActive(false);

        collider = entity.GetComponent<BoxCollider>();
        collider.SetActive(false);

        particles = entity.GetComponent<ParticleComponent>();
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

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

        if(focusPointOnCollect != null)
        {
            Player.Instance.Camera.FocusOnPoint(focusPointOnCollect.transform.position, 15, 6);

            yield return new WaitForSeconds(2.5f);

            Player.Instance.Camera.StopFocus();
        }


        yield return null;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};