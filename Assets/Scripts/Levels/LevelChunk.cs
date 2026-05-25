using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    private bool m_isActivated = false;

    [SerializeField] private GameObject testSpawnObject;

    public bool IsActivated { get { return m_isActivated; } }

    public void ActivateLevel()
    {
        // This will make eneies and stuff stick to the spawned levels;
        // Let's do enemies first.
        // Even trigger dialogues
        Instantiate(testSpawnObject, transform.localPosition + new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity, transform);

        m_isActivated = true;
    }
}
