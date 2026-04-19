using System;
using Loopie;

public class PlayerAnimation : PlayerComponent
{
    private Animator modelAnimator;

    public string idleClipName = "idle";
    public string walkClipName = "walk";
    public string dashIdleClipName = "DashFromIdle";
    public string dashWalkClipName = "DashFromWalk";
    public string attack1Clip = "Attack1";
    public string attack2Clip = "Attack2";
    public string attack3Clip = "Attack3";
    public string grappleShootClip = "GrapleShoot";
    public string grapplePoseClip = "GraplePose";
    public string grappleLandingClip = "GrapleLanding";

    private enum AnimationState { 
        IDLE,
        WALK, 
        DASH, 
        ATTACK, 
        GRAPPLE_SHOOT, 
        GRAPPLE_FLIGHT, 
        GRAPPLE_LAND, 
        NULL 
    };
    private AnimationState state;
    private int lastPlayedComboIndex = 0;

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

        if (player.Grapple.IsLaunching)
        {
            PlayGrappleAnim(grappleShootClip, AnimationState.GRAPPLE_SHOOT, false);
        }
        else if (player.Grapple.IsGrappling)
        {
            PlayGrappleAnim(grapplePoseClip, AnimationState.GRAPPLE_FLIGHT, true);
        }
        else if (player.Grapple.IsLanding)
        {
            PlayGrappleAnim(grappleLandingClip, AnimationState.GRAPPLE_LAND, false);
        }

        if (player.Combat.isAttacking)
        {
            Attack();
        }
        else if (player.Movement.IsDashing())
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
        if (state == AnimationState.IDLE) return;

        state = AnimationState.IDLE;
        lastPlayedComboIndex = 0;
        modelAnimator.Play(idleClipName, 0.2f);
        modelAnimator.Looping = true;
    }

    private void Move()
    {
        if (state == AnimationState.WALK) return;

        state = AnimationState.WALK;
        lastPlayedComboIndex = 0;
        modelAnimator.Play(walkClipName, 0.2f);
        modelAnimator.Looping = true;
    }

    private void Dash()
    {
        if (state == AnimationState.DASH) return;

        string clip = (state == AnimationState.WALK) ? dashWalkClipName : dashIdleClipName;
        state = AnimationState.DASH;
        lastPlayedComboIndex = 0;

        modelAnimator.Play(clip, 0.1f);
        modelAnimator.Looping = false;
    }

    private void Attack()
    {
        int currentCombo = player.Combat.GetCurrentComboIndex();

        if (state != AnimationState.ATTACK || lastPlayedComboIndex != currentCombo)
        {
            state = AnimationState.ATTACK;
            lastPlayedComboIndex = currentCombo;

            string clipToPlay = attack1Clip;
            if (currentCombo == 2) clipToPlay = attack2Clip;
            else if (currentCombo == 3) clipToPlay = attack3Clip;

            modelAnimator.Play(clipToPlay, 0.05f);
            modelAnimator.Looping = true;
        }
    }
    private void PlayGrappleAnim(string clip, AnimationState newState, bool loop)
    {
        if (state == newState) return;
        state = newState;
        modelAnimator.Play(clip, 0.1f);
        modelAnimator.Looping = loop;
    }
}