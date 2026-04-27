using System;
using Loopie;

public class SkillsHUD : Component
{
    public string playerName = "Player";

    public string torchActiveName = "Torch_Active";
    public string torchInactiveName = "Torch_Inactive";

    public string grappleActiveName = "Grapple_Active";
    public string grappleInactiveName = "Grapple_Inactive";

    private PlayerTorch playerTorch;
    private PlayerGrapple playerGrapple;

    private Entity torchActiveEntity;
    private Entity torchInactiveEntity;
    private Entity grappleActiveEntity;
    private Entity grappleInactiveEntity;

    private bool wasTorchReady = true;
    private bool wasGrappleReady = true;

    private float simulatedGrappleCooldown = 0.0f;
    private float lastGrappleTimerValue = 0.0f;

    void OnCreate()
    {
        Entity player = Entity.FindEntityByName(playerName);
        if (player != null)
        {
            playerTorch = player.GetComponent<PlayerTorch>();
            playerGrapple = player.GetComponent<PlayerGrapple>();
        }

        torchActiveEntity = Entity.FindEntityByName(torchActiveName);
        torchInactiveEntity = Entity.FindEntityByName(torchInactiveName);

        grappleActiveEntity = Entity.FindEntityByName(grappleActiveName);
        grappleInactiveEntity = Entity.FindEntityByName(grappleInactiveName);

        if (torchInactiveEntity != null) torchInactiveEntity.SetActive(false);
        if (grappleInactiveEntity != null) grappleInactiveEntity.SetActive(false);
    }

    void OnUpdate()
    {

        if (playerTorch != null && torchActiveEntity != null && torchInactiveEntity != null && DatabaseRegistry.playerDB != null)
        {
            if (DatabaseRegistry.playerDB.Player.hasBurner)
            {
                bool isTorchReady = !playerTorch.IsTorching;

                if (isTorchReady != wasTorchReady)
                {
                    torchActiveEntity.SetActive(isTorchReady);
                    torchInactiveEntity.SetActive(!isTorchReady);
                    wasTorchReady = isTorchReady;
                }
            }
            else
            {
                if (wasTorchReady)
                {
                    torchActiveEntity.SetActive(false);
                    torchInactiveEntity.SetActive(true);
                    wasTorchReady = false;
                }
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
                isNewGrapple = true;
            else if (lastGrappleTimerValue == 0.0f && currentGrappleTimer > 0.0f)
                isNewGrapple = true;

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