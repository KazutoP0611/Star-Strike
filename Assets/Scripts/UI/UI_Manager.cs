using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    public UI_FadeScreen ui_fadeScreen          { get; private set; }
    public UI_GameOverScreen ui_gameOverScreen  { get; private set; }
    public UI_Dialogue ui_dialogue              { get; private set; }

    [SerializeField] private GameObject[] uiElements;

    [Header("Test")]
    [SerializeField] private DialogueLineSO lines;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        ui_fadeScreen = GetComponentInChildren<UI_FadeScreen>(true);
        ui_gameOverScreen = GetComponentInChildren<UI_GameOverScreen>(true);
        ui_dialogue = GetComponentInChildren<UI_Dialogue>(true);
    }

    private void Start()
    {
        foreach (var uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
    }

    // For testing purposes
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            OpenDialogueUI(lines);
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

    public void OpenDialogueUI(DialogueLineSO firstLine)
    {
        ui_dialogue.gameObject.SetActive(true);
        ui_dialogue.PlayDialogueLine(firstLine);
    }

    public void SetActiveCursor(bool active) => Cursor.visible = active;
}
