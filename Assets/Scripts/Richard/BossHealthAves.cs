using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthAves : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private AvesAI _enemyAI;
    private AvesAttack _avesAttack;
    
    private float _currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject blood;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    private float lerpSpeed = 0.05f;
<<<<<<< Updated upstream
    
    public GameObject pulsarObject1;
    public GameObject pulsarObject2;

=======
    [SerializeField] private GameObject specialEffectObject;
    private bool specialEffectActivated = false;
>>>>>>> Stashed changes
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<AvesAI>();
        _avesAttack = GetComponent<AvesAttack>();
        _currentHealth = maxHealth;
        
        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
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
        
        if (_currentHealth >= damage)
        {
            _currentHealth -= damage;
            _animator.SetTrigger("hurt");
        }
        else if (_currentHealth <= damage)
        {
            _currentHealth = 0;
            _animator.SetBool("dead", true);
        }

        // Updates Health bar UI
        healthSlider.value = _currentHealth;
        
        Instantiate(blood, transform.position, Quaternion.identity);

        if (_currentHealth <= maxHealth * 0.5f && !specialEffectActivated)
        {
            specialEffectObject.SetActive(true);
            specialEffectActivated = true;
            Destroy(specialEffectObject, 1f);
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        healthBar.SetActive(false);
        pulsarObject1.SetActive(false);
        pulsarObject2.SetActive(false);
        _enemyAI.enabled = false;
        _avesAttack.enabled = false;
        
        Destroy(gameObject, 2f);
        
        _animator.SetTrigger("death");
    
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
        
        GameManager.gameManager._playerHealth.HealUnit(100);
        
        enabled = false;
    }
}