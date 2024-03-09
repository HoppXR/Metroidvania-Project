using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            GameManager.gameManager._playerHealth.TakeDamage(damage);
            Debug.Log(GameManager.gameManager._playerHealth.Health);
        }
    }
}
