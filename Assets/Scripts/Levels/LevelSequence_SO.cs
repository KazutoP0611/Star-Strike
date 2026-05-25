using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Setup/Level Sequence Data", fileName = "Level Sequence - ")]
public class LevelSequence_SO : ScriptableObject
{
    private int currentChunkIndex = 0;

    [SerializeField] private int normalChunkBeforeMainChunk = 12;
    [SerializeField] private GameObject[] levelChunks;

    public int GetNormalChunkAmount() => normalChunkBeforeMainChunk;

    public GameObject GetCurrentChunk()
    {
        if (currentChunkIndex >= levelChunks.Length)
            return null;

        GameObject currentLevelChunk = levelChunks[currentChunkIndex];
        currentChunkIndex++;

        return currentLevelChunk;
    }
}
