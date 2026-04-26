using System;
using System.Collections;
using Loopie;

public class SkillsHUD : Component
{
    public Entity torchActiveEntity;
    public Entity torchInactiveEntity;
    public Entity grappleActiveEntity;
    public Entity grappleInactiveEntity;

    private bool wasDashReady = true;
    private bool wasGrappleReady = true;

    void OnCreate()
    {
        if (torchInactiveEntity != null) torchInactiveEntity.SetActive(false);
        if (grappleInactiveEntity != null) grappleInactiveEntity.SetActive(false);
    }

    void OnUpdate()
    {
        if (Player.Instance.Movement != null && torchActiveEntity != null && torchInactiveEntity != null)
        {
            bool isTorchReady = !Player.Instance.Torch.IsTorching;
            if (isTorchReady != wasDashReady)
            {
                torchActiveEntity.SetActive(isTorchReady);
                torchInactiveEntity.SetActive(!isTorchReady);
                wasDashReady = isTorchReady;
            }
        }

        if (Player.Instance.Grapple != null && grappleActiveEntity != null && grappleInactiveEntity != null)
        {
            bool isGrappleReady = !Player.Instance.Grapple.IsGrappling;

            if (isGrappleReady != wasGrappleReady)
            {
                grappleActiveEntity.SetActive(isGrappleReady);
                grappleInactiveEntity.SetActive(!isGrappleReady);
                wasGrappleReady = isGrappleReady;
            }
        }
    }
};