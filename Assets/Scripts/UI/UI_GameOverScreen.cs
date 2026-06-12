using UnityEngine;

public class UI_GameOverScreen : MonoBehaviour
{
    public void Restart()
    {
        GameManager.instance.ChangeScene("MainMenu");
    }
}
