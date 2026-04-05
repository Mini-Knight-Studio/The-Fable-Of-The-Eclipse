using Loopie;
using System;
using static Effect;

public class TemporalEffect : Component
{
    public string category;
    public string type;
    public int probability;
    public bool application_addFalse_multiplyTrue;
    public int pointsInt;
    public float pointsFloat;
    public Vector3 pointsVector;
    public bool updateMode_useFalse_timeTrue;
    public float duration;
    private float timer;

    public void InitEffect()
    {
        timer = duration;
        Debug.Log("Effect Initialized");
    }

    public void UpdateEffect()
    { timer += Time.deltaTime; }

    public void UseEffect()
    { timer -= 1.0f; }

    public bool EffectEnded()
    { return timer <= 0.0f; }
};