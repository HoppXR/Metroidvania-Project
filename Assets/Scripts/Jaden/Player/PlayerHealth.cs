using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    
    void Start()
    {
        healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            TakeDamage(20);
            Debug.Log(GameManager.gameManager._playerHealth.Health);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Heal(10);
            Debug.Log(GameManager.gameManager._playerHealth.Health);
        }
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
