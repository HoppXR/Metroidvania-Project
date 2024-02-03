using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float knockBack;
    [SerializeField, Range(0,1)] private float forceDamping;
    
    private Vector2 forceToApply;
    private Vector2 playerInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    
    void FixedUpdate()
    {
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
            forceToApply += new Vector2(-knockBack, 0);
            //Destroy(collision.gameObject);
        }
    }
}
