using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        movement.Normalize();

        rb.velocity = movement * moveSpeed;

        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = false; 
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = true; 
        }
    }
}
