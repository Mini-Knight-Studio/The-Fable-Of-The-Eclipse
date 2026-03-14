using System;
using Loopie;

class PlayerAnimation : Component
{
    private PlayerMovement playerMovement;
    private Animator idleAnimator;
    private Animator walkAnimator;

    private bool toIdle = true;
    private bool toWalk = true;
    private bool toDash = true;

    public string idleClipName = "idle";
    public string walkClipName = "walk";
    public string dashClipName = "dash";

    private Entity idleEntity;
    private Entity walkEntity;
    private Entity dashEntity;

    public void OnCreate()
    {
        playerMovement = entity.GetComponent<PlayerMovement>();

        idleEntity = Entity.FindEntityByName("IdlePlayer");
        walkEntity = Entity.FindEntityByName("WalkPlayer");
        dashEntity = Entity.FindEntityByName("WalkPlayer");

        idleAnimator = idleEntity.GetComponent<Animator>();
        walkAnimator = walkEntity.GetComponent<Animator>();
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
        toIdle = false;
        toWalk = true;
        idleEntity.SetActive(true);
        walkEntity.SetActive(false);
        dashEntity.SetActive(false);
        idleAnimator.Play(idleClipName);
        idleAnimator.Looping = true;
    }

    private void Move()
    {
        toWalk = false;
        toIdle = true;
        idleEntity.SetActive(false);
        walkEntity.SetActive(true);
        dashEntity.SetActive(true);
        walkAnimator.Play(walkClipName);
        walkAnimator.Looping = true;
    }

    private void Dash()
    {
        toDash = false;
        toWalk = true;
        toIdle = true;
        idleEntity.SetActive(false);
        walkEntity.SetActive(false);
        dashEntity.SetActive(true);
        walkAnimator.Play(dashClipName);
        walkAnimator.Looping = false;
    }
};