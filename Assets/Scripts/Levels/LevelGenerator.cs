using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //private GameObject[] chunks;
    private List<GameObject> listOfChunks;

    [Header("Level Chunk Details")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private int startingChunkAmount = 12;

    [Tooltip("The same length of chunk prefab.")]
    [SerializeField]
    private float chunkLength = 10.0f;

    [SerializeField] private Transform chunkParent;

    [Header("Movement Details")]
    [SerializeField] private bool moveChunk = true;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private Transform clearPointTransform;

    private void Start()
    {
        //chunks = new GameObject[startingChunkAmount];
        listOfChunks = new List<GameObject>();

        SpawnStartChunks();
    }

    private void Update()
    {
        if (moveChunk)
            MoveChunks();
    }

    private void SpawnStartChunks()
    {
        for (int i = 0; i < startingChunkAmount; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, 0, GetPositionZ());

        GameObject chunk = Instantiate(
                chunkPrefab,
                spawnPosition,
                Quaternion.identity,
                chunkParent
            );

        listOfChunks.Add(chunk);
    }

    private float GetPositionZ()
    {
        float positionZ;

        if (listOfChunks.Count == 0)
            positionZ = transform.position.z;
        else
            positionZ = listOfChunks[listOfChunks.Count - 1].transform.position.z + chunkLength;

        return positionZ;
    }

    private void MoveChunks()
    {
        for (int i = 0; i < listOfChunks.Count; i++)
        {
            GameObject chunk = listOfChunks[i];
            chunk.transform.Translate(transform.forward * (-moveSpeed * Time.deltaTime));

            if (chunk.transform.position.z <= clearPointTransform.position.z - chunkLength)
            {
                listOfChunks.Remove(chunk);
                Destroy(chunk);

                SpawnChunk();
            }
        }
    }
}
