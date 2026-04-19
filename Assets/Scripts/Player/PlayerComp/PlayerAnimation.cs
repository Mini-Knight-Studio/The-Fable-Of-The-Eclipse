using System;
using Loopie;

public class PlayerAnimation : PlayerComponent
{
    private Animator modelAnimator;

    public string idleClipName = "idle";
    public string walkClipName = "walk";
    public string dashClipName = "dash";
    public string attackClipName = "attack";
    public string dashFromWalkClipName = "dashFromWalk";
    public string dashFromIdleClipName = "dashFromIdle";

    private enum AnimationState
    {
        IDLE,
        WALK,
        DASH,
        NULL

    };

    private AnimationState state;
    public Entity modelEntity;

    public void OnCreate()
    {
        state = AnimationState.NULL;
        modelAnimator = modelEntity.GetComponent<Animator>();
        Idle();
    }

    public void ProcessAnimations()
    {
        if (player.Movement == null || player.Combat == null) return;

        if (player.Movement.IsDashing())
        {
            Dash();
        }
        else if (player.Movement.IsMoving())
        {
            Move();
        }
        else
        {
            Idle();
        }



    }

    private void Idle()
    {
        if(state == AnimationState.IDLE) { return; }
        state = AnimationState.IDLE;
        modelAnimator.Play(idleClipName, .2f);
        modelAnimator.Looping = true;

        Debug.Log("idle");
    }

    private void Move()
    {
        if (state == AnimationState.WALK) { return; }

        state = AnimationState.WALK;
        modelAnimator.Play(walkClipName, .2f);
        modelAnimator.Looping = true;
        Debug.Log("Move");

    }

    private void Dash()
    {
        if (state == AnimationState.DASH) { return; }

        string clipToPlay = (state == AnimationState.WALK) ? dashFromWalkClipName : dashFromIdleClipName;

        state = AnimationState.DASH;

        modelAnimator.Play(clipToPlay, 0.1f);
        modelAnimator.Looping = false;

        Debug.Log("Dashing with: " + clipToPlay);
    }

    private void Attack()
    {
        
    }
};