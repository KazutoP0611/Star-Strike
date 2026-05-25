using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //private GameObject[] chunks;
    private List<LevelChunk> m_listOfChunks;
    private int m_currentLevelSeqIndex = 0;
    private float m_speedMultiplier = 1.0f;
    private LevelSequence_SO m_currentLevelSequence;

    [Header("Level Details")]
    [SerializeField] private GameObject normalChunkPrefab;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private int chunkMinLimit = 12;
    [Space]
    [SerializeField] private LevelSequence_SO[] levelSeqs;
    [Space]
    [Tooltip("The same length of chunk prefab.")]
    [SerializeField]
    private float chunkLength = 10.0f;

    [Header("Movement Details")]
    [SerializeField] private bool moveChunk = true;
    [SerializeField] private float normalMoveSpeed = 8.0f;

    [Header("Check Point Details")]
    [SerializeField] private Transform clearPointTransform;
    [SerializeField] private Transform activateChunkTrasnform;

    private void Start()
    {
        m_listOfChunks = new List<LevelChunk>();

        RegisterNewSequence();
    }

    private void RegisterNewSequence()
    {
        m_currentLevelSequence = levelSeqs[m_currentLevelSeqIndex];

        for (int i = 0; i < m_currentLevelSequence.GetNormalChunkAmount(); i++)
        {
            SpawnNormalChunk();
        }
    }

    private void Update()
    {
        if (!moveChunk)
            return;

        MoveChunks();
    }

    private void MoveChunks()
    {
        for (int i = 0; i < m_listOfChunks.Count; i++)
        {
            LevelChunk chunk = m_listOfChunks[i];
            GameObject chunkObject = chunk.gameObject;

            chunkObject.transform.Translate(-transform.forward * ((normalMoveSpeed * m_speedMultiplier) * Time.deltaTime));

            // Activate chunk's set sequence or spawn objects when it came to certain point;
            if (chunkObject.transform.position.z <= activateChunkTrasnform.position.z - chunkLength)
            {
                if (!chunk.IsActivated)
                {
                    chunk.ActivateLevel();
                    continue;
                }
            }

            if (chunkObject.transform.position.z <= clearPointTransform.position.z - chunkLength)
            {
                m_listOfChunks.Remove(chunk);
                Destroy(chunkObject);

                // If chunks in list is lower than limited number, spawn new level;
                if (m_listOfChunks.Count < chunkMinLimit)
                    SpawnLevelChunk();
            }
        }
    }

    private void SpawnLevelChunk()
    {
        // Prepare to spawn new chunk object, get chunk object from current sequence;
        LevelChunk chunkLevel = m_currentLevelSequence.GetCurrentChunk();

        if (chunkLevel != null)
        {
            // Spawn set level from level sequence;
            SpawnChunk(chunkLevel.gameObject);
        }
        // There is no more level chunk in current level sequence;
        else
        {
            // Update current sequence index number;
            m_currentLevelSeqIndex++;

            // Check if there is still seqence left to play;
            if (m_currentLevelSeqIndex < levelSeqs.Count())
            {
                // Register new sequence
                RegisterNewSequence();
            }
            else
            {
                // this means the game has end, or at least there is no more level left;
                // Spawn normal chunk instead;
                SpawnNormalChunk();
            }
        }
    }

    private void SpawnNormalChunk() => SpawnChunk(normalChunkPrefab);

    private void SpawnChunk(GameObject chunkObject)
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, 0, GetPositionZ());

        GameObject chunk = Instantiate(
                chunkObject,
                spawnPosition,
                Quaternion.identity,
                chunkParent
            );

        LevelChunk chunkLevel = chunk.GetComponent<LevelChunk>();
        m_listOfChunks.Add(chunkLevel);
    }

    private float GetPositionZ()
    {
        float positionZ;

        if (m_listOfChunks.Count == 0)
            positionZ = transform.position.z;
        else
            positionZ = m_listOfChunks[m_listOfChunks.Count - 1].transform.position.z + chunkLength;

        return positionZ;
    }

    public void SetLevelMovementSpeed(float multiplier)
    {
        m_speedMultiplier = multiplier;
    }
}
