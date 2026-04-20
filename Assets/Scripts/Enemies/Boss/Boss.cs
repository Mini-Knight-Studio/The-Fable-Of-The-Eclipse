using System;
using System.Threading;
using Loopie;

public class Boss : Component
{
    [Header("Boss Parts")]
    public Entity leftHandEntity;
    public Entity rightHandEntity;

    private Hand leftHand;
    private Hand rightHand;
    [Space(10)]
    [Header("Boss Variables")]
    public Vector2 StartEndWaitTime;
    public float InBetweenStagesCooldown;
    public Vector2 InBetweenAttacksCooldown;
    [Space(10)]
    [Header("Hand Variables")]
    public Vector2 punchTrackTime;
    public Vector2 shakeTime;
    public Vector2 handVelocity;
    public Vector2 punchGroundCooldown;
    public Vector2 spikeWarn;
    public Vector2 spikeMovementDuration;
    public Vector2 spikeActive;

    public int Damage;
    [HideInInspector]
    public int stage;
    private bool on_sequence;
    private bool defeated;
    private float stage_timer;
    [HideInInspector]
    public Vector2 target_side_comparition;
    [HideInInspector]
    public Player target;
    private SceneTransition winScene;
    private HeadLookAt headTemporalFeedback;

    #region Internal
    void OnCreate()
    {
        stage = 0;
        on_sequence = false;
        stage_timer = 0;
        target_side_comparition = new Vector2(-1, -1);

        leftHand = leftHandEntity.GetComponent<Hand>();
        leftHand.SetUp(this);
        
        rightHand = rightHandEntity.GetComponent<Hand>();
        rightHand.SetUp(this);

        target = Player.Instance;
        headTemporalFeedback = entity.GetComponent<HeadLookAt>();
        winScene = entity.GetComponent<SceneTransition>();
        headTemporalFeedback.active = false;
    }

    void OnUpdate()
    {
        #region Timer Between Attacks
        if(on_sequence)
        {
            headTemporalFeedback.active = true;
            if (stage_timer > 0.0f)
                stage_timer -= Time.deltaTime;
            else
                StartNextAttack();

            leftHand.Update();
            rightHand.Update();
        }
        #endregion

        #region Start Timer | In Stage Timer | End Timer
        if(!on_sequence)
        {
            headTemporalFeedback.active = false;
            if (StartEndWaitTime.x > 0.0f)
                StartEndWaitTime.x -= Time.deltaTime;
            else
                StartBattle();

            if (defeated && stage < 2)
            {
                if(InBetweenStagesCooldown > 0.0f)
                    InBetweenStagesCooldown -= Time.deltaTime;
                else
                    Evolve();
            }
            else if (defeated && stage >= 2)
            {
                if (StartEndWaitTime.y > 0.0f)
                    StartEndWaitTime.y -= Time.deltaTime;
                else
                    winScene.StartTransition();
            }
        }
        #endregion
    }

    #endregion

    #region Combat Control
    public void StartBattle()
    {
        on_sequence = true;
        stage_timer = 0;
    }

    private void StartNextAttack()
    {
        if (leftHand.IsOnSide())
            leftHand.Attack();

        if (rightHand.IsOnSide())
            rightHand.Attack();
    }

    public void Evolve()
    {
        stage = 1;
        StartBattle();
    }

    private void GetTargetSide()
    {
        target_side_comparition.x = -1;
        if (leftHand.IsOnSide())
            target_side_comparition.x = 0;
        if(rightHand.IsOnSide())
            target_side_comparition.x = 1;
    }

    public bool NeedsToCancel()
    {
        GetTargetSide();
        if(target_side_comparition.x == -1 || target_side_comparition.y == -1)
            return true;
        if (target_side_comparition.x != target_side_comparition.y)
            return true;
        return false;
    }

    public float Value(Vector2 stage_variable)
    {
        return stage == 0 ? stage_variable.x : stage == 1 ? stage_variable.y : 0;
    }

    public void CompleteAttackCycle()
    {
        stage_timer = Value(InBetweenAttacksCooldown);
    }

    private void HeadFall()
    {

    }
    
    #endregion
};