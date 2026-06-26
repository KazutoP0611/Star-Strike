using System.Linq;
using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    private bool m_isActivated = false;

    [SerializeField] private GameObject testSpawnObject;
    [SerializeField] private GameObject enemyParent;

    public bool IsActivated { get { return m_isActivated; } }

    public void ActivateLevel()
    {
        enemyParent.SetActive(true);

        m_isActivated = true;
    }
}
