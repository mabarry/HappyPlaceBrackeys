using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public LevelTransition levelTransition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            levelTransition.StartTransition();
        }
    }
}