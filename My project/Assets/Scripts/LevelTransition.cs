using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    public Image whiteFade;

    [Header("Level Order")]
    public string[] levels = {"Monastery", "Trippy", "DarkWorld", "End"};

    [Header("Distance Settings")]
    public float maxDistance = 10f;
    public float minDistance = 1f;

    [Header("Timing")]
    public float finalFadeDuration = 0.5f;
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

    public void TransitionToScene(string sceneName)
    {
        if (!isTransitioning)
        {
            nextSceneName = sceneName;
            StartCoroutine(FadeTransition());
        }
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
        GameObject portalObj = GameObject.FindGameObjectWithTag("Portal");
        portal = portalObj != null ? portalObj.transform : null;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj != null ? playerObj.transform : null;
    }

    public string GetNextLevel()
    {
        Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
        string current = SceneManager.GetActiveScene().name;
        for (int i = 0; i < levels.Length - 1; i++)
        {
            if (levels[i] == current)
                return levels[i + 1];
        }
        return null; // no next level (last level)
    }

    public void StartTransition()
    {
        if (!isTransitioning)
        {
            nextSceneName = GetNextLevel();
            if (nextSceneName != null)
                StartCoroutine(FadeTransition());
        }
    }

    public string nextSceneName;

    IEnumerator FadeTransition()
    {
        isTransitioning = true;

        float currentAlpha = whiteFade.color.a;
        yield return StartCoroutine(FadeToWhite(currentAlpha, finalFadeDuration));
        yield return new WaitForSeconds(holdDuration);

        SceneManager.LoadScene(nextSceneName);

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