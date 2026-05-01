using System;
using Loopie;

public class SkillsHUD : Component
{
    public Entity torchActiveEntity;
    public Entity torchInactiveEntity;
    public Entity grappleActiveEntity;
    public Entity grappleInactiveEntity;

    private PlayerTorch playerTorch;
    private PlayerGrapple playerGrapple;

    private bool wasTorchReady = true;
    private bool wasGrappleReady = true;

    private float simulatedGrappleCooldown = 0.0f;
    private float lastGrappleTimerValue = 0.0f;

    void OnCreate()
    {

        playerTorch = Player.Instance.Torch;
        playerGrapple = Player.Instance.Grapple;    

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

        if (playerGrapple != null && grappleActiveEntity != null && grappleInactiveEntity != null && DatabaseRegistry.playerDB != null)
        {
            if (DatabaseRegistry.playerDB.Player.hasGrappling)
            {
                bool isGrappleReady = !playerGrapple.IsGrappling;

                if (isGrappleReady != wasGrappleReady)
                {
                    grappleActiveEntity.SetActive(isGrappleReady);
                    grappleInactiveEntity.SetActive(!isGrappleReady);
                    wasGrappleReady = isGrappleReady;
                }
            }
            else
            {
                if (wasGrappleReady)
                {
                    grappleActiveEntity.SetActive(false);
                    grappleInactiveEntity.SetActive(true);
                    wasGrappleReady = false;
                }
            }
        }
    }
};