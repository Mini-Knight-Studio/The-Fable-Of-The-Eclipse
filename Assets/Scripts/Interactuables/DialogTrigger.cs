using Loopie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class DialogTrigger : Component
{
    [Header("Values")]
    [TextArea]
    public string textValue = "";
    public string nextCharSeparator = "";

    [Header("Typing")]
    public float characterDelay = 0.03f;

    [Header("References")]
    public Entity interactPrompt;

    private InteractHover interactPromptComponent;

    BoxCollider boxCollider;

    void OnCreate()
    {
        boxCollider = entity.GetComponent<BoxCollider>();

        interactPromptComponent = interactPrompt.GetComponent<InteractHover>();
    }

    void OnUpdate()
    {
        if (DialogUI.Instance.IsDialogOpen)
        {
            interactPromptComponent.DeactivatePromt();
            return;
        }

        if (boxCollider.IsColliding)
        {
            if (!interactPrompt.Active)
            {
                interactPromptComponent.ActivatePromt();
            }

            if (Player.Instance.Input.interactKeyPressed)
            {
                Open();
            }
        }
        else
        {
            if (interactPrompt.Active)
            {
                interactPromptComponent.DeactivatePromt();
            }
        }

    }

    private void Open()
    {
        DialogUI.Instance.SetText(textValue, nextCharSeparator);
        DialogUI.Instance.StartReading(characterDelay);
    }
};