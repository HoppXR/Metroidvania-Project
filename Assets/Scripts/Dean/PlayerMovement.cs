using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f; // Adjust this value to change the movement speed

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
    }

    void Update()
    {
        // Input handling
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Calculate movement vector
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Normalize the movement vector to ensure constant speed in all directions
        movement.Normalize();

        // Move the player
        rb.velocity = movement * moveSpeed;

        // Flip the character if moving left
        if (moveHorizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // Flip the character if moving right
        else if (moveHorizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
