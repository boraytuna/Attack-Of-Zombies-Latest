using UnityEngine;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private string[] prefabTags = { "PoliceCar", "SoldierTank" }; // Tags for the prefabs in the ObjectPooler
    [SerializeField] private Transform[] spawnPositions; // Array of predefined spawn positions
    [SerializeField] private int maxStartVehicles; // Maximum number of vehicles to spawn at the start
    private int currentStartVehicles = 0; // Counter for vehicles spawned at start
    private List<Transform> occupiedPositions = new List<Transform>(); // List of positions already occupied

    private ObjectPooler objectPooler;

    private void Start()
    {
        // Get the ObjectPooler instance
        objectPooler = ObjectPooler.Instance;

        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler instance not found.");
            return;
        }

        // Spawn vehicles at the start of the game with a limit
        SpawnVehiclesAtStart();
    }

    private void SpawnVehiclesAtStart()
    {
        // Shuffle the spawn positions array to randomize the starting positions
        Shuffle(spawnPositions);

        foreach (Transform spawnPosition in spawnPositions)
        {
            if (currentStartVehicles >= maxStartVehicles)
                break;

            // Randomly select a prefab tag from the array
            int prefabIndex = Random.Range(0, prefabTags.Length);
            string prefabTag = prefabTags[prefabIndex];

            // Spawn the selected prefab from the pool
            GameObject pooledObject = objectPooler.SpawnFromPool(prefabTag, spawnPosition.position, spawnPosition.rotation);

            if (pooledObject == null)
            {
                Debug.LogWarning($"Failed to spawn object with tag {prefabTag} from pool.");
            }
            else
            {
                // Keep track of the number of vehicles spawned at start
                currentStartVehicles++;
                occupiedPositions.Add(spawnPosition);
                Debug.Log($"Spawned object with tag {prefabTag} at position {spawnPosition.position}.");
            }
        }
    }

    // Utility function to shuffle an array
    private void Shuffle(Transform[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
