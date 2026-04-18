using System;
using System.Collections;
using Loopie;

class Hand : Component
{
    private bool available;
    private bool doingSequence;
    private bool canAttack;
    private bool hasAttacked;
    private int stage;
    private Vector3 basePosition;
    public Entity HandShadow;
    public bool rightHand;
    public Vector2 fistTrackTime;
    public Vector2 shakeTime;
    public Vector2 fistVelocity;
    public Vector2 fistGroundCooldown;
    public Vector2 spikeWarn;
    public Vector2 spikeActive;
    private float timer;
    private Entity target;
    public Entity spikes;
    private BoxCollider handTrigger;
    private BoxCollider spikeTrigger;

    public void SetUpHand(Entity t, int s)
    {
        timer = 0;
        target = t;
        stage = s;
        available = true;
        available = false;
        basePosition = transform.position;
        hasAttacked = false;
        handTrigger = entity.GetComponent<BoxCollider>();
        spikeTrigger = spikes.GetComponent<BoxCollider>();
    }

    private float ValueByStage(Vector2 vector)
    {
        return stage == 0 ? vector.x : stage == 1 ? vector.y : 0;
    }

    private Vector3 DirectionToTarget(Vector3 exclude)
    {
        Vector3 direction = (target.transform.position - transform.position);
        if(exclude.x == 1) direction.x = 0;
        if(exclude.y == 1) direction.y = 0;
        if(exclude.z == 1) direction.z = 0;
        return direction.normalized;
    }

    public IEnumerator Punch()
    {
        if (available)
        {
            //Tracking
            while(timer < ValueByStage(fistTrackTime))
            {
                timer += Time.deltaTime;
                transform.position += DirectionToTarget(Vector3.Up) * ValueByStage(fistVelocity) * Time.deltaTime;
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
            while (Vector3.Distance(transform.position, basePosition) > ValueByStage(fistVelocity)*Time.deltaTime)
            {
                transform.position += new Vector3(transform.position.x - basePosition.x, transform.position.y - basePosition.y, transform.position.z - basePosition.z) * -1 * ValueByStage(fistVelocity ) * Time.deltaTime;
                yield return null;
            }
            transform.position = basePosition;
            canAttack = false;
        }
        timer = 0;
    }
};