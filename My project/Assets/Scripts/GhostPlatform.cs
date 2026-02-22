using UnityEngine;

public class GhostPlatform : MonoBehaviour
{
    [Header("Visibility")]
    private SpriteRenderer sr;
    private Collider2D col;
    private AstralProjection astralProjection;
    private bool activated = false;

    [Header("Colors")]
    public Color defaultColor = new Color(0.4f, 0.7f, 1f, 0.6f);
    public Color activatedColor = new Color(1f, 0.4f, 0.8f, 1f);

    [Header("Movement")]
    public bool moves = true;
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public float pauseDuration = 3f;

    private Vector3 startPos;
    private Vector3 topPos;
    private bool movingUp = true;
    private bool pausing = false;
    private float pauseTimer = 0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        astralProjection = FindObjectOfType<AstralProjection>();

        startPos = transform.position;
        topPos = startPos + Vector3.up * moveDistance;
    }

    public void Activate()
    {
        activated = true;
        sr.color = activatedColor;
        col.enabled = true;
        sr.enabled = true;
    }

    private void Update()
    {
        if (!activated)
        {
            // Only visible in ghost mode when not activated
            bool ghostMode = astralProjection != null && astralProjection.IsProjecting();
            sr.enabled = ghostMode;
            col.enabled = ghostMode;
            if (ghostMode)
                sr.color = defaultColor;
        }

        if (!moves)
            return;

        // Movement
        if (pausing)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                pausing = false;
                pauseTimer = 0f;
                movingUp = false;
            }
            return;
        }

        if (movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, topPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, topPos) < 0.01f)
            {
                transform.position = topPos;
                pausing = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
            {
                transform.position = startPos;
                movingUp = true;
            }
        }
    }
}