using Loopie;
using System;
using System.Collections;

public class Hand : Component
{
    [Header("Hand")]
    public bool rightHand;
    [Space(10)]
    [Header("Children")]
    public Entity Side;
    public Entity HandShadow;
            
    private int sequence;                   //0) No sequence 1) Punch | 2) Spikes
    private float timer;                    //Global internal timer

    private bool defeated;                  //True if hand is burned
    private bool ended_attack_sequence;     //True if hand finished its attack
    private bool doing_sequence;            //True if hand is attacking with any attack or returning from attack
    private bool can_attack;                //True when collider is able to damage player
    private bool already_attacked;          //True if attack has already attacked player

    private Boss boss;

    private Transform base_hand_transform;
    private Entity spikes;

    private BoxCollider hand_punch_trigger;
    private BoxCollider side_trigger;

    #region Internal
    public void SetUp(Boss b)               //Set ups starting vars
    {
        boss = b;

        hand_punch_trigger = entity.GetComponent<BoxCollider>();
        side_trigger = Side.GetComponent<BoxCollider>();

        base_hand_transform = transform;
        spikes = Side.GetChild(0);

        sequence = 1;
        timer = 0.0f;

        defeated = false;
        ended_attack_sequence = false;
        doing_sequence = false;
        can_attack = false;
        already_attacked = false;
    }

    public void Update()                    //Updates hand constant behaviour
    {
        if (defeated || timer == 0.0f) return;
        if (timer < 0.0f) timer = 0.0f;
        timer -= Time.deltaTime;
    }

    private float Value(Vector2 stage_variable)
    {
        return boss.stage == 0 ? stage_variable.x : boss.stage == 1 ? stage_variable.y : 0;
    }
    #endregion

    #region Attacks
    public void Attack()
    {
        boss.target_side_comparition.y = rightHand ? 1 : 0;
        if (sequence == 0) StartCoroutine(Punch());
        if (sequence == 1) StartCoroutine(Spikes());
    }

    public IEnumerator Punch()
    {
        yield return null;
    }

    public IEnumerator Spikes()
    {
        yield return null;
    }

    public IEnumerator Recover()
    {
        yield return null;
    }
    #endregion

    #region Attack Behaviours
    #endregion

    #region Attack Controls
    public bool IsOnSide()
    {
        return side_trigger.IsColliding;
    }
    #endregion

    //private float ValueByStage(Vector2 vector)
    //{
    //    return stage == 0 ? vector.x : stage == 1 ? vector.y : 0;
    //}

    //private Vector3 SameAsTarget(Vector3 exclude)
    //{
    //    Vector3 position = target.transform.position;
    //    if(exclude.x == 1) position.x = transform.position.x;
    //    if(exclude.y == 1) position.y = transform.position.y;
    //    if (exclude.z == 1) position.z = transform.position.z;
    //    return position;
    //}

    //public void StartSequence()
    //{
    //    if (!IsOnSide() || doingSequence) return;
    //    if (sequence == 0) StartCoroutine(Punch());
    //    if (sequence == 1) StartCoroutine(SummonSpikes());
    //    if(endedSequence) Return();
    //}

    //public void CancelSequence()
    //{
    //    StopAllOwnedCoroutines();
    //    StartCoroutine(Return());
    //}

    //#region Sequences
    //public IEnumerator Punch()
    //{
    //    if (available)
    //    {
    //        doingSequence = true;
    //        endedSequence = false;
    //        hasAttacked = false;
    //        //Tracking
    //        while(timer < ValueByStage(fistTrackTime))
    //        {
    //            timer += Time.deltaTime;
    //            transform.position = SameAsTarget(Vector3.Up);
    //            yield return null;
    //        }
    //        Vector3 attackPosition = transform.position;
    //        timer = 0;
    //        //Shake
    //        while (timer < ValueByStage(shakeTime))
    //        {
    //            timer += Time.deltaTime;
    //            transform.position += Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, Loopie.Random.Range(0,360)) * ValueByStage(fistVelocity) * Time.deltaTime;
    //            yield return null;
    //        }
    //        transform.position = attackPosition;
    //        //Hand going down
    //        while (transform.position.y > ValueByStage(fistVelocity*4) * Time.deltaTime)
    //        {
    //            transform.position += new Vector3(0, -1 * ValueByStage(fistVelocity*4) * Time.deltaTime, 0);
    //            yield return null;
    //        }
    //        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    //        timer = 0;
    //        canAttack = true;
    //        while(timer < ValueByStage(fistGroundCooldown))
    //        {
    //            timer += Time.deltaTime;
    //            yield return null;
    //        }
    //        canAttack = false;
    //        endedSequence = true;
    //    }
    //    timer = 0;
    //}

    //public IEnumerator SummonSpikes()
    //{
    //    if (available)
    //    {
    //        hasAttacked = false;
    //        yield return null;
    //    }
    //    timer = 0;
    //}

    //public IEnumerator Return()
    //{
    //    if (available)
    //    {
    //        while (Vector3.Distance(transform.position, basePosition.position) > ValueByStage(fistVelocity) * Time.deltaTime)
    //        {
    //            transform.position += new Vector3(transform.position.x - basePosition.position.x, transform.position.y - basePosition.position.y, transform.position.z - basePosition.position.z) * -1 * ValueByStage(fistVelocity) * Time.deltaTime;
    //            yield return null;
    //        }

    //        while (transform.rotation.x - basePosition.rotation.x > ValueByStage(fistVelocity) * Time.deltaTime)
    //        {
    //            transform.rotation += new Vector3(ValueByStage(fistVelocity) * Time.deltaTime, 0, 0);
    //            yield return null;
    //        }
    //        transform.position = basePosition.position;
    //        transform.rotation = basePosition.rotation;
    //        sequence = sequence == 0 ? 1 : 0;
    //        doingSequence = false;
    //        endedSequence = false;
    //    }
    //}

    //#endregion

    //public void Attack()
    //{
    //    if (!canAttack || !hasAttacked) return;
    //    if(handTrigger.HasCollided || sideTrigger.HasCollided)
    //    {
    //        target.PlayerHealth.Damage(damage);
    //        hasAttacked = true;
    //    }
    //}
};