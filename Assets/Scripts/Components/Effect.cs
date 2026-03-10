using Loopie;
using System;

public class Effect : Component
{
    public int Probability;
    public int Damage;
    public int Ticks;
    public float TickDuration;
    private float timer;
    public void InitEffect()
    {
        timer = 0;
    }
    public bool UpdateEffect(Health health)
    {
        timer += Time.deltaTime;
        if (timer > TickDuration)
        {
            timer = 0;
            health.Damage(Damage);
            Ticks--;
        }
        return Ticks == 0;
    }
}

