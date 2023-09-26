using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float wallJumpForce = 10.0f;
    public float wallSlideSpeed = 1.0f;
    public float wallStickTime = 0.25f;
    public float wallJumpTime = 0.5f;

    private Rigidbody2D rb;

    private bool isJumping;
    private bool isWallSliding;

    private float wallStickTimer;
    private float wallJumpTimer;

    private int wallDirX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        bool jumpInput = Input.GetButtonDown("Jump");

        HandleWallSliding();

        if (isWallSliding)
        {
            if (jumpInput)
            {
                // Wall jump
                rb.velocity = new Vector2(-wallDirX * wallJumpForce, jumpForce);
                wallJumpTimer = wallJumpTime;
            }
        }
        else
        {
            // Normal jump
            if (jumpInput && !isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
            }
        }

        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else
        {
            wallJumpTimer = 0;
        }

        float moveVelocity = horizontalInput * moveSpeed;
        rb.velocity = new Vector2(moveVelocity, rb.velocity.y);

        if (horizontalInput != 0)
        {
            // Flip the player sprite based on the direction
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void HandleWallSliding()
    {
        wallDirX = (rb.velocity.x > 0) ? 1 : -1;

        isWallSliding = false;

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.right * wallDirX, 0.6f, LayerMask.GetMask("Wall"));
        if (hitInfo.collider != null)
        {
            if (!isJumping && rb.velocity.y < 0)
            {
                isWallSliding = true;
                wallStickTimer = wallStickTime;
            }
        }

        if (wallStickTimer > 0)
        {
            rb.velocity = new Vector2(0, -wallSlideSpeed);
            wallStickTimer -= Time.deltaTime;
        }
        else
        {
            wallStickTimer = 0;
        }
    }
}