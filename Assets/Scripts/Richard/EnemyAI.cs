using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public BoxCollider2D detectionCollider;
    public CircleCollider2D attackCollider;
    public CircleCollider2D textCollider;
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
    [SerializeField] private float attackDelay = 2f;
    private float lastEnterTime = 0f;
    private float triggerEnterTime = 0f;
    [SerializeField] private float attackRange = 1f;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            return;
        }

        if (detectionCollider == null || attackCollider == null)
        {
            return;
        }

        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void UpdatePath()
    {
        if (seeker != null && seeker.IsDone())
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
        if (other.CompareTag("Player"))
        {
            lastEnterTime = Time.time;
            colliderActivated = true;
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time - lastEnterTime >= attackDelay && !isAttacking)
        {
            ActivateAttack();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            colliderActivated = false;
            if (isAttacking)
            {
                StartCoroutine(DeactivateAttack());
            }
        }
    }

    private void ActivateAttack()
    {
        isAttacking = true;
        attackCollider.enabled = true;
        if (player != null)
        {
            Vector2 directionToPlayer = player.position - transform.position;
            directionToPlayer.Normalize();
            attackCollider.offset = directionToPlayer * attackRange;
        }
        StartCoroutine(DeactivateAttack());
    }

    private IEnumerator DeactivateAttack()
    {
        yield return new WaitForSeconds(0.3f);
        attackCollider.enabled = false;
        isAttacking = false;
    }
    
    void OnEnable()
    {
            detectionCollider.enabled = true;

        if (textCollider != null)
        {
            textCollider.enabled = false;
        }
    }
    
    void OnDisable()
    {
        detectionCollider.enabled = false;
    }
}