using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthLastima : MonoBehaviour
{
    private Rigidbody2D _rb;
    private LatismaAI _enemyAI;
    private PirateAttacks _pirateAttacks;
    
    private float _currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject blood;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    private float lerpSpeed = 0.05f;

    private bool chase = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<LatismaAI>();
        _pirateAttacks = GetComponent<PirateAttacks>();
        
        _currentHealth = maxHealth;
    }
    
    private void Update()
    {
        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, _currentHealth, lerpSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        healthBar.SetActive(true);
        
        if (!chase)
        {
            _enemyAI.enabled = true;
            _enemyAI.rb.constraints &= ~(RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY);
            _enemyAI.canMove = true;
            _pirateAttacks.enabled = true;
            chase = true;
            
        }
        
        if (_currentHealth >= damage)
        {
            _currentHealth -= damage;
        }
        else if (_currentHealth <= damage)
        {
            _currentHealth = 0;
        }

        // Updates Health bar UI
        healthSlider.value = _currentHealth;
        
        // TODO: Play hurt animation
        
        Instantiate(blood, transform.position, Quaternion.identity);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        healthBar.SetActive(false);
        
        _enemyAI.enabled = false;
        _pirateAttacks.enabled = false;
        Destroy(gameObject);

        // Play die animation
    
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
        
        enabled = false;
    }
}