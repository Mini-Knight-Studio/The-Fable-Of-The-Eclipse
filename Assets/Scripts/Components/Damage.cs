using Loopie;
using System;
using System.Numerics;

public class Effect
{
    public int Probability;
    public int Damage;
    public int Ticks;
    public int TickSpeed;
    private float timer;
    public void InitEffect()
    {
        timer = 0;
    }
    public bool UpdateEffect(Health health)
    {
        timer += Time.deltaTime;
        if (timer > TickSpeed)
        {
            timer = 0;
            health.Damage(Damage);
            Ticks--;
        }
        return Ticks == 0;
    }
}