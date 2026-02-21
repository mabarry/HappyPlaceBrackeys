using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public string mainMenuSceneName = "MainMenu";
    public InputActionReference pauseAction; // add this
    private bool isPaused = false;

    void Awake()
    {
        if (FindObjectsByType<PauseMenuManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        pauseMenuUI.SetActive(false);
    }

    void OnEnable()
    {
        pauseAction.action.Enable();
    }

    void OnDisable()
    {
        pauseAction.action.Disable();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
        {
            pauseMenuUI.SetActive(false);
            return;
        }

        if (pauseAction.action.triggered)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}