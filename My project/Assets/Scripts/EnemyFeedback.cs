using UnityEngine;
using System.Collections;

public class EnemyFeedback : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector3 originalScale;
    private Color originalColor;

    [Header("Flash Settings")]
    public float flashDuration = 0.1f;

    [Header("Squash Settings")]
    public float squashAmount = 0.7f;
    public float squashDuration = 0.1f;

    [Header("Stomp Settings")]
    public float bounceForce = 8f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalColor = sr.color; // set once
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                Stomped();

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // Reflect vertical velocity, keep horizontal
                    playerRb.linearVelocity = new Vector2(
                        playerRb.linearVelocity.x,
                        Mathf.Abs(playerRb.linearVelocity.y) // flip negative fall speed to positive bounce
                    );
                }

                break;
            }
        }
    }

    public void Stomped()
    {
        StopAllCoroutines();
        StartCoroutine(StompEffect());
    }

    IEnumerator StompEffect()
    {
        sr.color = new Color(1f, 1f, 1f, 0.6f);
        transform.localScale = new Vector3(originalScale.x, originalScale.y * squashAmount, originalScale.z);

        yield return new WaitForSeconds(squashDuration);

        sr.color = originalColor; // always restores correctly
        transform.localScale = originalScale;
    }
}