using UnityEngine;

public class GhostActivator : MonoBehaviour
{
    private bool activated = false;
    private GhostPlatform[] ghostPlatforms;
    private SpriteRenderer sr;
    private AstralProjection astralProjection;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ghostPlatforms = FindObjectsOfType<GhostPlatform>();
        astralProjection = FindObjectOfType<AstralProjection>();
    }

    private void Update()
    {
        bool ghostMode = astralProjection != null && astralProjection.IsProjecting();
        sr.enabled = ghostMode;
        GetComponent<Collider2D>().enabled = ghostMode && !activated;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated && other.name == "Ghost")
        {
            activated = true;
            sr.flipX = !sr.flipX;

            foreach (GhostPlatform platform in ghostPlatforms)
                platform.Activate();
        }
    }
}