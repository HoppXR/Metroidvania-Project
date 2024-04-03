using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthATERALBUS : MonoBehaviour
{
    private Animator _animator;
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
    [SerializeField] private GameObject specialEffectObject;
    private bool specialEffectActivated = false;
    private bool chase = false;

    public static bool isDead;

    [SerializeField] public GameObject enablePortal;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _ATERALBUSAI = GetComponent<EnemyAIATERALBUS>();
        _swordsManAttacks = GetComponent<SwordsmanAttacks>();

        isDead = false;
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
        
        HandleAnimation();
    }
    
    private void HandleAnimation()
    {
        _animator.SetFloat("Horizontal", _rb.velocity.x);
        _animator.SetFloat("Vertical", _rb.velocity.y);
        _animator.SetFloat("Speed", _rb.velocity.sqrMagnitude);
        
        if (_rb.velocity.x >= 1 || _rb.velocity.x >= -1 || _rb.velocity.y >= 1 || _rb.velocity.y >= -1)
        {
            _animator.SetFloat("LastHorizontal", _rb.velocity.x);
            _animator.SetFloat("LastVertical", _rb.velocity.y);
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
        enablePortal.SetActive(true);
        _ATERALBUSAI.enabled = false;
        _swordsManAttacks.enabled = false;

        _animator.SetTrigger("Death");
        
        isDead = true;
    
        Destroy(gameObject, 3f);
        
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;
        
        GameManager.gameManager._playerHealth.HealUnit(100);
        
        enabled = false;
    }
}