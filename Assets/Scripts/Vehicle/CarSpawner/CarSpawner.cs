// using UnityEngine;
// using System.Collections.Generic;

// public class CarSpawner : MonoBehaviour
// {
//     [SerializeField] private string[] prefabNames = { "AttackerPrefabs/Vehicles/PoliceCar", "AttackerPrefabs/Vehicles/SoldierTank" }; // Names of the prefabs in the Resources folder
//     [SerializeField] private Transform[] spawnPositions; // Array of predefined spawn positions
//     private List<Transform> occupiedPositions = new List<Transform>(); // List of positions already occupied

//     // This method is called to request a new spawn
//     public void RequestRespawn()
//     {
//         // Check if all positions are occupied
//         if (occupiedPositions.Count >= spawnPositions.Length)
//         {
//             Debug.LogWarning("All spawn positions are currently occupied.");
//             return;
//         }

//         // Randomly select a prefab from the array
//         int prefabIndex = Random.Range(0, prefabNames.Length);
//         GameObject prefab = Resources.Load<GameObject>(prefabNames[prefabIndex]);

//         if (prefab == null)
//         {
//             Debug.LogError($"Prefab with name {prefabNames[prefabIndex]} not found in Resources folder.");
//             return;
//         }

//         // Find an unoccupied spawn position
//         Transform spawnPosition = null;
//         int attemptCount = 0;
//         while (spawnPosition == null && attemptCount < spawnPositions.Length)
//         {
//             int positionIndex = Random.Range(0, spawnPositions.Length);
//             if (!occupiedPositions.Contains(spawnPositions[positionIndex]))
//             {
//                 spawnPosition = spawnPositions[positionIndex];
//             }
//             attemptCount++;
//         }

//         if (spawnPosition == null)
//         {
//             Debug.LogWarning("Failed to find an unoccupied spawn position.");
//             return;
//         }

//         // Spawn the selected prefab at the selected position
//         Instantiate(prefab, spawnPosition.position, spawnPosition.rotation);
//         occupiedPositions.Add(spawnPosition);
//     }
// }
using UnityEngine;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private string[] prefabNames = { "AttackerPrefabs/Vehicles/PoliceCar", "AttackerPrefabs/Vehicles/SoldierTank" }; // Names of the prefabs in the Resources folder
    [SerializeField] private Transform[] spawnPositions; // Array of predefined spawn positions
    [SerializeField] private int maxRespawns = 10; // Maximum number of respawns allowed
    private int currentRespawns = 0; // Current number of respawns
    private List<Transform> occupiedPositions = new List<Transform>(); // List of positions already occupied

    // This method is called to request a new spawn
    public void RequestRespawn()
    {
        // Check if the maximum number of respawns has been reached
        if (currentRespawns >= maxRespawns)
        {
            Debug.LogWarning("Maximum number of respawns reached.");
            return;
        }

        // Check if all positions are occupied
        if (occupiedPositions.Count >= spawnPositions.Length)
        {
            Debug.LogWarning("All spawn positions are currently occupied.");
            return;
        }

        // Randomly select a prefab from the array
        int prefabIndex = Random.Range(0, prefabNames.Length);
        GameObject prefab = Resources.Load<GameObject>(prefabNames[prefabIndex]);

        if (prefab == null)
        {
            Debug.LogError($"Prefab with name {prefabNames[prefabIndex]} not found in Resources folder.");
            return;
        }

        // Find an unoccupied spawn position
        Transform spawnPosition = null;
        int attemptCount = 0;
        while (spawnPosition == null && attemptCount < spawnPositions.Length)
        {
            int positionIndex = Random.Range(0, spawnPositions.Length);
            if (!occupiedPositions.Contains(spawnPositions[positionIndex]))
            {
                spawnPosition = spawnPositions[positionIndex];
            }
            attemptCount++;
        }

        if (spawnPosition == null)
        {
            Debug.LogWarning("Failed to find an unoccupied spawn position.");
            return;
        }

        // Spawn the selected prefab at the selected position
        Instantiate(prefab, spawnPosition.position, spawnPosition.rotation);
        occupiedPositions.Add(spawnPosition);
        
        // Increment the respawn count
        currentRespawns++;
    }
}