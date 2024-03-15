using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float range = 5f;
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 5f;

    private Transform player;
    private float nextFireTime;

    void Start()
    {

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
                    FireProjectile();
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }
    }

    void FireProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectileObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * projectileSpeed;
        }
    }
}