using System.Collections;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private Animator _animator;
    
    public float range = 5f;
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;

    private Transform player;
    private float nextFireTime;

    void Start()
    {
        _animator = GetComponent<Animator>();
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) <= range)
            {
                if (Time.time >= nextFireTime)
                {
                    StartCoroutine(FireProjectile());
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }
    }

    IEnumerator FireProjectile()
    {
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.85f);
        
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectileObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * projectileSpeed;
        }
    }
}