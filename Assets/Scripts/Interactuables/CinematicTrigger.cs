using Loopie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class CinematicTrigger : Component
{
    [Header("Settings")]
    public bool playMoreThanOnce;
    [Header("Typing")]
    public Entity cinematicEntity;

    [Header("References")]
    public Entity interactPrompt;

    BoxCollider boxCollider;
    bool canBePlayed = true;

    void OnCreate()
    {
        boxCollider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (CinematicUI.Instance.IsCinematicOpen || !canBePlayed)
        {
            if (interactPrompt.Active)
                interactPrompt.SetActive(false);
            return;
        }

        if (boxCollider.IsColliding)
        {
            if (!interactPrompt.Active)
            {
                interactPrompt.SetActive(true);
            }

            if (Player.Instance.Input.interactKeyPressed && canBePlayed)
            {
                Open();
                if (!playMoreThanOnce)
                {
                    canBePlayed = false;
                }
            }
        }
        else
        {
            if (interactPrompt.Active)
            {
                interactPrompt.SetActive(false);
            }
        }

    }

    private void Open()
    {
        CinematicUI.Instance.SetUpCinematic(cinematicEntity);
        CinematicUI.Instance.StartCinematic();
    }
};