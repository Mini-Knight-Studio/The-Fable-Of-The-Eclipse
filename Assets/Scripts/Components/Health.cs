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

    public int GetActualHealth()
    {
        return actualHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void ModifyActualHealth(int new_actual_health)
    {
        new_actual_health = new_actual_health < 0 ? 0 : new_actual_health;
        new_actual_health = new_actual_health > maxHealth ? maxHealth : new_actual_health;
        actualHealth = new_actual_health;
    }

    public void ModifyMaxHealth(int new_max_health)
    {
        maxHealth = new_max_health;
        actualHealth = actualHealth < 0 ? 0 : actualHealth;
        actualHealth = actualHealth > maxHealth ? maxHealth : actualHealth;
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

    public void Heal(int points)
    {
        if (!canBeHealed) return;
        actualHealth += points;
        actualHealth = actualHealth > maxHealth ? maxHealth : actualHealth;
    }

    public void Reset()
    {
        actualHealth = maxHealth;
    }
};