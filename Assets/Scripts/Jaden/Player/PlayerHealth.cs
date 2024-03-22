using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        GameManager.gameManager._playerHealth.Health = GameManager.gameManager._playerHealth.MaxHealth;
    }
    
    void FixedUpdate()
    {
        healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }
}
