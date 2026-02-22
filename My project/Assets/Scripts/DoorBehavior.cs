using UnityEngine;

public class NormalPlatform : MonoBehaviour
{
    [Header("Visibility")]
    private SpriteRenderer sr;
    private Collider2D col;
    private AstralProjection astralProjection;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color ghostFadeColor = new Color(1f, 1f, 1f, 0.3f);

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        astralProjection = FindFirstObjectByType<AstralProjection>();

        // Start visible and solid
        sr.enabled = true;
        col.enabled = true;
        sr.color = normalColor;
    }

    private void Update()
    {
        bool ghostMode = astralProjection != null && astralProjection.IsProjecting();

        if (ghostMode)
        {
            sr.enabled = true;
            col.enabled = false;
            sr.color = ghostFadeColor;
        }
        else
        {
            sr.enabled = true;
            col.enabled = true;
            sr.color = normalColor;
        }
    }
}