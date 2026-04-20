using System;
using System.Collections;
using Loopie;

public class SkillsHUD : Component
{
    public Entity dashActiveEntity;
    public Entity dashInactiveEntity;
    public Entity grappleActiveEntity;
    public Entity grappleInactiveEntity;

    private bool wasDashReady = true;
    private bool wasGrappleReady = true;

    private float simulatedGrappleCooldown = 0.0f; 
    private float lastGrappleTimerValue = 0.0f;    

    void OnCreate()
    {
        if (dashInactiveEntity != null) dashInactiveEntity.SetActive(false);
        if (grappleInactiveEntity != null) grappleInactiveEntity.SetActive(false);
    }

    void OnUpdate()
    {
        if (Player.Instance.Movement != null && dashActiveEntity != null && dashInactiveEntity != null)
        {
            bool isDashReady = Player.Instance.Movement.dashCooldownTimer <= 0;
            if (isDashReady != wasDashReady)
            {
                dashActiveEntity.SetActive(isDashReady);
                dashInactiveEntity.SetActive(!isDashReady);
                wasDashReady = isDashReady;
            }
        }

        if (Player.Instance.Grapple != null && grappleActiveEntity != null && grappleInactiveEntity != null)
        {
            if (simulatedGrappleCooldown > 0)
            {
                simulatedGrappleCooldown -= Time.deltaTime;
            }

            float currentGrappleTimer = Player.Instance.Grapple.grappleCooldownTimer;

            bool isNewGrapple = false;

            if (currentGrappleTimer < lastGrappleTimerValue)
            {
                isNewGrapple = true;
            }
            else if (lastGrappleTimerValue == 0.0f && currentGrappleTimer > 0.0f)
            {
                isNewGrapple = true;
            }

            if (isNewGrapple && simulatedGrappleCooldown <= 0)
            {
                simulatedGrappleCooldown = Player.Instance.Grapple.grappleCooldown;
            }

            lastGrappleTimerValue = currentGrappleTimer;

            bool isGrappleReady = simulatedGrappleCooldown <= 0;

            if (isGrappleReady != wasGrappleReady)
            {
                grappleActiveEntity.SetActive(isGrappleReady);
                grappleInactiveEntity.SetActive(!isGrappleReady);
                wasGrappleReady = isGrappleReady;
            }
        }
    }
};