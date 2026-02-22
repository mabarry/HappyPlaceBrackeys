using UnityEngine;

public class RecallPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Ghost")
        {
            FindObjectOfType<AstralProjection>().RecallToGhostPosition();
        }
    }
}