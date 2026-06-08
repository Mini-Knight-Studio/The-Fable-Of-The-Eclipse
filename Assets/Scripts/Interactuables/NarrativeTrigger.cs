using Loopie;
using System;

class NarrativeTrigger : Component
{
    public string textValue = "";
    private BoxCollider boxCollider;

    void OnCreate()
    {
        boxCollider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (boxCollider.HasCollided)
        {
            SimpleTextUI.Instance.Open(0.5f);
            SimpleTextUI.Instance.SetText(textValue);
        }
        if (boxCollider.HasEndedCollision)
        {
            SimpleTextUI.Instance.Close(0.5f);
        }
    }
};