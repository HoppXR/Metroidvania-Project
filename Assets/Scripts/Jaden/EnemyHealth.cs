using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int currentHealth;
    
    void Start()
    {
        currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
        
        // Play die animation

        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }
}
