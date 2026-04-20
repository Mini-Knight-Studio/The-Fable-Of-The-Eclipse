using System;
using System.Collections;
using Loopie;

public class SkillsHUD : Component
{
    public string playerName = "Player";

    public string dashActiveName = "Dash_Active";
    public string dashInactiveName = "Dash_Inactive";

    public string grappleActiveName = "Grapple_Active";
    public string grappleInactiveName = "Grapple_Inactive";

    private PlayerMovement playerMovement;
    private PlayerGrapple playerGrapple;

    private Entity dashActiveEntity;
    private Entity dashInactiveEntity;
    private Entity grappleActiveEntity;
    private Entity grappleInactiveEntity;

    private bool wasDashReady = true;
    private bool wasGrappleReady = true;

    private float simulatedGrappleCooldown = 0.0f; 
    private float lastGrappleTimerValue = 0.0f;    

    void OnCreate()
    {
        Entity player = Entity.FindEntityByName(playerName);
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerGrapple = player.GetComponent<PlayerGrapple>();
        }

        dashActiveEntity = Entity.FindEntityByName(dashActiveName);
        dashInactiveEntity = Entity.FindEntityByName(dashInactiveName);

        grappleActiveEntity = Entity.FindEntityByName(grappleActiveName);
        grappleInactiveEntity = Entity.FindEntityByName(grappleInactiveName);

        if (dashInactiveEntity != null) dashInactiveEntity.SetActive(false);
        if (grappleInactiveEntity != null) grappleInactiveEntity.SetActive(false);
    }

    void OnUpdate()
    {
        if (playerMovement != null && dashActiveEntity != null && dashInactiveEntity != null)
        {
            bool isDashReady = playerMovement.dashCooldownTimer <= 0;
            if (isDashReady != wasDashReady)
            {
                dashActiveEntity.SetActive(isDashReady);
                dashInactiveEntity.SetActive(!isDashReady);
                wasDashReady = isDashReady;
            }
        }

        if (playerGrapple != null && grappleActiveEntity != null && grappleInactiveEntity != null)
        {
            if (simulatedGrappleCooldown > 0)
            {
                simulatedGrappleCooldown -= Time.deltaTime;
            }

            float currentGrappleTimer = playerGrapple.grappleCooldownTimer;

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
                simulatedGrappleCooldown = playerGrapple.grappleCooldown;
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