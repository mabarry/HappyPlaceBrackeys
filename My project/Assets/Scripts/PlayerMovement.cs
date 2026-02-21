using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 5f;
    public int maxJumps = 2;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float acceleration = 10f;

    [Header("Better Jump")]
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D body;
    private Animator animator;
    private Vector2 moveInput;
    public int jumpCount;
    private bool isGrounded;
    private bool wasGrounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded) {
             jumpCount = 0;
            animator.SetBool("isJumping", false);
        }

        wasGrounded = isGrounded;

        // Flip sprite based on direction
        if (moveInput.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Update animations
        animator.SetBool("isRunning", Mathf.Abs(body.linearVelocity.x) > 0.1f && isGrounded);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && jumpCount < maxJumps)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0f);
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            animator.SetBool("isJumping", true);
        }
    }

    private void FixedUpdate()
    {
        float targetSpeed = moveInput.x * speed;
        float speedDiff = targetSpeed - body.linearVelocity.x;
        float force = speedDiff * acceleration;
        body.AddForce(new Vector2(force, 0f));

        // Better jump physics
        if (body.linearVelocity.y < 0)
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (body.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}