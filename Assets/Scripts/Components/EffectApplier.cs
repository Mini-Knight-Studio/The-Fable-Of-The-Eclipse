using System;
using System.Collections.Generic;
using System.Net.Security;
using Loopie;

public class EffectApplier : Component
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
            if (effect.GetUpdateMode() == Effect.UpdateMode.Time)
                effect.UpdateEffect();
        }

        for (int i = effects.Count - 1; i >= 0; i--)
        {
            Effect effect = effects[i];

            if (effect.EffectEnded())
                effects.RemoveAt(i);
        }
    }

    public void AddEffect(Effect effect)
    {
        if (effect == null) return;
        
        effect.InitEffect();
        int random = Loopie.Random.Range(0, 100);
        if (random >= effect.GetProbability())
            return;
        effects.Add(effect);
        
    }

    public void GetEffectValueInt(int baseValue, string category, out int modifiedValue)
    {
        modifiedValue = baseValue;
        float multiplier = 1;
        foreach (Effect effect in effects)
        {
            if (!effect.IsCategory(category))
                continue;

            if (effect.GetValueApplication() == Effect.ValueApplication.Add)
            {
                if (effect is IntEffect castedint)
                    modifiedValue += castedint.points;
                else
                    Debug.LogError("Cannot apply effect that is not Int in Adding mode");
            }

            if (effect.GetValueApplication() == Effect.ValueApplication.Multiply)
            {
                if (effect is IntEffect castedint)
                    multiplier *= castedint.points;
                else if (effect is FloatEffect castedfloat)
                    multiplier *= castedfloat.points;
                else
                    Debug.LogError("Cannot apply effect that is not Int/Float in Multiply mode");
            }

            if (effect.GetUpdateMode() == Effect.UpdateMode.Use)
                effect.UseEffect();
        }
        float multipliedValue = modifiedValue * multiplier; 
        modifiedValue = (int)multipliedValue;
    }

    public void GetEffectValueFloat(float baseValue, string category, out float modifiedValue)
    {
        modifiedValue = baseValue;
        float multiplier = 1f;

        foreach (Effect effect in effects)
        {
            if (!effect.IsCategory(category))
                continue;

            if (effect.GetValueApplication() == Effect.ValueApplication.Add)
            {
                if (effect is FloatEffect castedfloat)
                    modifiedValue += castedfloat.points;
                else if (effect is IntEffect castedint)
                    modifiedValue += castedint.points;
                else
                    Debug.LogError("Cannot apply effect that is not Float/Int in Adding mode");
            }

            if (effect.GetValueApplication() == Effect.ValueApplication.Multiply)
            {
                if (effect is FloatEffect castedfloat)
                    multiplier *= castedfloat.points;
                else if (effect is IntEffect castedint)
                    multiplier *= castedint.points;
                else
                    Debug.LogError("Cannot apply effect that is not Float/Int in Multiply mode");
            }

            if (effect.GetUpdateMode() == Effect.UpdateMode.Use)
                effect.UseEffect();
        }

        modifiedValue *= multiplier;
    }

    public void GetEffectValueVector3(Vector3 baseValue, string category, out Vector3 modifiedValue)
    {
        modifiedValue = baseValue;
        Vector3 multiplier = new Vector3(1f, 1f, 1f);

        foreach (Effect effect in effects)
        {
            if (!effect.IsCategory(category))
                continue;

            if (effect.GetValueApplication() == Effect.ValueApplication.Add)
            {
                if (effect is Vector3Effect castedVec)
                    modifiedValue += castedVec.points;
                else
                    Debug.LogError("Cannot apply effect that is not Vector3 in Adding mode");
            }

            if (effect.GetValueApplication() == Effect.ValueApplication.Multiply)
            {
                if (effect is Vector3Effect castedVec)
                {
                    multiplier.x *= castedVec.points.x;
                    multiplier.y *= castedVec.points.y;
                    multiplier.z *= castedVec.points.z;
                }
                else
                    Debug.LogError("Cannot apply effect that is not Vector3 in Multiply mode");
            }

            if (effect.GetUpdateMode() == Effect.UpdateMode.Use)
                effect.UseEffect();
        }

        modifiedValue.x *= multiplier.x;
        modifiedValue.y *= multiplier.y;
        modifiedValue.z *= multiplier.z;
    }

};