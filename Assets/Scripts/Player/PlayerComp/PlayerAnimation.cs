using System;
using Loopie;

public class PlayerAnimation : PlayerComponent
{
    private Animator idleAnimator;
    private Animator walkAnimator;
    private Animator dashAnimator;
    private Animator attackAnimator;

    private bool toIdle = true;
    private bool toWalk = true;
    private bool toDash = true;
    private bool toAttack = true;

    public string idleClipName = "idle";
    public string walkClipName = "walk";
    public string dashClipName = "dash";
    public string attackClipName = "attack";

    public Entity idleEntity;
    public Entity walkEntity;
    public Entity dashEntity;
    public Entity attackEntity;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackDuration = 0.2f;

    public void OnCreate()
    {
        idleAnimator = idleEntity.GetComponent<Animator>();
        walkAnimator = walkEntity.GetComponent<Animator>();
        dashAnimator = dashEntity.GetComponent<Animator>();
        attackAnimator = attackEntity.GetComponent<Animator>();
    }

    public void ProcessAnimations()
    {
        if (player.Movement == null || player.Combat == null) return;

        if (player.Combat.TemporalFunctionIsAttacking() && toAttack)
        {
            toAttack = false;
            Attack();
            return;
        }

        if (!player.Combat.TemporalFunctionIsAttacking())
        {
            toAttack = true;
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                isAttacking = false;

                attackEntity.SetActive(false);

                toIdle = true;
                toWalk = true;
                toDash = true;

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

            return;
        }

        bool isDashing = player.Movement.IsDashing();
        if (isDashing && toDash)
        {
            Dash();
            return;
        }

        if (!isDashing)
        {
            toDash = true;

            bool isMoving = player.Movement.IsMoving();
            if (isMoving && toWalk)
            {
                Move();
            }
            else if (!isMoving && toIdle)
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
        attackEntity.SetActive(false);
        idleAnimator.Play(idleClipName);
        idleAnimator.Looping = true;
    }

    private void Move()
    {
        toWalk = false;
        toIdle = true;
        idleEntity.SetActive(false);
        walkEntity.SetActive(true);
        dashEntity.SetActive(false);
        attackEntity.SetActive(false);
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
        attackEntity.SetActive(false);
        dashAnimator.Play(dashClipName);
        dashAnimator.Looping = false;
    }

    private void Attack()
    {
        isAttacking = true;

        toDash = true;
        toWalk = true;
        toIdle = true;

        idleEntity.SetActive(false);
        walkEntity.SetActive(false);
        dashEntity.SetActive(false);
        attackEntity.SetActive(true);

        attackAnimator.Play(attackClipName);
        attackAnimator.Looping = false;

        attackTimer = attackDuration;
    }
};