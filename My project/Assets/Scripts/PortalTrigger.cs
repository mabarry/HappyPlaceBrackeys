using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    private LevelTransition levelTransition;

    void Start()
    {
        levelTransition = FindObjectOfType<LevelTransition>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            levelTransition.StartTransition();
        }
    }
}