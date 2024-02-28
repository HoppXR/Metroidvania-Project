using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttacks : MonoBehaviour
{
    private Transform player;
    private CircleCollider2D meleeCollider;

    private void Start()
    {
        meleeCollider = GetComponentInChildren<CircleCollider2D>();
        meleeCollider.enabled = false;
        
        player = GameObject.FindWithTag("Player").transform;
    }

    public void ActivateMeleeAttack()
    {
        meleeCollider.enabled = true;
        Vector2 directionToPlayer = player.position - transform.parent.position;
        directionToPlayer.Normalize();
        meleeCollider.offset = directionToPlayer * 1f;
        
        Invoke(nameof(DeactivateMeleeAttack), 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Hit!");
        }
    }

    public void DeactivateMeleeAttack()
    {
        meleeCollider.enabled = false;
    }
}