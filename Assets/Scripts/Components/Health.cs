using System;
using System.Collections.Generic;
using Loopie;

public class Health : Component
{
    public int maxHealth;
    private int actualHealth;
    public bool canBeDamaged;
    public bool canBeHealed;
    private EffectApplier effectApplier;

    public void Init()
    {
        effectApplier = entity.GetComponent<EffectApplier>();
        canBeDamaged = true;
        canBeHealed = true;
        Reset();
    }

    public void UpdateHealth()
    {

    }

    public int GetActualHealth()
    {
        return actualHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead()
    {
        return actualHealth <= 0;
    }

    public void Damage(int points)
    {
        if (!canBeDamaged) return;
        actualHealth -= points;
        actualHealth = actualHealth < 0 ? 0 : actualHealth;
    }

    //public void AddEffect(List<Effect> effectList)
    //{
    //    int probability = Loopie.Random.Range(0, 101);
    //    for (int i = 0; i < effectList.Count; i++)
    //    {
    //        if (probability > effectList[i].Probability + 1)
    //            continue;
    //        effects.Add(effectList[i]);
    //        effectList[i].InitEffect();
    //    }
    //}

    public void Heal(int points)
    {
        if (!canBeHealed) return;
        actualHealth += points;
        actualHealth = actualHealth > maxHealth ? maxHealth : actualHealth;
    }

    public void Reset()
    {
        actualHealth = maxHealth;
        //effects = new List<Effect>();
    }
};
