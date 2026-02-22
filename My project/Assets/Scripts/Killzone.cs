using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.name == "Ghost")
        {
            // respawn or destroy
            Destroy(other.gameObject);
        }
    }
}