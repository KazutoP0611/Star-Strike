using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //private GameObject[] chunks;
    private List<GameObject> m_listOfChunks;
    private int m_currentLevelSeqIndex = 0;
    private LevelSequence_SO m_currentLevelSequence;

    [Header("Level Details")]
    [SerializeField] private GameObject chunkPrefab;
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
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private Transform clearPointTransform;

    private void Start()
    {
        m_listOfChunks = new List<GameObject>();

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
            GameObject chunk = m_listOfChunks[i];
            chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

            if (chunk.transform.position.z <= clearPointTransform.position.z - chunkLength)
            {
                m_listOfChunks.Remove(chunk);
                Destroy(chunk);

                // If chunks in list is lower than limited number, spawn new level;
                if (m_listOfChunks.Count < chunkLimit)
                    SpawnLevelChunk();
            }
        }
    }

    private void SpawnLevelChunk()
    {
        // Prepare to spawn new chunk object, get chunk object from current sequence;
        GameObject chunkLevel = m_currentLevelSequence.GetCurrentChunk();

        if (chunkLevel != null)
        {
            // Spawn set level from level sequence;
            SpawnChunk(chunkLevel);
        }
        // There is no more level in current level sequence;
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

    private void SpawnNormalChunk() => SpawnChunk(chunkPrefab);

    private void SpawnChunk(GameObject levelChunk)
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, 0, GetPositionZ());

        GameObject chunk = Instantiate(
                levelChunk,
                spawnPosition,
                Quaternion.identity,
                chunkParent
            );

        // TODO;
        // After spawned each chunk, each chunk has to have setting for enemies, sequences, etc.
        // And we have to activate.

        m_listOfChunks.Add(chunk);
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
}
