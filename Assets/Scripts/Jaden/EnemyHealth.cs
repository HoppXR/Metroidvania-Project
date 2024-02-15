using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int currentHealth;
    private EnemyAI enemyAI;
    private bool chase = false;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = _maxHealth;
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        if (!chase)
        {
            enemyAI.enabled = true;
            chase = true;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }

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
        enemyAI.enabled = false;
        Destroy(gameObject);
    
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
        
        enabled = false;
    }
}