using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float originalSpeed = 1000f;
    public float speed = 1000f;
    public float nextWaypointDistance = 1f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    public Rigidbody2D rb;
    private EnemyMeleeAttacks enemyMeleeAttack;
    private bool isAttacking = false;
    private float attackCooldown = 0f;
    private float attackCooldownDuration = 2f;

    // New variable to track player's in-range status
    private bool playerInRange = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemyMeleeAttack = GetComponentInChildren<EnemyMeleeAttacks>();
        originalSpeed = speed;
        rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
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
    
    private void OnEnable()
    {
        EnemyInRangeToAttack.OnPlayerInRange += HandlePlayerInRange;
    }
    
    private void OnDisable()
    {
        EnemyInRangeToAttack.OnPlayerInRange -= HandlePlayerInRange;
    }

    private void HandlePlayerInRange(bool inRange)
    {
        // Update playerInRange status
        playerInRange = inRange;

        if (inRange)
        {
            speed = 0;
            if (!isAttacking)
            {
                StartCoroutine(AttackRoutine());
                isAttacking = true;
            }
        }
        else
        {
            speed = originalSpeed;
        }
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (Time.time >= attackCooldown && playerInRange)
            {
                Attack();
                attackCooldown = Time.time + attackCooldownDuration;
            }
            yield return null; 
        }
    }

    private void Attack()
    {
        enemyMeleeAttack.ActivateMeleeAttack();
    }
}
