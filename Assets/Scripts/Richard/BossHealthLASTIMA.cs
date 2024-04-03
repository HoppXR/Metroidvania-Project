using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthLastima : MonoBehaviour
{
    private Animator _animator;
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

    public static bool isDead;

    [SerializeField] public GameObject enablePortal;
    [SerializeField] private GameObject specialEffectObject; 

    private bool specialEffectActivated = false; 

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<LatismaAI>();
        _pirateAttacks = GetComponent<PirateAttacks>();

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
        _animator.SetBool("Dead", isDead);

        _animator.SetFloat("Horizontal", _rb.velocity.x);
        _animator.SetFloat("Speed", _rb.velocity.sqrMagnitude);

        if (_rb.velocity.x >= 1 || _rb.velocity.x >= -1 || _rb.velocity.y >= 1 || _rb.velocity.y >= -1)
        {
            _animator.SetFloat("LastHorizontal", _rb.velocity.x);
        }
    }

    public void TakeDamage(float damage)
    {
        healthBar.SetActive(true);

        if (_currentHealth >= damage)
        {
            _currentHealth -= damage;
        }
        else if (_currentHealth <= damage)
        {
            _currentHealth = 0;
        }

        Debug.Log(_currentHealth);

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

        enablePortal.SetActive(true);
        _enemyAI.enabled = false;
        _pirateAttacks.enabled = false;

        _animator.SetTrigger("Death");

        isDead = true;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        GameManager.gameManager._playerHealth.HealUnit(100);

        enabled = false;
    }

}
