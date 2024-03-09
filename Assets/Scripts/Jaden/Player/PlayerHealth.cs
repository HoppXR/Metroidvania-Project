using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    
    void FixedUpdate()
    {
        healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    public void TakeDamage(int damage)
    {
        GameManager.gameManager._playerHealth.TakeDamage(damage);
        healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    private void Heal(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
        healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}
