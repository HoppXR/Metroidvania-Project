using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Rigidbody2D _rb;
    private EnemyAI _enemyAI;
    private NPC _npc;
    
    private float _currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject blood;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    private float lerpSpeed = 0.05f;
    
    public GameObject TalkECanavs;
    private bool chase = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();
        _npc = GetComponent<NPC>();
        
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
            if (_npc != null)
            {
                _npc.enabled = false;
                TalkECanavs.SetActive(false);
            }

            if (_enemyAI != null)
            {
                _enemyAI.enabled = true;
                chase = true;
                _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            }
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
        _enemyAI.enabled = false;
        Destroy(gameObject);

        // Play die animation
    
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
        
        enabled = false;
    }
}