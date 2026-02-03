using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.12f;
    public KeyCode flipKey = KeyCode.G;
    bool canFlip = true;


    Rigidbody2D rb;
    Collider2D col;
    int gravityDir = -1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        gravityDir = rb.gravityScale >= 0 ? -1 : 1;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        bool groundedNow = IsGrounded();
        if (groundedNow) canFlip = true;

        if (Input.GetKeyDown(KeyCode.Space) && groundedNow)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, -gravityDir * jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(flipKey) && canFlip)
        {
            FlipGravity();
            canFlip = false;
        }
    }


    void FlipGravity()
    {
        gravityDir *= -1;
        rb.gravityScale *= -1f;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
    }

    bool IsGrounded()
    {
        Bounds b = col.bounds;
        Vector2 checkPos = new Vector2(b.center.x, gravityDir == -1 ? b.min.y : b.max.y);
        return Physics2D.OverlapCircle(checkPos, groundCheckRadius, groundLayer) != null;
    }
}
