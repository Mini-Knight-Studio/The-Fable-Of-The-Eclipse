using Loopie;
using System;

public class Effect : Component
{
    public enum ValueApplication
    {
        Add,
        Multiply,
    }
    public enum UpdateMode
    {
        Use,
        Time,
    }
    protected string inner_category;
    protected int inner_probability;
    protected float inner_duration;
    protected ValueApplication inner_application;
    protected UpdateMode inner_updateMode;
    private float timer;

    public bool IsCategory(string category)
    { return inner_category == category; }

    public virtual void InitEffect() { timer = inner_duration; }

    public ValueApplication GetValueApplication() { return inner_application; }
    public UpdateMode GetUpdateMode() { return inner_updateMode; }

    public int GetProbability()
    { return inner_probability; }
    public void UpdateEffect()
    { timer += Time.deltaTime; }

    public void UseEffect()
    { timer -= 1.0f; }

    public bool EffectEnded()
    { return timer <= 0.0f; }
}
