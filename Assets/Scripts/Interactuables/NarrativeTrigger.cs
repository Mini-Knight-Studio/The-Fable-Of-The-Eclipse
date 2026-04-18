using System;
using Loopie;

class NarrativeTrigger : Component
{
    public string entityName = "TextNarrative";
    public Text text;
    public Entity textEntity;

    public string textValue = "";
    private BoxCollider boxCollider;

    void OnCreate()
    {
        textEntity = Entity.FindEntityByName(entityName);
        text = textEntity.GetComponent<Text>();
        boxCollider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (boxCollider.HasCollided)
        {
            text.Value = textValue;
        }
        if (boxCollider.HasEndedCollision)
        {
            text.Value = "";
        }
    }
};