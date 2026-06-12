using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    public void Flight()
    {
        GameManager.instance.ChangeScene(gameSceneName);
    }

    public void Quit()
    {
        GameManager.instance.QuitGame();
    }
}
