using System;
using Loopie;

class MovingPillar : Component
{
    private BoxCollider upCollider;
    private BoxCollider downCollider;
    private BoxCollider rightCollider;
    private BoxCollider leftCollider;

    public string upColliderName;
    public string downColliderName;
    public string rightColliderName;
    public string leftColliderName;

    public float collisionCooldown = 2.0f;
    private float collisionTimer = 0.0f;

    public float movementSpeed = 2.0f;

    void OnCreate()
    {
        upCollider = Entity.FindEntityByName(upColliderName).GetComponent<BoxCollider>();
        downCollider = Entity.FindEntityByName(downColliderName).GetComponent<BoxCollider>();
        rightCollider = Entity.FindEntityByName(rightColliderName).GetComponent<BoxCollider>();
        leftCollider = Entity.FindEntityByName(leftColliderName).GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        HandleCollision();
    }

    void HandleCollision()
    {
        if (collisionTimer < collisionCooldown)
        {
            collisionTimer++;
            return;
        }

        if (upCollider.HasCollided)
        {
            HandleUpCollision();
        }
        else if (downCollider.HasCollided)
        {
            HandleDownCollision();
        }
        else if (rightCollider.HasCollided)
        {
            HandleRightCollision();
        }
        else if (leftCollider.HasCollided)
        {
            HandleLeftCollision();
        }

        collisionTimer++;
    }

    void HandleUpCollision()
    {

        collisionTimer = 0;
    }

    void HandleDownCollision()
    {

        collisionTimer = 0;
    }

    void HandleRightCollision()
    {

        collisionTimer = 0;
    }

    void HandleLeftCollision()
    {

        collisionTimer = 0;
    }
};