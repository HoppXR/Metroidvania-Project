using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    internal GrapplingHook GrappleHook;

    private InputManager _input = null;
    private Vector2 _moveVector = Vector2.zero;
    
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
        _input = new InputManager();
    }

    void Start()
    {
        GrappleHook = FindFirstObjectByType<GrapplingHook>();
        
        _rb = GetComponent<Rigidbody2D>();

        _canMove = true;
        _canDash = true;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat("Horizontal", moveX);
        animator.SetFloat("Vertical", moveY);
        animator.SetFloat("Speed", _moveVector.sqrMagnitude);
        
        if (_isDashing || !_canMove)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _canDash)
        {
            StartCoroutine(Dash());
        }
        
        _moveVector = new Vector2(moveX, moveY).normalized;
    }
    
    void FixedUpdate()
    {
        if (_isDashing || !_canMove)
        {
            return;
        }
        
        Vector2 moveForce = _moveVector * moveSpeed;
        
        moveForce += _forceToApply;
        _forceToApply *= forceDamping;

        if (Mathf.Abs(_forceToApply.x) <= 0.01f && Mathf.Abs(_forceToApply.y) <= 0.01f)
        {
            _forceToApply = Vector2.zero;
        }
        _rb.velocity = moveForce;
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

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += OnMovementPerformed;
        _input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Movement.performed -= OnMovementPerformed;
        _input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        _moveVector = Vector2.zero;
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
