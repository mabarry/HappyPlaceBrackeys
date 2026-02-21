using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    public Image whiteFade;
    public string nextSceneName = "Monastery";
    
    [Header("Distance Settings")]
    public float maxDistance = 10f; // distance where fade starts
    public float minDistance = 1f;  // distance for full fade (before entering)
    
    [Header("Timing")]
    public float finalFadeDuration = 0.5f; // quick fade when entering portal
    public float holdDuration = 0.3f;
    public float fadeOutDuration = 1.5f;

    private bool isTransitioning = false;
    private Transform portal;
    private Transform player;

    void Start()
{
    DontDestroyOnLoad(gameObject);
    DontDestroyOnLoad(whiteFade.transform.root.gameObject);
    
    Color c = whiteFade.color;
    c.a = 0f;
    whiteFade.color = c;
}

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        
        if (isTransitioning) return;

        if (portal == null || player == null) return;

        // Calculate distance to portal
        float distance = Vector2.Distance(player.position, portal.position);

        float fadeAmount = 0f;
        if (distance < maxDistance)
        {
            fadeAmount = Mathf.InverseLerp(maxDistance, minDistance, distance);
            fadeAmount = Mathf.Clamp(fadeAmount, 0f, 0.8f);
        }

        Color c = whiteFade.color;
        c.a = fadeAmount;
        whiteFade.color = c;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindReferences();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindReferences();
    }

    void FindReferences()
    {
        // Find portal
        GameObject portalObj = GameObject.FindGameObjectWithTag("Portal");
        portal = portalObj != null ? portalObj.transform : null;
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj != null ? playerObj.transform : null;
    }

    public void StartTransition()
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeTransition());
        }
    }

    IEnumerator FadeTransition()
    {
        isTransitioning = true;

        // Get current fade amount
        float currentAlpha = whiteFade.color.a;

        // Quick fade to full white from current position
        yield return StartCoroutine(FadeToWhite(currentAlpha, finalFadeDuration));
        
        // Hold on white
        yield return new WaitForSeconds(holdDuration);
        
        // Load new scene
        SceneManager.LoadScene(nextSceneName);
        
        // Fade from white
        yield return StartCoroutine(FadeFromWhite(fadeOutDuration));
        
        isTransitioning = false;
    }

    IEnumerator FadeToWhite(float startAlpha, float duration)
    {
        float elapsed = 0f;
        Color c = whiteFade.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, 1f, elapsed / duration);
            whiteFade.color = c;
            yield return null;
        }

        c.a = 1f;
        whiteFade.color = c;
    }

    IEnumerator FadeFromWhite(float duration)
    {
        float elapsed = 0f;
        Color c = whiteFade.color;
        c.a = 1f;
        whiteFade.color = c;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            whiteFade.color = c;
            yield return null;
        }

        c.a = 0f;
        whiteFade.color = c;
    }
}