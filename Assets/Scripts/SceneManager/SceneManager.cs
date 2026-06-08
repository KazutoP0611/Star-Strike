using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public SceneManager instance;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
