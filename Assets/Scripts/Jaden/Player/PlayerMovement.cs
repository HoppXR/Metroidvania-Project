using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    
    private Vector2 forceToApply;
    private Vector2 playerInput;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    
    [Header("Knockback Settings")]
    [SerializeField] private float knockback;
    [SerializeField, Range(0,1)] private float forceDamping;

    [Header("Dash Settings")] 
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private bool isDashing;
    private bool canDash;
    private bool canMove;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        canMove = true;
        canDash = true;
    }

    void Update()
    {
        //Prevents any input when dashing
        if (isDashing)
        {
            return;
        }

        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        playerInput = new Vector2(moveX, moveY).normalized;
    }
    
    void FixedUpdate()
    {
        //Prevents any input when dashing
        if (isDashing)
        {
            return;
        }
        
        Vector2 moveForce = playerInput * moveSpeed;
        
        moveForce += forceToApply;
        forceToApply *= forceDamping;

        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
        {
            forceToApply = Vector2.zero;
        }
        rb.velocity = moveForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ApplyDamage"))
        {
            Vector2 collisionNormal = collision.contacts[0].normal;
            forceToApply += collisionNormal * knockback;
            //Destroy(collision.gameObject);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        rb.velocity = new Vector2(playerInput.x * dashSpeed, playerInput.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public bool CanMoveFalse()
    {
        return canMove = false;
    }

    public bool canMoveTrue()
    {
        return canMove = true;
    }
}
