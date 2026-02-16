using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    public float speed = 0.5f;
    public bool stayOnLedge = true;

    private Rigidbody2D body;
    private SpriteRenderer sr;

    private float halfWidth;
    private float halfHeight;
    private int startDirection = 1;
    private int currentDirection;
    private Vector2 movement;
    private bool isGrounded;



    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;
        currentDirection = startDirection;
        sr.flipX = startDirection == 1 ? false : true;
    }

    void FixedUpdate() {
        movement.x = speed * currentDirection;
        movement.y = body.linearVelocity.y;
        body.linearVelocity = movement;
        SetDirection();
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Ground")) {
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    private void SetDirection()
    {
        if (!isGrounded) return;

        Vector2 rightPos = transform.position;
        Vector2 leftPos = transform.position;
        rightPos.x += halfWidth;
        leftPos.x -= halfWidth;

        if (body.linearVelocity.x > 0) {
             if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground"))) {
                currentDirection *= -1;
                sr.flipX = true;
            } else if (stayOnLedge && !Physics2D.Raycast(rightPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground"))) {
                currentDirection *= -1;
                sr.flipX = true;
            }
        } else if (body.linearVelocity.x < 0) {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground"))) {
                currentDirection *= -1;
                sr.flipX = false;
            } else if (stayOnLedge && !Physics2D.Raycast(leftPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground"))) {
                currentDirection *= -1;
                sr.flipX = false;
            }
        }
    }
}
