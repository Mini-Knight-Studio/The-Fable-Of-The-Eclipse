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

    private Entity idleMesh;
    private Entity walkMesh;
    private Entity dashMesh;

    public void OnCreate()
    {
        playerMovement = entity.GetComponent<PlayerMovement>();
        animator = entity.GetComponent<Animator>();

        idleMesh = Entity.FindEntityByName("brightwhiskers_idle");
        walkMesh = Entity.FindEntityByName("brightwhiskers_walk");
        dashMesh = Entity.FindEntityByName("brightwhiskers_dash");
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
        idleMesh.SetActive(true);
        walkMesh.SetActive(false);
        dashMesh.SetActive(false);
    }

    private void Move()
    {
        animator.Play(walkClipName);
        Console.WriteLine("I'm moving");
        toWalk = false;
        toIdle = true;
        idleMesh.SetActive(false);
        walkMesh.SetActive(true);
        dashMesh.SetActive(false);
    }

    private void Dash()
    {
        animator.Play(walkClipName);
        Console.WriteLine("I'm dashing");
        toDash = false;
        toWalk = true;
        toIdle = true;
        idleMesh.SetActive(false);
        walkMesh.SetActive(false);
        dashMesh.SetActive(true);
    }
};