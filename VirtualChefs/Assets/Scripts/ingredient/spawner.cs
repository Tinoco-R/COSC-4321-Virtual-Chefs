using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Array of object prefabs to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float respawnTime = 3f; // Time before respawning an object

    private GameObject[] spawnedObjects; // Array to hold spawned objects
    private Vector3[] lastObjectPositions; // Array to store last known positions of spawned objects

    void Start()
    {
        // Array to hold spawned objects will now be as big as the amount of spawn points there is for our script
        spawnedObjects = new GameObject[spawnPoints.Length];
        // Array to hold last known positions will now be as big as the amount of spawn points there is for our script
        lastObjectPositions = new Vector3[spawnPoints.Length];
        SpawnInitialObjects();
    }

    void SpawnInitialObjects()
    {
        // For each of the spawn points spawn a prefab
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnObject(i);
        }
    }

    void SpawnObject(int index)
    {
        // Instantiate one of the prefabs from the list with the random index and select a spawn point position with the random index too
        GameObject newObject = Instantiate(objectPrefabs[index], spawnPoints[index].position, Quaternion.identity);
        // Keep track of the newly spawned object by placing it in the spawnedObjects list
        spawnedObjects[index] = newObject;
        // Keep track of the newly spawned object position by placing it in the lastObjectPositions list
        lastObjectPositions[index] = newObject.transform.position;
    }

    IEnumerator RespawnObject(int index)
    {
        yield return new WaitForSeconds(respawnTime);
        if (spawnedObjects[index] == null)
        {
            SpawnObject(index);
        }
    }

    void Update()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnedObjects[i] != null)
            {
                // Check if the object's position has changed since the last update
                if (spawnedObjects[i].transform.position != lastObjectPositions[i])
                {
                    spawnedObjects[i] = null; // Mark the object as removed
                    StartCoroutine(RespawnObject(i)); // Start the respawn coroutine
                }
                lastObjectPositions[i] = spawnedObjects[i].transform.position; // Update last known position
            }
        }
    }
}
