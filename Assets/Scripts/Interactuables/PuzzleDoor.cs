using System;
using System.Collections;
using Loopie;

class PuzzleDoor : Component
{
    [Header("References")]
    public Entity rightDoor;
    public Entity leftDoor;
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

    }

    void OnUpdate()
    {

    }

    IEnumerator OpenDoors()
    {
        collected = true;
        Entity player = Player.Instance.entity;
        Vector3 initialPosition = transform.position;
        Vector3 initialScale = transform.scale;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(initialPosition, player.transform.position + new Vector3(0, 2, 0), Mathf.Clamp01(timer / collectTime));
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

        if (focusPointOnCollect != null)
        {
            Player.Instance.Camera.FocusOnPoint(focusPointOnCollect.transform.position, 15, 7);

            yield return new WaitForSeconds(4);

            Player.Instance.Camera.StopFocus();
        }


        yield return null;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};