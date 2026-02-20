using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class AstralProjection : MonoBehaviour
{
    private bool isActive = false;
    private GameObject ghostPlayer;
    public Image frostOverlay;
    public GameObject ghostPrefab;
    public InputActionReference toggleGhostMode;
    
    private void OnEnable()  { toggleGhostMode.action.Enable(); }
    private void OnDisable() { toggleGhostMode.action.Disable(); }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frostOverlay.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if (toggleGhostMode.action.triggered)
        {
            if (!isActive) {
                createGhostPlayer();
            } else {
                deleteGhostPlayer();
            }
        }
        
        
    }

    void createGhostPlayer() {
        isActive = true;
        frostOverlay.enabled = true;
        SpriteRenderer playerRender = GetComponent<SpriteRenderer>();
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;

        ghostPlayer = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostPlayer.name = "Ghost";

        SpriteRenderer ghostRender = ghostPlayer.GetComponent<SpriteRenderer>();
        ghostRender.sprite = playerRender.sprite;
        ghostRender.color = Color.blue;

        Collider2D playerCollider = GetComponent<Collider2D>();
        Collider2D ghostCollider = ghostPlayer.GetComponent<Collider2D>();
        if (playerCollider != null && ghostCollider != null)
            Physics2D.IgnoreCollision(playerCollider, ghostCollider, true);
    }

    void deleteGhostPlayer() {
        frostOverlay.enabled = false;
        Destroy(ghostPlayer);
        ghostPlayer = null;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerInput>().enabled = true;
        isActive = false;
    }
}
