using UnityEngine;

public class GroundShadow : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 5f;
    public LayerMask groundLayer;

    [Header("Shadow Settings")]
    public float minScale = 0.4f;
    public float maxAlpha = 0.6f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, maxDistance);

        if (hit.collider != null)
        {
            // Lock shadow to ground
            transform.position = new Vector3(player.position.x, hit.point.y + 0.01f, 0);

            float height = hit.distance;
            float t = Mathf.Clamp01(height / maxDistance);

            // Scale based on height
            float scale = Mathf.Lerp(1f, minScale, t);
            transform.localScale = new Vector3(scale, scale, 1);

            // Fade based on height
            float alpha = Mathf.Lerp(maxAlpha, 0f, t);
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
        else
        {
            sr.color = new Color(1, 1, 1, 0);
        }
    }
}