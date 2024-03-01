using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    internal GrapplingHook GrappleHook;

    private InputReader _input;
    private Vector2 _moveVector;
    
    Rigidbody2D _rb;
    public Animator animator;
    
    private Vector2 _forceToApply;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    
    [Header("Knockback Settings")]
    [SerializeField] private float knockback;
    [SerializeField, Range(0,1)] private float forceDamping;

    [Header("Dash Settings")] 
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private bool _isDashing;
    private bool _canDash;
    private bool _canMove;

    private void Awake()
    {
        GrappleHook = FindFirstObjectByType<GrapplingHook>();
        
        _rb = GetComponent<Rigidbody2D>();
        
        InputReader.Init(this);
        InputReader.SetPlayerControls();
    }

    void Start()
    {
        _canMove = true;
        _canDash = true;
    }

    void Update()
    {
        animator.SetFloat("Horizontal", _moveVector.x);
        animator.SetFloat("Vertical", _moveVector.y);
        animator.SetFloat("Speed", _moveVector.sqrMagnitude);
        
        if (_isDashing || !_canMove)
        {
            return;
        }
        
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ApplyDamage"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;
            _forceToApply += collisionNormal * knockback;
            //Destroy(collision.gameObject);
        }
    }

    public void SetMovementDirection(Vector2 dir)
    {
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
        StartCoroutine(Dash());
    }
    
    private IEnumerator Dash()
    {
        if (_canDash)
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
    }

    public void CanMoveFalse()
    {
        _canMove = false;
        _rb.velocity = Vector2.zero;
        
        _canDash = false;
    }

    public void CanMoveTrue()
    {
        _canMove = true;
        
        _canDash = true;
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
