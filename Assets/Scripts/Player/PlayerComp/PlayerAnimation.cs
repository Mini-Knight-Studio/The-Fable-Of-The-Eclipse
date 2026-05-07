using System;
using Loopie;

public class PlayerAnimation : PlayerComponent
{
    private Animator modelAnimator;

    public string idleClipName = "";
    public string walkClipName = "";
    public string dashIdleClipName = "";
    public string dashWalkClipName = "";
    public string attack1Clip = "";
    public string attack2Clip = "";
    public string attack3Clip = "";
    public string grappleShootClip = "";
    public string grapplePoseClip = "";
    public string grappleLandingClip = "";
    public string torchClip = "";
    public string onHitClip = "";
    public string pickUpClip = "";

    private enum AnimationState
    {
        IDLE,
        WALK,
        DASH,
        ATTACK,
        GRAPPLE_SHOOT,
        GRAPPLE_FLIGHT,
        GRAPPLE_LAND,
        TORCH_BURN,
        HIT,
        PICKUP,
        NULL
    };
    private AnimationState state;
    private int lastPlayedComboIndex = 0;

    public Entity modelEntity;
    public Entity swordEntity;

    private float loopDelayTimer = 0f;
    private bool isWaitingForLoop = false;
    private float hitTimer = 0f;

    public void OnCreate()
    {
        state = AnimationState.NULL;
        modelAnimator = modelEntity.GetComponent<Animator>();
        Idle();
    }

    public void ProcessAnimations()
    {
        if (player.Movement == null || player.Combat == null || player.Grapple == null || player.Torch == null) return;

        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
            modelAnimator.Looping = false;

            if (state == AnimationState.HIT)
            {
                if (hitTimer > 0.1f) return;
            }
            else 
            {
                return;
            }
        }

        if (state != AnimationState.IDLE)
        {
            isWaitingForLoop = false;
        }

        if (isWaitingForLoop)
        {
            loopDelayTimer -= Time.deltaTime;
            if (loopDelayTimer <= 0f)
            {
                modelAnimator.Looping = true;
                isWaitingForLoop = false;
            }
        }

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
        else if (player.Torch.IsTorching)
        {
            Torch();
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

        if (state != AnimationState.ATTACK)
            swordEntity.SetActive(false);
    }

    public void PlayHit()
    {
        state = AnimationState.HIT;
        lastPlayedComboIndex = 0;
        isWaitingForLoop = false;

        modelAnimator.Looping = false;
        modelAnimator.Play(onHitClip, 0.05f);
        hitTimer = 0.5f;
    }

    public void PlayPickUp()
    {
        state = AnimationState.PICKUP;
        lastPlayedComboIndex = 0;
        isWaitingForLoop = false;

        modelAnimator.Looping = false;
        modelAnimator.Play(pickUpClip, 0.1f);
        hitTimer = 1.0f;
    }

    private void PlayGrappleAnim(string clip, AnimationState newState, bool loop)
    {
        if (state == newState) return;
        state = newState;
        lastPlayedComboIndex = 0;
        modelAnimator.Play(clip, 0.1f);
        modelAnimator.Looping = loop;
    }

    private void Idle()
    {
        if (state == AnimationState.IDLE) return;
        state = AnimationState.IDLE;
        lastPlayedComboIndex = 0;

        modelAnimator.Looping = false;
        modelAnimator.Play(idleClipName, 0.4f);

        isWaitingForLoop = true;
        loopDelayTimer = 0.4f;
    }

    private void Move()
    {
        if (state == AnimationState.WALK) return;
        state = AnimationState.WALK;
        lastPlayedComboIndex = 0;
        modelAnimator.Looping = true;
        modelAnimator.Play(walkClipName, 0.4f);
    }

    private void Dash()
    {
        if (state == AnimationState.DASH) return;
        string clip = (state == AnimationState.WALK) ? dashWalkClipName : dashIdleClipName;
        state = AnimationState.DASH;
        lastPlayedComboIndex = 0;
        modelAnimator.Looping = false;
        modelAnimator.Play(clip, 0.0f);
    }

    private void Torch()
    {
        if (state == AnimationState.TORCH_BURN) return;
        state = AnimationState.TORCH_BURN;
        lastPlayedComboIndex = 0;
        modelAnimator.Looping = false;
        modelAnimator.Play(torchClip, 0.0f);
    }

    private void Attack()
    {
        swordEntity.SetActive(true);
        int currentCombo = player.Combat.GetCurrentComboIndex();
        if (state != AnimationState.ATTACK || lastPlayedComboIndex != currentCombo)
        {
            state = AnimationState.ATTACK;
            lastPlayedComboIndex = currentCombo;
            string clipToPlay = attack1Clip;
            if (currentCombo == 2) clipToPlay = attack2Clip;
            else if (currentCombo == 3) clipToPlay = attack3Clip;
            modelAnimator.Looping = false;
            modelAnimator.Play(clipToPlay, 0.0f);
        }
    }
}