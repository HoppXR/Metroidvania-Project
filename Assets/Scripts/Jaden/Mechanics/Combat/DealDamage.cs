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
            if (eAI != null) eAI.rb.constraints &= ~(RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY);
            eHealth.TakeDamage(damage);
        }

        if (other.CompareTag("Boss"))
        {
            var lHealth = other.gameObject.GetComponent<BossHealthLastima>();
            lHealth.TakeDamage(damage);
        }
        
        if (other.CompareTag("Boss2"))
        {
            var aHealth = other.gameObject.GetComponent<BossHealthAves>();
            aHealth.TakeDamage(damage);
        }
        
        if (other.CompareTag("Boss3"))
        {
            var tHealth = other.gameObject.GetComponent<BossHealthATERALBUS>();
            tHealth.TakeDamage(damage);
        }
        
        if (other.CompareTag("Boss4"))
        {
            var iHealth = other.gameObject.GetComponent<BossHealthIoko>();
            iHealth.TakeDamage(damage);
        }
    }
}
