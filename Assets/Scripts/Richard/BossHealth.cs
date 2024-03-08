using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private Rigidbody2D _rb;
    private EnemyAI _enemyAI;
    private PirateAttacks _pirateAttacks;
    private NPC _npc;
    
    private float _currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject blood;
    
    public GameObject TalkECanavs;
    private bool chase = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _pirateAttacks = GetComponent<PirateAttacks>();
        _npc = GetComponent<NPC>();
        
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!chase)
        {
            _enemyAI.enabled = true;
            _pirateAttacks.enabled = true;
            chase = true;
            TalkECanavs.SetActive(false);
            _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            _npc.enabled = false;
        }
        
        if (_currentHealth >= damage)
        {
            _currentHealth -= damage;
        }
        else if (_currentHealth <= damage)
        {
            _currentHealth = 0;
        }
        
        // TEMP
        Debug.Log(_currentHealth);

        // Play hurt animation
        Instantiate(blood, transform.position, Quaternion.identity);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        _enemyAI.enabled = false;
        _pirateAttacks.enabled = false;
        Destroy(gameObject);
        
        /////////////////// TEMP ///////////////////
        
        Debug.Log("Enemy died");

        // Play die animation
    
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
        
        enabled = false;
    }
}