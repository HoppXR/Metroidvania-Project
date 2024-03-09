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
            eHealth.TakeDamage(damage);
        }

        if (other.CompareTag("Boss"))
        {
            var bHealth = other.gameObject.GetComponent<BossHealth>();
            bHealth.TakeDamage(damage);
        }
    }
}
