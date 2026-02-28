using System;
using System.Numerics;
using Loopie;

class PlayerAnimation : Component
{
    private PlayerMovement playerMovement;
    private Animator animator;

    private bool toIdle = true;
    private bool toWalk = true;
    private bool toDash = true;

    public string idleClipName = "idle";
    public string walkClipName = "walk";
    public string dashClipName = "dash";

    public void OnCreate()
    {
        playerMovement = entity.GetComponent<PlayerMovement>();
        animator = entity.GetComponent<Animator>();
    }

    public void OnUpdate()
    {
        if (playerMovement == null) return;

        if (playerMovement.isDashing && toDash)
        {
            Dash();
            return;
        }

        if (!playerMovement.isDashing)
        {
            toDash = true;
            if (playerMovement.isMoving && toWalk)
            {
                Move();
            }
            else if (!playerMovement.isMoving && toIdle)
            {
                Idle();
            }
        }
    }

    private void Idle()
    {
        animator.Play(idleClipName);
        Console.WriteLine("I'm idle");
        toIdle = false;
        toWalk = true;
    }

    private void Move()
    {
        animator.Play(walkClipName);
        Console.WriteLine("I'm moving");
        toWalk = false;
        toIdle = true;
    }

    private void Dash()
    {
        animator.Play(walkClipName);
        Console.WriteLine("I'm dashing");
        toDash = false;
        toWalk = true;
        toIdle = true;
    }
};