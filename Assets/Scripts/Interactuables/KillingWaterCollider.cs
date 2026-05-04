using System;
using Loopie;

class KillingWaterCollider : Component
{
    private BoxCollider collider;

    public float killTime = 3.0f;
    private float killTimer = 0.0f;

    private float rayDistance = 5.0f;

    public int fallingDamage = 1;
    void OnCreate()
    {
        collider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (Pause.isPaused) { return; }

        Entity player = Player.Instance.entity;
        BoxCollider playerCollider = player.GetComponent<BoxCollider>();

        Vector3 playerCenter = player.transform.position + playerCollider.LocalCenter;
        Vector3 playerScale = player.transform.scale;

        Vector3 worldExtents = new Vector3(playerCollider.LocalExtents.x * playerScale.x, playerCollider.LocalExtents.y * playerScale.y, playerCollider.LocalExtents.z * playerScale.z);

        Vector3 origin = playerCenter;
        origin.y -= worldExtents.y + 0.02f;

        Vector3 direction = transform.Down.normalized;

        RaycastHit hit;
        bool didHit = Collisions.Raycast(origin, direction, rayDistance, out hit, playerCollider, Collisions.GetLayerBit("PlayerRaycast"));

        if (didHit && hit.collider == collider)
        {
            killTimer += Time.deltaTime;

            if (!Player.Instance.Movement.isGodMode && !Player.Instance.Grapple.IsGrappling && !Player.Instance.Movement.IsDashing() && Player.Instance.Movement.gravityActive)
            {
                Player.Instance.Movement.gravityActive = false;
            }

            if (killTimer >= killTime)
            {
                if (!Player.Instance.Movement.isGodMode && !Player.Instance.Grapple.IsGrappling && !Player.Instance.Movement.IsDashing())
                {
                    Player.Instance.PlayerHealth.Damage(fallingDamage);
                    Player.Instance.StartRespawn();
                    Player.Instance.Movement.gravityActive = true;
                }
                killTimer = 0.0f;
            }
        }
        else
        {
            killTimer = 0.0f;
        }
    }

};