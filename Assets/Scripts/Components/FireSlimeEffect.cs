using System;
using Loopie;

class FireSlimeEffect : Effect
{
    public FireSlimeEffect() : base("FireSlime") {}
    Slime component;
    int damage;

    public override void InitEffect()
    {
        component = entity.GetComponent<Slime>();
        component.Damage = damage;
        
        base.InitEffect();
    }

    public override void UpdateEffect()
    {
        base.UpdateEffect();
    }
};