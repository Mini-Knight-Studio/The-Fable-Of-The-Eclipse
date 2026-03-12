using System;
using System.Collections.Generic;
using Loopie;

public class Health : Component
{
    public int maxHealth;
    private int actualHealth;
    public List<Effect> effects;
    private float timer;
    private bool canBeDamaged;
    private bool canBeHealed;
    
    public void Init()
    {
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
        //Debug.Log("Ouch!");
        //Debug.Log($"{actualHealth}");
    }
    
    public void AddEffect(Effect effect)
    {
        Random random = new Random();
        int number = random.Next(0, 101);
        if (number > effect.Probability)
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
    
    public void CanBeDamaged(bool option)
    {
    	canBeDamaged = option;
    }
    
    public void CanBeHealed(bool option)
    {
    	canBeHealed = option;
    }

    public void Reset()
    {
        actualHealth = maxHealth;
    }
};
