using System;
using Loopie;

class SimpleTextUI : Component
{
    public Entity containerEntity;
    public Entity textEntity;
    [HideInInspector] public Text text;

    public static SimpleTextUI Instance { get; private set; }
    void OnCreate()
    {
        if (Instance == null)
            Instance = this;
        else
            return;
        
        text = textEntity.GetComponent<Text>();
        containerEntity.SetActive(false);
    }

    public void SetText(string value)
    {
        if (text != null)
            text.SetText(value);
    }

    public void Open()
    {
        containerEntity.SetActive(true);
    }

    public void Close()
    {
        containerEntity.SetActive(false);
    }
};