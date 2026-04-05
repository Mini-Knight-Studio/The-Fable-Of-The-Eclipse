using System;
using Loopie;

class IntEffect : Effect
{
    public string category;
    public int probability;
    public bool application_addFalse_multiplyTrue;
    public int points;
    public bool updateMode_useFalse_timeTrue;
    public float duration;

    public override void InitEffect()
    {
        inner_category = category;
        inner_probability = probability;
        inner_application = application_addFalse_multiplyTrue ? ValueApplication.Multiply : ValueApplication.Add;
        inner_duration = duration;
        inner_updateMode = updateMode_useFalse_timeTrue ? UpdateMode.Time : UpdateMode.Use;
        Debug.Log(inner_probability);
        base.InitEffect();
    }
};