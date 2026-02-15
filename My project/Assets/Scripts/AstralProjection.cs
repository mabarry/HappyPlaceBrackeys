using UnityEngine;
using UnityEngine.InputSystem;


public class AstralProjection : MonoBehaviour
{
    public Sprite mainPlayer;
    public GameObject ghostPlayer;
    public bool isActive = false;
    public InputActionReference spawnGhostAction;

    private void OnEnable()  { spawnGhostAction.action.Enable(); }
    private void OnDisable() { spawnGhostAction.action.Disable(); }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (spawnGhostAction.action.triggered)
        {
            if (!isActive) {
                createGhostPlayer();
            } else {

            }
        }
        
        
    }

    void createGhostPlayer() {
        isActive = true;
        SpriteRenderer playerRender = GetComponent<SpriteRenderer>();
        ghostPlayer = Instantiate(gameObject, transform.position, transform.rotation);
        ghostPlayer.name = "Ghost";

        SpriteRenderer ghostRender = ghostPlayer.GetComponent<SpriteRenderer>();
        ghostRender.sprite = playerRender.sprite;
        ghostRender.color = Color.blue;

        Collider2D playerCollider = GetComponent<Collider2D>();
        Collider2D ghostCollider = ghostPlayer.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, ghostCollider, true);
    }
}
