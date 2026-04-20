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
    public string TorchClip= "Torch";

    private enum AnimationState { 
        IDLE,
        WALK, 
        DASH, 
        ATTACK, 
        GRAPPLE_SHOOT, 
        GRAPPLE_FLIGHT, 
        GRAPPLE_LAND, 
        TORCH_BURN,
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
        if (player.Movement == null || player.Combat == null || player.Grapple == null) return;


        if (player.Torch != null && player.Torch.IsTorching)
        {
            if (state != AnimationState.TORCH_BURN)
            {
                state = AnimationState.TORCH_BURN;
                modelAnimator.Play(TorchClip, 0.1f);
                modelAnimator.Looping = false;
            }
        }
        else if (player.Grapple.IsLaunching)
        {
            if (state != AnimationState.GRAPPLE_SHOOT) Debug.Log("ANIM: Trying to play SHOOT: " + grappleShootClip);
            PlayGrappleAnim(grappleShootClip, AnimationState.GRAPPLE_SHOOT, false);
        }
        else if (player.Grapple.IsGrappling)
        {
            if (state != AnimationState.GRAPPLE_FLIGHT) Debug.Log("ANIM: Trying to play POSE: " + grapplePoseClip);
            PlayGrappleAnim(grapplePoseClip, AnimationState.GRAPPLE_FLIGHT, true);
        }
        else if (player.Grapple.IsLanding)
        {
            if (state != AnimationState.GRAPPLE_LAND) Debug.Log("ANIM: Trying to play LANDING: " + grappleLandingClip);
            PlayGrappleAnim(grappleLandingClip, AnimationState.GRAPPLE_LAND, false);
        }
        else if (player.Combat.isAttacking)
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

    private void PlayGrappleAnim(string clip, AnimationState newState, bool loop)
    {
        if (state == newState) return;
        state = newState;

        Debug.Log("ANIM FIRE: " + clip); 

        modelAnimator.Play(clip, 0.1f);
        modelAnimator.Looping = loop;
    }
    private void Idle()
    {
        if (state == AnimationState.IDLE) return;

        state = AnimationState.IDLE;
        lastPlayedComboIndex = 0;
        modelAnimator.Play(idleClipName, 0.4f);
        modelAnimator.Looping = true;
    }

    private void Move()
    {
        if (state == AnimationState.WALK) return;

        state = AnimationState.WALK;
        lastPlayedComboIndex = 0;
        modelAnimator.Play(walkClipName, .4f);
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

            modelAnimator.Play(clipToPlay, .1f);
            modelAnimator.Looping = false;
        }
    }
 
}