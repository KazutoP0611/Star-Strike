using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //private GameObject[] chunks;
    private List<LevelChunk> m_listOfChunks;
    private LevelSequence_SO m_currentLevelSequence;
    private int m_prefixLevelIndex;
    private int m_currentLevelIndex;
    private int m_currentLevelSeqIndex;
    private float m_speedMultiplier = 1.0f;

    [Header("Level Details")]
    [SerializeField] private GameObject[] normalChunkPrefabs;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private int chunkLimit = 12;
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
        m_currentLevelSeqIndex = 0;
        m_listOfChunks = new List<LevelChunk>();

        RegisterNewSequence();
        SpawnStartChunk();
    }

    private void RegisterNewSequence()
    {
        m_currentLevelIndex = 0;
        m_currentLevelSequence = levelSeqs[m_currentLevelSeqIndex];

        m_prefixLevelIndex = m_currentLevelSequence.GetNormalChunkAmount();
    }

    private void SpawnStartChunk()
    {
        while (m_listOfChunks.Count < chunkLimit)
            SpawnNormalChunk();
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
            LevelChunk chunkLevel = m_listOfChunks[i];
            GameObject chunkObject = chunkLevel.gameObject;

            chunkObject.transform.Translate(-transform.forward * ((normalMoveSpeed * m_speedMultiplier) * Time.deltaTime));

            // Activate chunk's set sequence or spawn objects when it came to certain point;
            if (chunkObject.transform.position.z <= activateChunkTrasnform.position.z - chunkLength)
            {
                if (!chunkLevel.IsActivated)
                {
                    chunkLevel.ActivateLevel();
                    continue;
                }
            }

            if (chunkObject.transform.position.z <= clearPointTransform.position.z - chunkLength)
            {
                m_listOfChunks.Remove(chunkLevel);
                Destroy(chunkObject);

                // If chunks in list is lower than limited number, spawn new level;
                if (m_listOfChunks.Count < chunkLimit)
                    SpawnLevelChunk();
            }
        }
    }

    private void SpawnLevelChunk()
    {
        // If there is no more level left, spawn only normal level, so there is no need to check all of below;
        if (m_currentLevelSeqIndex >= levelSeqs.Count())
        {
            SpawnNormalChunk();
            return;
        }

        //
        if (m_prefixLevelIndex > 0)
        {
            m_prefixLevelIndex--;

            SpawnNormalChunk();
            return;
        }

        // Prepare to spawn new chunk object, get chunk object from current sequence;
        LevelChunk chunkLevel = m_currentLevelSequence.GetCurrentLevelChunk(m_currentLevelIndex);
        
        if (chunkLevel != null)
        {
            // Spawn set level from level sequence;
            SpawnChunk(chunkLevel.gameObject);
            m_currentLevelIndex++;
        }
        // There is no more level chunk in current level sequence;
        else
        {
            // Update current sequence index number;
            m_currentLevelSeqIndex++;

            // Check if there is still level to play
            if (m_currentLevelSeqIndex < levelSeqs.Count())
            {
                // Register new sequence
                RegisterNewSequence();
            }
            else
            {
                // No more level to play, spawn normal chunk;
                SpawnNormalChunk();
            }
        }
    }

    private void SpawnNormalChunk() => SpawnChunk(normalChunkPrefabs[Random.Range(0, normalChunkPrefabs.Count())]);

    private void SpawnChunk(GameObject chunkObject)
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, 0, GetPositionZ());

        GameObject spawnedChunk = Instantiate(
                chunkObject,
                spawnPosition,
                Quaternion.identity,
                chunkParent
            );

        LevelChunk chunkLevel = spawnedChunk.GetComponent<LevelChunk>();
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
