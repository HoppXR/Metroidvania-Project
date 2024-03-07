using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public CircleCollider2D[] collidersToUse;
    private Transform player;
    public float speed = 1000f;
    public float nextWaypointDistance = 1f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;
    private Seeker seeker;
    public Rigidbody2D rb;
    private bool isAttacking = false;
    private bool colliderActivated = false;
    private float attackDelay = 2f;
    private float lastEnterTime = 0f;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform; // Null check added here
        if (player == null)
        {
            Debug.LogError("Player transform not found.");
            return;
        }

        if (collidersToUse == null || collidersToUse.Length < 1)
        {
            Debug.LogError("Colliders to use not assigned or empty.");
            return;
        }

        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void UpdatePath()
    {
        if (seeker != null && seeker.IsDone()) // Null check added here
            seeker.StartPath(rb.position, player.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * (speed * Time.deltaTime);
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !colliderActivated)
        {
            lastEnterTime = Time.time; 
            colliderActivated = true; 
            StartCoroutine(ActivateCollider());
        }
    }
    
    private IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(attackDelay);
        
        if (colliderActivated && collidersToUse != null && collidersToUse.Length > 0) // Null check added here
        {
            collidersToUse[0].enabled = true;

            // Calculate the direction to the player
            if (player != null) // Null check added here
            {
                Vector2 directionToPlayer = player.position - transform.position;
                directionToPlayer.Normalize();
                collidersToUse[0].offset = directionToPlayer * 1f;
            }

            StartCoroutine(DeactivateCollider());
        }
    }
    
    private IEnumerator DeactivateCollider()
    {
        yield return new WaitForSeconds(0.3f);
        if (collidersToUse != null && collidersToUse.Length > 0)
        {
            collidersToUse[0].enabled = false;
            colliderActivated = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (collidersToUse != null && collidersToUse.Length >= 2)
            {
                collidersToUse[0].enabled = false;
                colliderActivated = false;
            }
        }
    }
    
    void Update()
    {
        if (!enabled)
            return;
        if (Time.time - lastEnterTime > attackDelay && !isAttacking)
        {
            Attack();
        }
    }
    
    private void Attack()
    {
        isAttacking = true;
        lastEnterTime = Time.time;
    }

    void OnEnable()
    {
        collidersToUse[1].enabled = true;
    }

}
