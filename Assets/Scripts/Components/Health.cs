using System;
using System.Collections.Generic;
using Loopie;

public class Health : Component
{
    public int maxHealth;
    public int healthCap;
    private int actualHealth;

    public bool canBeDamaged;
    public bool canBeHealed;
    private EffectApplier effectApplier;

    public event Action OnDeath;
    public event Action OnHit;
    public event Action OnHeal;

    public void Init()
    {
        effectApplier = entity.GetComponent<EffectApplier>();
        canBeDamaged = true;
        canBeHealed = true;

        if (healthCap <= 0) 
            healthCap = 9999;

        Reset();
    }

    public int GetActualHealth()
    {
        return actualHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void ModifyMaxHealth(int new_max_health)
    {
        if (new_max_health >= healthCap)
            maxHealth = healthCap;
        else
            maxHealth = new_max_health;
        actualHealth = Mathf.Clamp(actualHealth, 0, maxHealth);
    }
    public void IncreaseMaxHealth(int amount)
    {
        ModifyMaxHealth(maxHealth + amount);
    }

    public void ModifyActualHealth(int new_actual_health)
    {
        new_actual_health = new_actual_health < 0 ? 0 : new_actual_health;
        new_actual_health = new_actual_health > maxHealth ? maxHealth : new_actual_health;
        actualHealth = new_actual_health;
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
        if(actualHealth == 0)
            OnDeath?.Invoke();
        else
            OnHit?.Invoke();
    }

    public void Heal(int points)
    {
        if (!canBeHealed) return;
        actualHealth += points;
        actualHealth = actualHealth > maxHealth ? maxHealth : actualHealth;
        if(points > 0)
            OnHeal?.Invoke();
    }

    public void Reset()
    {
        actualHealth = maxHealth;
    }
};