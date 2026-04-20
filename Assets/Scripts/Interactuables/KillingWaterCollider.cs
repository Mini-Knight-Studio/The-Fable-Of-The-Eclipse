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
        Entity player = Player.Instance.entity;
        BoxCollider playerCollider = player.GetComponent<BoxCollider>();

        Vector3 playerCenter = player.transform.position + playerCollider.LocalCenter;
        Vector3 playerScale = player.transform.scale;

        Vector3 worldExtents = new Vector3(playerCollider.LocalExtents.x * playerScale.x, playerCollider.LocalExtents.y * playerScale.y, playerCollider.LocalExtents.z * playerScale.z);

        Vector3 origin = playerCenter;
        origin.y -= worldExtents.y + 0.02f;

        Vector3 direction = transform.Down.normalized;

        RaycastHit hit;
        bool didHit = Collisions.Raycast(origin, direction, rayDistance, out hit, Collisions.GetLayerBit("PlayerRaycast"), playerCollider);

        if (didHit && hit.collider == collider)
        {
            killTimer += Time.deltaTime;

            if (killTimer >= killTime)
            {
                Player.Instance.PlayerHealth.Damage(fallingDamage);
                Player.Instance.StartRespawn();
                killTimer = 0.0f;
            }
        }
        else
        {
            killTimer = 0.0f;
        }
    }

};