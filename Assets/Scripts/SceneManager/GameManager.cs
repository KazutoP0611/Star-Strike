using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private float sceneFadeDuration = 1.0f;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName);
    }

    public void ChangeScene(string sceneName)
    {
        // SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName));
    }

    private IEnumerator ChangeSceneCo(string sceneName)
    {
        // Fade in
        UI_FadeScreen fadeScreen = GetFadeScreen();
        fadeScreen.FadeIn();

        //FadeUI.instance.FadeIn();

        yield return fadeScreen.fadeCoroutine;

        SceneManager.LoadScene(sceneName);

        //yield return new WaitForSeconds(0.2f);

        // Fade out
    }

    private UI_FadeScreen GetFadeScreen()
    {
        if (UI_Manager.instance != null)
            return UI_Manager.instance.ui_fadeScreen;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
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
