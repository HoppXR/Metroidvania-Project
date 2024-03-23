using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageRNG : MonoBehaviour
{
    [SerializeField] private int[] damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            int randomIndex = Random.Range(0, damage.Length);
            int randomDamage = damage[randomIndex];
            GameManager.gameManager._playerHealth.TakeDamage(randomDamage);
            Debug.Log(GameManager.gameManager._playerHealth.Health);
        }
    }
}
