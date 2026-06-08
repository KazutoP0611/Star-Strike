using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneController : MonoBehaviour
{
    [SerializeField] private string gameLevelString;

    public void GotoGameScene()
    {
        // Add fade screen
        // After fade, load new scene

        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevelString);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
