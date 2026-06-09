using Loopie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class CinematicTrigger : Component
{
    [Header("Settings")]
    public bool playMoreThanOnce;
    public bool playOnStart;

    public float fadeInTime = 0.5f;
    public float fadeOutTime = 0.5f;

    [Header("Cinematics")]
    public string cinematicID;

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

    void OnPostCreate()
    {
        if (!playMoreThanOnce)
        {
            if (DatabaseRegistry.levelsDB.Levels.IsCinematicDone(cinematicID))
            {
                canBePlayed = false;
            }
        }

        if (playOnStart)
        {
            Open();
        }
    }

    void OnUpdate()
    {
        if (boxCollider == null)
            return;

        if (CinematicUI.Instance.IsCinematicOpen || !canBePlayed)
        {
            if (interactPrompt != null && interactPrompt.Active)
                interactPrompt.SetActive(false);
            return;
        }

        if (boxCollider.IsColliding)
        {
            if (interactPrompt != null && !interactPrompt.Active)
            {
                interactPrompt.SetActive(true);
            }

            if (Player.Instance.Input.interactKeyPressed && canBePlayed)
            {
                Open();           
            }
        }
        else
        {

            if (interactPrompt!=null && interactPrompt.Active)
            {
                interactPrompt.SetActive(false);
            }
        }

    }

    private void Open()
    {
        if (!canBePlayed)
            return;

        CinematicUI.Instance.SetUpCinematic(cinematicEntity, fadeInTime, fadeOutTime);
        CinematicUI.Instance.StartCinematic();

        if (!playMoreThanOnce)
        {
            canBePlayed = false;

            DatabaseRegistry.levelsDB.Levels.SetCinematicDone(cinematicID);
        }
    }
};