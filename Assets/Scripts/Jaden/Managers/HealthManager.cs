using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager
{
    private int _currentHealth;
    private int _currentMaxHealth;

    public int Health
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }
    
    public int MaxHealth
    {
        get
        {
            return _currentMaxHealth;
        }
        set
        {
            _currentMaxHealth = value;
        }
    }

    public HealthManager(int health, int  maxHealth)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount > _currentHealth)
        {
            _currentHealth = 0;
        }
        
        if (_currentHealth > 0)
        {
            _currentHealth -= damageAmount;
        }
    }
    
    public void HealUnit(int healAmount)
    {
        if (_currentHealth == 0)
        {
            return;
        }
        
        if (_currentHealth < _currentMaxHealth)
        {
            _currentHealth += healAmount;
        }

        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
    }
}
