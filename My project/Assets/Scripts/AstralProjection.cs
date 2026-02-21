using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;

public class AstralProjection : MonoBehaviour
{
    private bool isActive = false;
    private GameObject ghostPlayer;

    public Image frostOverlay;
    public GameObject ghostPrefab;
    public GameObject ghostLightPrefab;

    [Header("Ghost Settings")]
    public float ghostSpeed = 10f;
    public float ghostGrowTime = 0.2f;
    public float ghostStartScale = 0.2f;
    public float ghostDrag = 3f;

    private Rigidbody2D bodyRb;
    private PlayerMovement bodyMovement;
    private PlayerInput bodyInput;
    private Animator bodyAnimator;

    private CinemachineVirtualCamera cam;

    private void Awake()
    {
        bodyRb = GetComponent<Rigidbody2D>();
        bodyMovement = GetComponent<PlayerMovement>();
        bodyInput = GetComponent<PlayerInput>();
        bodyAnimator = GetComponent<Animator>();
        cam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Start()
    {
        frostOverlay.enabled = false;
    }

    public void OnToggleGhost(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!isActive)
            CreateGhostPlayer();
    }

    void CreateGhostPlayer()
    {
        isActive = true;
        frostOverlay.enabled = true;

        // Disable main player controls
        bodyMovement.enabled = false;
        bodyInput.enabled = false;
        bodyRb.linearVelocity = Vector2.zero;

        // Sleep animation
        bodyAnimator.SetBool("isSleeping", true);

        // Spawn ghost
        ghostPlayer = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostPlayer.name = "Ghost";

        // Start small
        ghostPlayer.transform.localScale = Vector3.one * ghostStartScale;

        // Ghost visuals
        SpriteRenderer ghostRender = ghostPlayer.GetComponent<SpriteRenderer>();
        ghostRender.color = new Color(0.4f, 0.7f, 1f, 0.6f);

        // Disable ghost control initially
        PlayerInput ghostInput = ghostPlayer.GetComponent<PlayerInput>();

        // Assign proper PlayerMovement settings for the ghost
        PlayerMovement ghostMovement = ghostPlayer.GetComponent<PlayerMovement>();
        if (ghostMovement != null)
        {
            ghostMovement.groundLayer = bodyMovement.groundLayer;

            Transform groundCheck = ghostPlayer.transform.Find("GroundCheck");
            if (groundCheck != null)
                ghostMovement.groundCheck = groundCheck;

            ghostMovement.jumpCount = 0;
        }

        // Init ghost recall input
        GhostInput ghostInputHandler = ghostPlayer.GetComponent<GhostInput>();
        if (ghostInputHandler != null)
            ghostInputHandler.Init(this);

        // Init ghost cleanup
        GhostCleanup ghostCleanup = ghostPlayer.GetComponent<GhostCleanup>();
        if (ghostCleanup != null)
            ghostCleanup.Init(this);

        // Spawn ghost light as child
        if (ghostLightPrefab != null)
        {
            GameObject ghostLight = Instantiate(ghostLightPrefab, ghostPlayer.transform.position, Quaternion.identity);
            ghostLight.transform.SetParent(ghostPlayer.transform, true);
            ghostLight.transform.localPosition = Vector3.zero;
        }

        // Camera follows ghost
        if (cam != null)
            cam.Follow = ghostPlayer.transform;

        // Shoot ghost in the player's facing direction
        Rigidbody2D ghostRb = ghostPlayer.GetComponent<Rigidbody2D>();
        if (ghostRb != null)
        {
            ghostRb.linearVelocity = Vector2.zero;
            ghostRb.linearDamping = ghostDrag;

            float direction = Mathf.Sign(transform.localScale.x);
            ghostRb.AddForce(Vector2.right * direction * ghostSpeed, ForceMode2D.Impulse);
        }

        // Start grow coroutine
        StartCoroutine(GrowAndEnableControl(ghostPlayer, ghostInput));
    }

    private IEnumerator GrowAndEnableControl(GameObject ghost, PlayerInput ghostInput)
    {
        Vector3 startScale = ghost.transform.localScale;
        Vector3 targetScale = Vector3.one;
        float elapsed = 0f;

        while (elapsed < ghostGrowTime)
        {
            ghost.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / ghostGrowTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ghost.transform.localScale = targetScale;

    }

    // G key recall: stay at body, destroy ghost
    public void RecallToGhost()
    {
        if (!isActive || ghostPlayer == null)
            return;

        if (cam != null)
            cam.Follow = transform;

        Destroy(ghostPlayer);

        bodyRb.simulated = true;
        bodyMovement.enabled = true;
        bodyInput.enabled = true;

        // Wake animation
        bodyAnimator.SetBool("isSleeping", false);

        frostOverlay.enabled = false;
        isActive = false;
    }

    // Recall platform: teleport body to ghost position
    public void RecallToGhostPosition()
    {
        if (!isActive || ghostPlayer == null)
            return;

        Vector3 ghostPos = ghostPlayer.transform.position;

        RecallToGhost();

        transform.position = ghostPos;
        bodyRb.position = ghostPos;
    }

    public bool IsProjecting()
    {
        return isActive;
    }
}