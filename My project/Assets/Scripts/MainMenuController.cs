using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    public LevelTransition levelTransition;
    
    public void OnStartClick()
    {
        if (levelTransition != null)
        {
            levelTransition.nextSceneName = "Monastery";
            levelTransition.StartTransition();
        }
        else
        {
            SceneManager.LoadScene("Monastery");
        }
    }

    public void OnQuitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
