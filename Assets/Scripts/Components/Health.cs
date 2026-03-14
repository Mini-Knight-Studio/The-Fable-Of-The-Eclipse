using System;
using System.Collections.Generic;
using Loopie;

public class Health : Component
{
    public int maxHealth;
    private int actualHealth;
    public List<Effect> effects;
    private float timer;
    public bool canBeDamaged;
    public bool canBeHealed;
    
    public void Init()
    {
        canBeDamaged = true;
        canBeHealed = true;
        Reset();
    }

    public void UpdateHealth()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].UpdateEffect(this))
            {
                effects.Remove(effects[i]);
                i--;
            }
        }
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
    	if(!canBeDamaged)return;
        actualHealth -= points;
        actualHealth = actualHealth < 0? 0 : actualHealth;
        Debug.Log($"{entity.Name} Ouch!");
    }
    
    public void AddEffect(Effect effect)
    {
        int probability = Loopie.Random.Range(0, 101);
        if (probability > effect.Probability+1)
            return;
        effects.Add(effect);
        effect.InitEffect();
    }

    public void Heal(int points)
    {
    	if(!canBeHealed)return;
        actualHealth += points;
        actualHealth = actualHealth > maxHealth ? maxHealth : actualHealth;
    }

    public void Reset()
    {
        actualHealth = maxHealth;
        effects = new List<Effect>();
    }
};
