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

    BoxCollider boxCollider;

    public event Action OnDialogStart;
    public event Action OnDialogEnd;

    void OnCreate()
    {
        boxCollider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (DialogUI.Instance.IsDialogOpen)
        {
            interactPrompt.SetActive(false);
            return;
        }

        if (boxCollider.IsColliding)
        {
            if (!interactPrompt.Active)
            {
                interactPrompt.SetActive(true);
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
                interactPrompt.SetActive(false);
            }
        }

    }

    private void Open()
    {
        DialogUI.Instance.SetText(textValue, nextCharSeparator);
        if(DialogUI.Instance.StartReading(characterDelay, this)) 
            OnDialogStart?.Invoke();
    }

    public void NotifyEnd()
    {
        OnDialogEnd?.Invoke();
    }
};