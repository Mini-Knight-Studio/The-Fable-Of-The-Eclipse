using System;
using System.Threading;
using Loopie;

class Boss : Component
{
    public Vector2 actionCooldown;
    public float stageRegeneration;
    public float startingCooldown;
    public int damage;
    private int stage;
    private float timer;
    private bool updating;
    public Entity leftHandEntity;
    public Entity rightHandEntity;
    private Hand leftHand;
    private Hand rightHand;

    void OnCreate()
    {
        stage = 0;
        updating = false;
        timer = 0;
        leftHand = leftHandEntity.GetComponent<Hand>();
        rightHand = rightHandEntity.GetComponent<Hand>();
        Debug.Log(leftHand.ID);
        Debug.Log(rightHand.ID);
    }

    void OnUpdate()
    {
        if (!updating && timer < startingCooldown)
        {
            timer += Time.deltaTime;
            if (timer >= startingCooldown)
            {
                timer = 0;
                StartSequence();
            }
        }

        if (updating)
        {
            if (leftHand.IsOnSide())
            {
                Debug.Log("On Left");
                rightHand.CancelSequence();
                leftHand.StartSequence();
            }

            if (rightHand.IsOnSide())
            {
                Debug.Log("On Right");
                leftHand.CancelSequence();
                rightHand.StartSequence();
            }
        }
    }

    public void StartSequence()
    {
        timer = 0;
        leftHand.SetUpHand(stage, damage);
        rightHand.SetUpHand(stage, damage);
        updating = true;
    }
};