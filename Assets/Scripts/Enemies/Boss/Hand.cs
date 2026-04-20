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
    private bool vulnerable;
    [HideInInspector]
    public bool defeated;                  //True if hand is burned
    private bool ended_attack_sequence;     //True if hand finished its attack
    private bool doing_sequence;            //True if hand is attacking with any attack or returning from attack
    private bool already_attacked;          //True if attack has already attacked player

    private Boss boss;

    private Transform base_hand_transform;
    private Entity spikes;

    private BoxCollider side_trigger;
    private BoxCollider hand_punch_trigger;
    private BoxCollider spike_trigger;

    #region Internal
    public void SetUp(Boss b)               //Set ups starting vars
    {
        boss = b;
        base_hand_transform = transform;
        spikes = Side.GetChild(0);

        hand_punch_trigger = entity.GetComponent<BoxCollider>();
        side_trigger = Side.GetComponent<BoxCollider>();
        spike_trigger = spikes.GetComponent<BoxCollider>();

        sequence = 1;
        timer = 0.0f;

        vulnerable = true;
        defeated = false;
        ended_attack_sequence = false;
        doing_sequence = false;
        already_attacked = false;
        HandShadow.SetActive(false);
        transform.rotation = Vector3.Zero;
    }

    public void Update()                    //Updates hand constant behaviour
    {
        HitPlayer();

        if (defeated || timer == 0.0f) return;
        timer -= Time.deltaTime;
        if (timer < 0.0f) timer = 0.0f;
    }

    private Vector3 Mantain(Vector3 vector, Vector3 excluder, Vector3 axis)
    {
        if(axis.x == 0.0f) vector.x = excluder.x;
        if(axis.y == 0.0f) vector.y = excluder.y;
        if(axis.z == 0.0f) vector.z = excluder.z;
        return vector;
    }
    #endregion

    #region Attacks
    public void Attack()
    {
        if (doing_sequence) return;
        already_attacked = false;
        boss.target_side_comparition.y = rightHand ? 1 : 0;
        if (sequence == 1) StartCoroutine(Punch());
        if (sequence == 2) StartCoroutine(Spikes());
    }

    public IEnumerator Punch()
    {
        if(!defeated)
        {
            doing_sequence = true;
            ended_attack_sequence = false;
            timer = boss.Value(boss.punchTrackTime);
            while(timer > 0.0f)
            {
                if (!boss.NeedsToCancel())
                {
                    HandShadow.SetActive(true);
                    Chase();
                }
                else
                {
                    HandShadow.SetActive(false);
                    Cancel();
                }
                yield return null;
            }
            Vector3 attackPos = transform.position;
            timer = boss.Value(boss.shakeTime);
            while (timer > 0.0f)
            {
                Shake(attackPos);
                yield return null;
            }
            transform.position = attackPos;
            HandShadow.SetActive(false);
            while (transform.position.y > boss.Value(boss.handVelocity) * Time.deltaTime)
            {
                transform.position += new Vector3(0, -1 * boss.Value(boss.handVelocity * 4) * Time.deltaTime, 0);
                yield return null;
            }
            vulnerable = true;
            boss.target.Camera.SetIsShaking(true, boss.Value(boss.punchGroundCooldown), 2, 1);
            yield return new WaitForSeconds(boss.Value(boss.punchGroundCooldown));
            vulnerable = false;
            ended_attack_sequence = true;
            Cancel();
        }
    }

    public IEnumerator Spikes()
    {
        if (!defeated)
        {
            doing_sequence = true;
            ended_attack_sequence = false;
            timer = 3.0f;
            float time = timer;
            while (timer > 0.0f)
            {
                if (!boss.NeedsToCancel())
                    RotatePalm(180, time, false);
                else
                    Cancel();
                yield return null;
            }
            transform.rotation = Vector3.Right * 180;
            timer = boss.Value(boss.spikeWarn);
            while (timer > 0.0f)
            {
                Shake(base_hand_transform.position);
                yield return null;
            }
            transform.position = base_hand_transform.position;
            timer = boss.Value(boss.spikeMovementDuration);
            time = timer;
            boss.target.Camera.SetIsShaking(true, timer, 2, 1);
            while (timer > 0.0f)
            {
                Shake(base_hand_transform.position);
                PushSpikes(Vector3.Up, time);
                yield return null;
            }
            transform.position = base_hand_transform.position;
            yield return new WaitForSeconds(boss.Value(boss.spikeActive));
            timer = 1.0f;
            time = timer;
            while (timer > 0.0f)
            {
                RotatePalm(180, time, true);
                PushSpikes(Vector3.Up * -1, time);
                yield return null;
            }
            transform.rotation = Vector3.Zero;
            ended_attack_sequence = true;
            Cancel();
        }
    }

    public IEnumerator Recover()
    {
        if (!defeated)
        {
            doing_sequence = true;
            while (Vector3.Distance(transform.position, base_hand_transform.position) > boss.Value(boss.handVelocity) * Time.deltaTime)
            {
                transform.position += new Vector3(transform.position.x - base_hand_transform.position.x, transform.position.y - base_hand_transform.position.y, transform.position.z - base_hand_transform.position.z) * -1 * boss.Value(boss.handVelocity) * Time.deltaTime;
                yield return null;
            }

            while (transform.rotation.x - base_hand_transform.rotation.x > boss.Value(boss.handVelocity) * Time.deltaTime)
            {
                transform.Rotate(new Vector3(boss.Value(boss.handVelocity) * Time.deltaTime, 0, 0),Transform.Space.LocalSpace);
                yield return null;
            }
            transform.position = base_hand_transform.position;
            transform.rotation = base_hand_transform.rotation;
            if(ended_attack_sequence) sequence = sequence == 1 ? 2 : 1;
            doing_sequence = false;
            boss.CompleteAttackCycle();
        }
    }
    #endregion

    #region Attack Behaviours
    private void Chase()
    {
        transform.position = Mantain(boss.target.entity.transform.position,transform.position, new Vector3(1,0,1));
    }

    private void Shake(Vector3 base_position)
    {
        transform.position = base_position + Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, Loopie.Random.Range(0, 360)) * boss.Value(boss.handVelocity) * Time.deltaTime;
    }

    private void RotatePalm(float angles, float duration, bool other_sense)
    {
        if(rightHand) angles = -angles;
        if(other_sense) angles = -angles;
        transform.rotation += new Vector3(angles / duration, 0, 0)*Time.deltaTime;
    }

    private void PushSpikes(Vector3 direction, float duration)
    {
        spikes.transform.position += direction * 10/duration * Time.deltaTime;
    }
    
    #endregion

    #region Attack Controls
    public bool IsOnSide()
    {
        return side_trigger.IsColliding;
    }

    private void HitPlayer()
    {
        if(already_attacked) return;
        if(spike_trigger.IsColliding || hand_punch_trigger.IsColliding)
        {
            already_attacked = true;
            boss.target.PlayerHealth.Damage(boss.Damage);
            Debug.Log(boss.target.PlayerHealth.GetActualHealth());
        }
    }

    public void Cancel()
    {
        doing_sequence = false;
        StopAllOwnedCoroutines();
        if (!defeated) StartCoroutine(Recover());
    }
    #endregion
};