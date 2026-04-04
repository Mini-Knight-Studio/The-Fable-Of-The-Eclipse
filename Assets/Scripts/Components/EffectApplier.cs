using System;
using System.Collections.Generic;
using Loopie;

class EffectApplier : Component
{
    private List<Effect> effects;
    void OnCreate()
    {
        effects = new List<Effect>();
    }

    void OnUpdate()
    {
        foreach (Effect effect in effects)
        {
            effect.UpdateEffect();
        }
    }

    public void AddEffect(Effect effect)
    {
        effects.Add(effect);
        effect.InitEffect();
    }
};