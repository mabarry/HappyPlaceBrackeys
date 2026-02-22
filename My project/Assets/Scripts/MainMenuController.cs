using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    public LevelTransition levelTransition;
    
    public void OnStartClick()
    {
        if (levelTransition != null)
            levelTransition.TransitionToScene("Monastery");
        else
            SceneManager.LoadScene("Monastery");
    }

    public void OnQuitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
