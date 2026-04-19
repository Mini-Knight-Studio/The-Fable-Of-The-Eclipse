using System;
using System.Collections;
using Loopie;

public class Hand : Component
{
    private bool available;
    private int sequence;
    private bool doingSequence;
    private bool endedSequence;
    private bool canAttack;
    private bool hasAttacked;
    private int damage;
    private int stage;
    private Transform basePosition;
    public Entity HandShadow;
    public bool rightHand;
    public Vector2 fistTrackTime;
    public Vector2 shakeTime;
    public Vector2 fistVelocity;
    public Vector2 fistGroundCooldown;
    public Vector2 spikeWarn;
    public Vector2 spikeActive;
    private float timer;
    private Player target;
    public Entity side;
    private BoxCollider handTrigger;
    private BoxCollider sideTrigger;
    private Entity spikes;

    public void SetUpHand(int s, int d)
    {
        target = Player.Instance;
        basePosition = transform;
        sequence = 0;
        timer = 0;
        stage = s;
        damage = d;
        available = true;
        doingSequence = false;
        hasAttacked = false;
        handTrigger = entity.GetComponent<BoxCollider>();
        sideTrigger = side.GetComponent<BoxCollider>();
        spikes = side.GetChild(0);
    }

    private float ValueByStage(Vector2 vector)
    {
        return stage == 0 ? vector.x : stage == 1 ? vector.y : 0;
    }

    private Vector3 SameAsTarget(Vector3 exclude)
    {
        Vector3 position = target.transform.position;
        if(exclude.x == 1) position.x = transform.position.x;
        if(exclude.y == 1) position.y = transform.position.y;
        if (exclude.z == 1) position.z = transform.position.z;
        return position;
    }

    public void StartSequence()
    {
        if (!IsOnSide() || !doingSequence) return;
        if (sequence == 0) StartCoroutine(Punch());
        if (sequence == 1) StartCoroutine(SummonSpikes());
        if(endedSequence) Return();
    }

    public void CancelSequence()
    {
        StopAllOwnedCoroutines();
        StartCoroutine(Return());
    }

    #region Sequences
    public IEnumerator Punch()
    {
        if (available)
        {
            endedSequence = false;
            hasAttacked = false;
            //Tracking
            while(timer < ValueByStage(fistTrackTime))
            {
                timer += Time.deltaTime;
                transform.position = SameAsTarget(Vector3.Up);
                yield return null;
            }
            Vector3 attackPosition = transform.position;
            timer = 0;
            //Shake
            while (timer < ValueByStage(shakeTime))
            {
                timer += Time.deltaTime;
                transform.position += Vector3.RotateAroundAxis(transform.Forward, Vector3.Up, Loopie.Random.Range(0,360)) * ValueByStage(fistVelocity) * Time.deltaTime;
                yield return null;
            }
            transform.position = attackPosition;
            //Hand going down
            while (transform.position.y > ValueByStage(fistVelocity*4) * Time.deltaTime)
            {
                transform.position += new Vector3(0, -1 * ValueByStage(fistVelocity*4) * Time.deltaTime, 0);
                yield return null;
            }
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            timer = 0;
            canAttack = true;
            while(timer < ValueByStage(fistGroundCooldown))
            {
                timer += Time.deltaTime;
                yield return null;
            }
            canAttack = false;
            endedSequence = true;
        }
        timer = 0;
    }

    public IEnumerator SummonSpikes()
    {
        if (available)
        {
            hasAttacked = false;
            yield return null;
        }
        timer = 0;
    }

    public IEnumerator Return()
    {
        if (available)
        {
            while (Vector3.Distance(transform.position, basePosition.position) > ValueByStage(fistVelocity) * Time.deltaTime)
            {
                transform.position += new Vector3(transform.position.x - basePosition.position.x, transform.position.y - basePosition.position.y, transform.position.z - basePosition.position.z) * -1 * ValueByStage(fistVelocity) * Time.deltaTime;
                yield return null;
            }

            while (basePosition.position.x - transform.rotation.x < ValueByStage(fistVelocity) * Time.deltaTime)
            {
                transform.rotation += new Vector3(ValueByStage(fistVelocity) * Time.deltaTime, 0, 0);
                yield return null;
            }
            transform.position = basePosition.position;
            transform.rotation = basePosition.rotation;
            sequence = sequence == 0 ? 1 : 0;
        }
    }

    #endregion

    public void Attack()
    {
        if (!canAttack || !hasAttacked) return;
        if(handTrigger.HasCollided || sideTrigger.HasCollided)
        {
            target.PlayerHealth.Damage(damage);
            hasAttacked = true;
        }
    }

    public bool IsOnSide()
    {
        return sideTrigger.IsColliding;
    }
};