using UnityEngine;

public class GhostActivator : MonoBehaviour
{
    private bool activated = false;
    private GhostPlatform[] ghostPlatforms;
    private SpriteRenderer sr;
    private AstralProjection astralProjection;
    private NormalPlatform doorToDisable;
    private GameObject ghostWall;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ghostPlatforms = FindObjectsByType<GhostPlatform>(FindObjectsSortMode.None);
        astralProjection = FindFirstObjectByType<AstralProjection>();
        
        GameObject doorObj = GameObject.Find("Door");
        if (doorObj != null)
        {
            doorToDisable = doorObj.GetComponent<NormalPlatform>();
        }

        ghostWall = GameObject.Find("GhostWall");
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
            
            if (doorToDisable != null)
            {
                doorToDisable.gameObject.SetActive(false);
            }

            // Disable the Ghost Wall
            if (ghostWall != null)
            {
                ghostWall.SetActive(false);
            }
        }
    }
}