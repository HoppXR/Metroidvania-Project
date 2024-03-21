using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var eHealth = other.gameObject.GetComponent<EnemyHealth>();
            var eAI = other.gameObject.GetComponent<EnemyAI>();
            eAI.rb.constraints &= ~(RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY);
            eHealth.TakeDamage(damage);
        }

        if (other.CompareTag("Boss"))
        {
            var bHealth = other.gameObject.GetComponent<BossHealthLastima>();
            bHealth.TakeDamage(damage);
            
        }
        if (other.CompareTag("Boss2"))
        {
            var bHealth = other.gameObject.GetComponent<BossHealthAves>();
            bHealth.TakeDamage(damage);
        }
        
        if (other.CompareTag("Boss3"))
        {
            var bHealth = other.gameObject.GetComponent<BossHealthATERALBUS>();
            bHealth.TakeDamage(damage);
        }
    }
}
