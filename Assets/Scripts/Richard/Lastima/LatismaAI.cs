using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LatismaAI : MonoBehaviour
{
    [HideInInspector] public bool canMove;
    
    public BoxCollider2D detectionCollider;
    public GameObject attackHitbox;
    public CircleCollider2D textCollider;
    private Transform player;
    public float speed = 1000f;
    public float nextWaypointDistance = 1f;
    private Path path;
    private Vector2 direction;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;
    private Seeker seeker;
    public Rigidbody2D rb;
    private PlayerHealth _playerHealth;
    private bool isAttacking = false;
    private bool colliderActivated = false;
    [SerializeField] private float attackDelay = 2f;
    private float lastEnterTime = 0f;
    private float triggerEnterTime = 0f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] int damage = 10;
    private NPC _npc;

    private void Awake()
    {
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform;
        
        if (player == null)
        {
            return;
        }

        if (detectionCollider == null || attackHitbox == null)
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

        if (!canMove)
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

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
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
            canMove = true;
            colliderActivated = false;
            if (isAttacking)
            {
                StartCoroutine(DeactivateAttack());
            }
        }
    }

    private void ActivateAttack()
    {
        canMove = false;
        isAttacking = true;

        
        StartCoroutine(DeactivateAttack());
    }

    private IEnumerator DeactivateAttack()
    {
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
    
    void OnEnable()
    {
        canMove = true;
        
        detectionCollider.enabled = true;

        if (textCollider != null)
        {
            textCollider.enabled = false;
        }

        if (_npc != null)
        {
            _npc.enabled = false;
        }
    }
    
    void OnDisable()
    {
        detectionCollider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY |RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.constraints &= ~(RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY);
        }
    }
}
