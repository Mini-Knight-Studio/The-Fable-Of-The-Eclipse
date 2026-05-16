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

    BoxCollider boxCollider;

    void OnCreate()
    {
        boxCollider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (Input.IsKeyPressed(KeyCode.U) && !DialogUI.Instance.IsDialogOpen)
        {
            Open();
        }
    }

    private void Open()
    {
        DialogUI.Instance.SetText(textValue, nextCharSeparator);
        DialogUI.Instance.StartReading(characterDelay);
    }
};