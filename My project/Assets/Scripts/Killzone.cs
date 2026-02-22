using UnityEngine;

public class Killzone : MonoBehaviour
{
    private DeathMenu deathMenu;

    void Start()
    {
        deathMenu = FindFirstObjectByType<DeathMenu>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (deathMenu != null)
            {
                Destroy(other.gameObject);
                deathMenu.ShowDeathMenu();
            }
        }
        else if (other.name == "Ghost")
        {
            Destroy(other.gameObject);
        }
    }
}