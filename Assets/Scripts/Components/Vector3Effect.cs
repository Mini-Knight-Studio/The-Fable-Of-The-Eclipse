using System;
using Loopie;

class Vector3Effect : Effect
{
    public string category;
    public int probability;
    public bool application_addFalse_multiplyTrue;
    public Vector3 points;
    public float pointx;
    public float pointy;
    public float pointz;
    public bool updateMode_useFalse_timeTrue;
    public float duration;

    public override void InitEffect()
    {
        points = new Vector3(pointx, pointy, pointz);
        inner_category = category;
        inner_probability = probability;
        inner_application = application_addFalse_multiplyTrue ? ValueApplication.Multiply : ValueApplication.Add;
        inner_duration = duration;
        inner_updateMode = updateMode_useFalse_timeTrue ? UpdateMode.Time : UpdateMode.Use;
        base.InitEffect();
    }

    public string GetEffectType()
    {
        return "Vector3Effect";
    }
};