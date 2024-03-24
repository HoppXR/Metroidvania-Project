using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthATERALBUS : MonoBehaviour
{
    private Rigidbody2D _rb;
    private EnemyAIATERALBUS _ATERALBUSAI;
    private SwordsmanAttacks _swordsManAttacks;

    
    private float _currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject blood;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    private float lerpSpeed = 0.05f;

    private bool chase = false;

    [SerializeField] public GameObject enablePortal;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ATERALBUSAI = GetComponent<EnemyAIATERALBUS>();
        _swordsManAttacks = GetComponent<SwordsmanAttacks>();
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
            _ATERALBUSAI.enabled = true;
            _swordsManAttacks.enabled = true;
            _ATERALBUSAI.rb.constraints &= ~(RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY);
            _ATERALBUSAI.canMove = true;
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
        enablePortal.SetActive(true);
        _ATERALBUSAI.enabled = false;
        _swordsManAttacks.enabled = false;
        Destroy(gameObject);

        // Play die animation
    
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
        
        enabled = false;
    }
}