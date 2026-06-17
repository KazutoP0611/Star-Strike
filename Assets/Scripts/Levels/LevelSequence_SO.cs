using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "StarStrike Setup/Level Setup/Level Sequence Data", fileName = "Level Sequence - ")]
public class LevelSequence_SO : ScriptableObject
{
    [SerializeField] private int normalChunkBeforeMainChunk = 12;
    [SerializeField] private LevelChunk[] levelChunks;

    public int GetNormalChunkAmount() => normalChunkBeforeMainChunk;

    public LevelChunk GetCurrentLevelChunk(int levelIndex)
    {
        if (levelIndex >= levelChunks.Length)
            return null;

        return levelChunks[levelIndex];
    }
}
