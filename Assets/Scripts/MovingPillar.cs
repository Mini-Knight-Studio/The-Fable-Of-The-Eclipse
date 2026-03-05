using System;
using Loopie;

class MovingPillar : Component
{
    private BoxCollider boxCollider;
    public float collisionCooldown = 2.0f;
    private float collisionTimer = 0.0f;

    public float movementSpeed = 2.0f;

    void OnCreate()
    {
        boxCollider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        HandleCollsion();
    }

    void HandleCollsion()
    {
        if (boxCollider.HasCollided && collisionTimer >= collisionCooldown)
        {
            //logic

            //check if collided with player
            //boxCollider.entity.transform

            collisionTimer = 0;
        }

        collisionTimer++;
    }
};