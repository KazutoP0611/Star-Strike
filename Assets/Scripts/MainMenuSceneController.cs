using UnityEngine;

public class MainMenuSceneController : MonoBehaviour
{
    public void GotoGameScene()
    {

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
