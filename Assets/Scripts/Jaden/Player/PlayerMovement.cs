using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    internal PlayerCombat Combat;
    internal GrapplingHook GrappleHook;
    internal InputReader Input;
    private Rigidbody2D _rb;
    public Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    private Vector2 _moveVector;
    private bool _canMove;
    
    [Header("Knockback Settings")]
    [SerializeField] private float knockback;
    [SerializeField, Range(0,1)] private float forceDamping;
    private Vector2 _forceToApply;

    [Header("Dash Settings")] 
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private bool _isDashing;
    private bool _canDash;

    private Collider2D[] _colliders;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int LastHorizontal = Animator.StringToHash("LastHorizontal");
    private static readonly int LastVertical = Animator.StringToHash("LastVertical");
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        GrappleHook = FindFirstObjectByType<GrapplingHook>();
        Combat = FindFirstObjectByType<PlayerCombat>();
        
        _rb = GetComponent<Rigidbody2D>();
        _colliders = GetComponents<Collider2D>();
        
        InputReader.Init(this);
        InputReader.SetPlayerControls();
    }

    void Start()
    {
        PlayerRevive();
    }

    void Update()
    {
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        if (_isDashing || !_canMove)
        {
            return;
        }
        
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerDamage"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;
            _forceToApply += collisionNormal * knockback;
        }
    }

    public void SetMovementDirection(Vector2 dir)
    {
        if (!_canMove)
        {
            _moveVector = Vector2.zero;
            return;
        }
        
        _moveVector = dir;
    }

    private void Move()
    {
        Vector2 moveForce = moveSpeed * _moveVector.normalized;

        moveForce += _forceToApply;
        _forceToApply *= forceDamping;

        if (Mathf.Abs(_forceToApply.x) <= 0.01f && Mathf.Abs(_forceToApply.y) <= 0.01f)
        {
            _forceToApply = Vector2.zero;
        }
        
        _rb.velocity = moveForce;
    }

    public void PlayerDash()
    {
        if (_canDash)
        {
            StartCoroutine(Dash());
        }
    }
    
    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;

        //Play dash animation
        
        _rb.velocity = new Vector2(_moveVector.x * dashSpeed, _moveVector.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
    
    private void HandleAnimation()
    {
        animator.SetFloat(Horizontal, _rb.velocity.x);
        animator.SetFloat(Vertical, _rb.velocity.y);
        animator.SetFloat(Speed, _rb.velocity.sqrMagnitude);

        if (!_canMove)
            return;
        
        if (_moveVector.x == 1 || _moveVector.x == -1 || _moveVector.y == 1 || _moveVector.y == -1)
        {
            animator.SetFloat(LastHorizontal, _moveVector.x);
            animator.SetFloat(LastVertical, _moveVector.y);
        }
    }

    private void OnEnable()
    {
        HealthManager.OnPlayerDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        HealthManager.OnPlayerDeath -= PlayerDeath;
    }

    private void PlayerDeath()
    {
        CanMoveFalse();

        foreach (Collider2D collider in _colliders)
        {
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
        
        animator.SetTrigger("Death");
        animator.SetBool("Dead", true);
    }

    private void PlayerRevive()
    {
        animator.SetBool("Dead", false);
        
        foreach (Collider2D collider in _colliders)
        {
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
        
        CanMoveTrue();
    }

    public void SlowDown()
    {
        moveSpeed *= 0.5f;
    }

    public void SpeedUp()
    {
        moveSpeed *= 2;
    }

    public void CanMoveFalse()
    {
        _canMove = false;
        _rb.velocity = Vector2.zero;
        
        _canDash = false;
        Combat.canAttack = false;
        GrappleHook.CanGrappleFalse();
    }

    public void CanMoveTrue()
    {
        _canMove = true;
        
        _canDash = true;
        Combat.canAttack = true;
        GrappleHook.CanGrappleTrue();
    }

    public void CanDashTrue()
    {
        _canDash = true;
    }

    public void CanDashFalse()
    {
        _canDash = false;
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
