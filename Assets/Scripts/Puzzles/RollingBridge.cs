using System;
using Loopie;

class RollingBridge : Component
{
    private BoxCollider collider;
    public Entity bridgeBase;
    public Entity blockingCollider;

    public Loopie.Vector3 finalPos;
    public Loopie.Vector3 finalRotation = Loopie.Vector3.Zero;

    public Loopie.Vector3 standingPos;
    public Loopie.Vector3 standingRotation;

    private float t = 0f;
    public float speed = 1.5f;

    private bool animationFinished = false;
    private bool animationStarted = false;

    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (animationFinished) return;

        if(DatabaseRegistry.puzzlesDB.Puzzles.BridgePushedDown == true)
        {
            bridgeBase.transform.local_position = finalPos;
            bridgeBase.transform.local_rotation = finalRotation;
            blockingCollider.GetComponent<BoxCollider>().SetActive(false);
            animationFinished = true;
        }

        if (!animationFinished && collider.IsColliding && Player.Instance.Movement.IsDashing())
        {
            animationStarted = true;
        }

        if (!animationFinished && animationStarted)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        if (animationFinished) return;

        t += Time.deltaTime * speed;
        if (t > 1f)
        {
            t = 1f;
            animationFinished = true;
            blockingCollider.GetComponent<BoxCollider>().SetActive(false);
            DatabaseRegistry.puzzlesDB.Puzzles.BridgePushedDown = true;
        }

        Loopie.Vector3 pos = Loopie.Vector3.Lerp(standingPos, finalPos, t);
        Loopie.Vector3 rot = Loopie.Vector3.Lerp(standingRotation, finalRotation, t);

        bridgeBase.transform.local_position = pos;
        bridgeBase.transform.local_rotation = rot;
    }
}
