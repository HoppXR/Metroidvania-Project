using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int currentHealth;
    
    private EnemyAI enemyAI;
    private NPC npc;
    public GameObject TalkECanavs;
    private bool chase = false;

    [SerializeField] private GameObject Blood;
    
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        currentHealth = _maxHealth;
        enemyAI = GetComponent<EnemyAI>();
        npc = GetComponent<NPC>();
    }

    public void TakeDamage(int damage)
    {
        if (!chase)
        {
            enemyAI.enabled = true;
            chase = true;
            TalkECanavs.SetActive(false);
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            npc.enabled = false;
        }
        
        currentHealth -= damage;

        // Play hurt animation
        Instantiate(Blood, transform.position, Quaternion.identity);

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