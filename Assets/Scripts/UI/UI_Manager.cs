using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    public UI_FadeScreen ui_fadeScreen { get; private set; }
    public UI_GameOverScreen ui_gameOverScreen { get; private set; }

    [SerializeField] private GameObject[] uiElements;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        ui_fadeScreen = GetComponentInChildren<UI_FadeScreen>(true);
        ui_gameOverScreen = GetComponentInChildren<UI_GameOverScreen>(true);
    }

    private void Start()
    {
        foreach (var uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
    }

    private void SwitchToScreen(GameObject targetScreen)
    {
        foreach (var uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }

        targetScreen.SetActive(true);
    }

   

    public void SetActiveGameOverScreen(bool active)
    {
        SwitchToScreen(ui_gameOverScreen.gameObject);

        SetActiveCursor(active);
    }

    public void SetActiveCursor(bool active) => Cursor.visible = active;
}
