using Loopie;
using System;

public class Effect : Component
{
    private string inner_category;
    protected bool effect_ended;

    public Effect(string category)
    { inner_category = category; }

    public bool IsCategory(string category)
    { return inner_category == category; }

    public virtual void InitEffect() { effect_ended = false; }
    public virtual void UpdateEffect() { }
    public virtual void RemoveEffect() { }
}