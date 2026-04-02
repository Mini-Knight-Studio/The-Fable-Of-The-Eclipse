using System;
using Loopie;

public class PlayerAnimation : Component
{
    private Player player;

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
        player = entity.GetComponent<Player>();

        idleEntity = Entity.FindEntityByName("IdlePlayer");
        walkEntity = Entity.FindEntityByName("WalkPlayer");
        dashEntity = Entity.FindEntityByName("WalkPlayer");

        idleAnimator = idleEntity.GetComponent<Animator>();
        walkAnimator = walkEntity.GetComponent<Animator>();
    }

    public void OnUpdate()
    {
        if (player == null || player.Movement == null) return;

        if (player.Movement.isDashing && toDash)
        {
            Dash();
            return;
        }

        if (!player.Movement.isDashing)
        {
            toDash = true;
            if (player.Movement.isMoving && toWalk)
            {
                Move();
            }
            else if (!player.Movement.isMoving && toIdle)
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