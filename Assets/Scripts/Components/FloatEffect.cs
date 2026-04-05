using System;
using Loopie;

class FloatEffect : Effect
{
    public string category;
    public int probability;
    public bool application_addFalse_multiplyTrue;
    public float points;
    public bool updateMode_useFalse_timeTrue;
    public float duration;

    public override void InitEffect()
    {
        inner_category = category;
        inner_probability = probability;
        inner_application = application_addFalse_multiplyTrue ? ValueApplication.Multiply : ValueApplication.Add;
        inner_duration = duration;
        inner_updateMode = updateMode_useFalse_timeTrue ? UpdateMode.Time : UpdateMode.Use;
        base.InitEffect();
    }

    
};