using System;
using System.Collections.Generic;
using Loopie;

public class Health : Component
{
    public int maxHealth;
    private int actualHealth;
    public List<Effect> effects;
    private float timer;
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
        return maxHealth <= 0;
    }

    public void Damage(int points)
    {
        actualHealth -= points;
        actualHealth = actualHealth < 0? 0 : actualHealth;
        Debug.Log("Ouch!");
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
        actualHealth += points;
        actualHealth = actualHealth > maxHealth ? maxHealth : actualHealth;
    }

    public void Reset()
    {
        actualHealth = maxHealth;
    }
};