using System;
using System.Threading;
using Loopie;

class Boss : Component
{
    public Entity target;
    public Vector2 actionCooldown;
    public float stageRegeneration;
    private int stage;
    private float timer;
    private bool updating;
    private Hand leftHand;
    private Hand rightHand;
    void OnCreate()
    {
        stage = 0;
        updating = false;
        timer = 0;
        foreach (Entity child in entity.Children)
        {
            Hand hand_comp = child.GetComponent<Hand>();
            if(hand_comp != null )
            {
                if (hand_comp.rightHand) rightHand = hand_comp;
                else leftHand = hand_comp;
            }
        }
        StartSequence();
        StartCoroutine(leftHand.Punch());
    }

    void OnUpdate()
    {
        if (!updating) return;
    }

    public void StartSequence()
    {
        updating = true;
        timer = 0;
        leftHand.SetUpHand(target, stage);
        rightHand.SetUpHand(target, stage);
    }


};