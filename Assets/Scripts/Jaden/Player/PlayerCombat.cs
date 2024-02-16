using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    [Header("Attack Settings")]
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackRange;
    private float _nextAttackTime;

    private bool canAttack;

    void Update()
    {
        if (Time.time >= _nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                _nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        if (canAttack)
        {
            // play attack animation
        
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
            
                Debug.Log("We hit " + enemy.name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void CanAttack()
    {
        canAttack = true;
    }

    public void CannotAttack()
    {
        canAttack = false;
    }
}
