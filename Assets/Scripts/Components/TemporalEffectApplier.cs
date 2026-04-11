using System;
using System.Collections.Generic;
using System.Net.Security;
using Loopie;

public class TemporalEffectApplier : Component
{
    private List<TemporalEffect> effects;
    void OnCreate()
    {
        effects = new List<TemporalEffect>();
    }

    void OnUpdate()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            TemporalEffect effect = effects[i];
            if (effect.updateMode_useFalse_timeTrue)
            {
                effect.UpdateEffect();
            }
        }
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            TemporalEffect effect = effects[i];

            if (effect.EffectEnded())
            {
                effects.RemoveAt(i);
            }
        }
    }

    public void AddEffect(TemporalEffect effect)
    {
        if (effect == null) return;

        effect.InitEffect();
        int random = Loopie.Random.Range(0, 100);
        if (random >= effect.probability)
            return;
        Debug.Log("Applied Effect");
        effects.Add(effect);
    }

    public int GetEffectValueInt(int baseValue, string category)
    {
        int modifiedValue = baseValue;
        float multiplier = 1;
        foreach (TemporalEffect effect in effects)
        {
            if (!(effect.category == category))
                continue;

            if (!effect.application_addFalse_multiplyTrue)
            {
                if (effect.type == "Int")
                    modifiedValue += effect.pointsInt;
            }

            if (effect.application_addFalse_multiplyTrue)
            {
                if (effect.type == "Int")
                    multiplier *= effect.pointsInt;
                else if (effect.type == "Float")
                    multiplier *= effect.pointsFloat;
            }

            if (!effect.updateMode_useFalse_timeTrue)
                effect.UseEffect();
        }
        float multipliedValue = modifiedValue * multiplier;
        return (int)multipliedValue;
    }
    public float GetEffectValueFloat(float baseValue, string category)
    {
        float modifiedValue = baseValue;
        float multiplier = 1;
        foreach (TemporalEffect effect in effects)
        {
            if (!(effect.category == category))
                continue;

            if (!effect.application_addFalse_multiplyTrue)
            {
                if (effect.type == "Int")
                    modifiedValue += effect.pointsInt;
                if (effect.type == "Float")
                    modifiedValue += effect.pointsFloat;
            }

            if (effect.application_addFalse_multiplyTrue)
            {
                if (effect.type == "Int")
                    multiplier *= effect.pointsInt;
                else if (effect.type == "Float")
                    multiplier *= effect.pointsFloat;
            }

            if (!effect.updateMode_useFalse_timeTrue)
                effect.UseEffect();
        }
        float multipliedValue = modifiedValue * multiplier;
        return multipliedValue;
    }
    public Vector3 GetEffectValueVector3(Vector3 baseValue, string category)
    {
        Vector3 modifiedValue = baseValue;
        Vector3 multiplier = Vector3.One;
        foreach (TemporalEffect effect in effects)
        {
            if (!(effect.category == category))
                continue;

            if (!effect.application_addFalse_multiplyTrue)
            {
                if (effect.type == "Vector3")
                    modifiedValue += effect.pointsVector;
            }

            if (effect.application_addFalse_multiplyTrue)
            {
                if (effect.type == "Vector3")
                {
                    multiplier.x *= effect.pointsVector.x;
                    multiplier.y *= effect.pointsVector.y;
                    multiplier.z *= effect.pointsVector.z;
                }
            }

            if (!effect.updateMode_useFalse_timeTrue)
                effect.UseEffect();
        }
        Vector3 multipliedValue = new Vector3(modifiedValue.x * multiplier.x, modifiedValue.y * multiplier.y, modifiedValue.z * multiplier.z);
        return multipliedValue;
    }
};