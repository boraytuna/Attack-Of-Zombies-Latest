// using System.Collections.Generic;
// using Unity.AI.Navigation;
// using UnityEngine;
// using UnityEngine.AI;

// public class HumanSpawner : Spawner, ISpawner
// {
//     [Header("Prefab Paths")]
//     [SerializeField] private string centralHumanPrefabPath = "HumanPrefabs/CentralHuman";
//     [SerializeField] private string humanPrefabPath = "HumanPrefabs/BasicHuman";

//     private GameObject centralHumanPrefab;
//     private GameObject humanPrefab;

//     private Dictionary<int, GameObject> centralHumansPerGroup = new Dictionary<int, GameObject>();

//     public override void Spawn()
//     {
//         InitializePrefabs();
//         SpawnHumans();
//     }

//     private void InitializePrefabs()
//     {   
        
//         centralHumanPrefab = Resources.Load<GameObject>(centralHumanPrefabPath);
//         if (centralHumanPrefab == null)
//         {
//             Debug.LogError("Central human prefab not found at path: " + centralHumanPrefabPath);
//         }

//         humanPrefab = Resources.Load<GameObject>(humanPrefabPath);
//         if (humanPrefab == null)
//         {
//             Debug.LogError("Human prefab not found at path: " + humanPrefabPath);
//         }
//     }

//     void SpawnHumans()
//     {
//         if (centralHumanPrefab == null || humanPrefab == null)
//         {
//             Debug.LogError("One or more prefabs are not loaded. Aborting spawn.");
//             return;
//         }

//         navMeshSurface.BuildNavMesh();

//         int numberOfGroups = Random.Range(minNumberOfGroups, maxNumberOfGroups + 1);
//         for (int i = 0; i < numberOfGroups; i++)
//         {
//             int attempts = 0;
//             const int maxAttempts = 100;
//             Vector3 groupCenter;
//             bool tooClose;
//             do
//             {
//                 groupCenter = GetRandomSpawnPositionOnNavMesh();
//                 tooClose = false;
//                 foreach (Vector3 existingCenter in groupCenters)
//                 {
//                     if (Vector3.Distance(groupCenter, existingCenter) < minGroupSeparationDistance)
//                     {
//                         tooClose = true;
//                         break;
//                     }
//                 }
//                 attempts++;
//                 if (attempts >= maxAttempts)
//                 {
//                     Debug.LogWarning("Max attempts reached, breaking out of loop to avoid infinite loop.");
//                     break;
//                 }
//             } while (tooClose);

//             if (tooClose) continue;

//             groupCenters.Add(groupCenter);
//             SpawnGroup(groupCenter, Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1), Random.Range(minGroupRadius, maxGroupRadius));
//         }
//     }

//     void SpawnGroup(Vector3 center, int humansPerGroup, float groupRadius)
//     {
//         if (centralHumanPrefab == null || humanPrefab == null)
//         {
//             Debug.LogError("One or more prefabs are not loaded. Aborting group spawn.");
//             return;
//         }

//         // Spawn the central human first
//         Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);
//         GameObject centralHumanObject = Instantiate(centralHumanPrefab, centralPosition, Quaternion.identity);
//         centralHumanObject.GetComponent<HumanToZombie>().isCentralHuman = true;
//         AddToCentralHumanList(centralHumanObject);

//         // Spawn the other humans in the group
//         for (int i = 1; i < humansPerGroup; i++)
//         {
//             Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
//             Instantiate(humanPrefab, spawnPosition, Quaternion.identity);
//         }
//     }

//     private void AddToCentralHumanList(GameObject centralHuman)
//     {
//         int instanceID = centralHuman.GetInstanceID();
//         if (!centralHumansPerGroup.ContainsKey(instanceID))
//         {
//             centralHumansPerGroup.Add(instanceID, centralHuman);
//         }
//         else
//         {
//             Debug.LogError("Trying to add a central human that already exists in the list.");
//         }
//     }

//     private void RemoveFromCentralHumanList(GameObject centralHuman)
//     {
//         int instanceID = centralHuman.GetInstanceID();
//         if (centralHumansPerGroup.ContainsKey(instanceID))
//         {
//             centralHumansPerGroup.Remove(instanceID);
//         }
//         else
//         {
//             Debug.LogWarning("Attempted to remove a central human that does not exist in the list. Instance ID: " + instanceID);
//         }
//     }

//     public void OnCentralHumanKilled(GameObject centralHuman)
//     {
//         RemoveFromCentralHumanList(centralHuman);

//         // Check if the maximum number of respawns has been reached
//         if (currentRespawns >= maxRespawns)
//         {
//             return; // Do not spawn a new group if the limit is reached
//         }

//         Vector3 newGroupCenter = GetRandomSpawnPositionOnNavMesh();
//         int humansPerGroup = Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1);
//         float groupRadius = Random.Range(minGroupRadius, maxGroupRadius);

//         groupCenters.Add(newGroupCenter);
//         SpawnGroup(newGroupCenter, humansPerGroup, groupRadius);

//         currentRespawns++; // Increment the respawn count
//     }
// }
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class HumanSpawner : Spawner, ISpawner
{
    [Header("Prefab Paths")]
    [SerializeField] private string centralHumanPrefabPath = "HumanPrefabs/CentralHuman";
    [SerializeField] private string humanPrefabPath = "HumanPrefabs/BasicHuman";

    private GameObject centralHumanPrefab;
    private GameObject humanPrefab;

    private Dictionary<int, GameObject> centralHumansPerGroup = new Dictionary<int, GameObject>();

    public override void Spawn()
    {
        InitializePrefabs();
        SpawnHumans();
    }

    private void InitializePrefabs()
    {     
        centralHumanPrefab = Resources.Load<GameObject>(centralHumanPrefabPath);
        if (centralHumanPrefab == null)
        {
            Debug.LogError("Central human prefab not found at path: " + centralHumanPrefabPath);
        }

        humanPrefab = Resources.Load<GameObject>(humanPrefabPath);
        if (humanPrefab == null)
        {
            Debug.LogError("Human prefab not found at path: " + humanPrefabPath);
        }
    }
    public void FindPlayerAndNavMesh()
    {
        // Example of finding player transform (you can adjust this based on your hierarchy):
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Ensure the player is tagged with 'Player' tag.");
        }

        // Example of finding NavMeshSurface (you can adjust this based on your setup):
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface not found in the scene.");
        }
    }

    void SpawnHumans()
    {
        if (centralHumanPrefab == null || humanPrefab == null)
        {
            Debug.LogError("One or more prefabs are not loaded. Aborting spawn.");
            return;
        }

        navMeshSurface.BuildNavMesh();

        int numberOfGroups = Random.Range(minNumberOfGroups, maxNumberOfGroups + 1);
        for (int i = 0; i < numberOfGroups; i++)
        {
            int attempts = 0;
            const int maxAttempts = 100;
            Vector3 groupCenter;
            bool tooClose;
            do
            {
                groupCenter = GetRandomSpawnPositionOnNavMesh();
                tooClose = false;
                foreach (Vector3 existingCenter in groupCenters)
                {
                    if (Vector3.Distance(groupCenter, existingCenter) < minGroupSeparationDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }
                attempts++;
                if (attempts >= maxAttempts)
                {
                    Debug.LogWarning("Max attempts reached, breaking out of loop to avoid infinite loop.");
                    break;
                }
            } while (tooClose);

            if (tooClose) continue;

            groupCenters.Add(groupCenter);
            SpawnGroup(groupCenter, Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1), Random.Range(minGroupRadius, maxGroupRadius));
        }
    }

    void SpawnGroup(Vector3 center, int humansPerGroup, float groupRadius)
    {
        if (centralHumanPrefab == null || humanPrefab == null)
        {
            Debug.LogError("One or more prefabs are not loaded. Aborting group spawn.");
            return;
        }

        // Spawn the central human first
        Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);
        GameObject centralHumanObject = Instantiate(centralHumanPrefab, centralPosition, Quaternion.identity);
        centralHumanObject.GetComponent<HumanToZombie>().isCentralHuman = true;
        AddToCentralHumanList(centralHumanObject);

        // Spawn the other humans in the group
        for (int i = 1; i < humansPerGroup; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
            Instantiate(humanPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void AddToCentralHumanList(GameObject centralHuman)
    {
        int instanceID = centralHuman.GetInstanceID();
        if (!centralHumansPerGroup.ContainsKey(instanceID))
        {
            centralHumansPerGroup.Add(instanceID, centralHuman);
        }
        else
        {
            Debug.LogError("Trying to add a central human that already exists in the list.");
        }
    }

    private void RemoveFromCentralHumanList(GameObject centralHuman)
    {
        int instanceID = centralHuman.GetInstanceID();
        if (centralHumansPerGroup.ContainsKey(instanceID))
        {
            centralHumansPerGroup.Remove(instanceID);
        }
        else
        {
            Debug.LogWarning("Attempted to remove a central human that does not exist in the list. Instance ID: " + instanceID);
        }
    }

    public void OnCentralHumanKilled(GameObject centralHuman)
    {
        RemoveFromCentralHumanList(centralHuman);

        // Check if the maximum number of respawns has been reached
        if (currentRespawns >= maxRespawns)
        {
            return; // Do not spawn a new group if the limit is reached
        }

        Vector3 newGroupCenter = GetRandomSpawnPositionOnNavMesh();
        int humansPerGroup = Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1);
        float groupRadius = Random.Range(minGroupRadius, maxGroupRadius);

        groupCenters.Add(newGroupCenter);
        SpawnGroup(newGroupCenter, humansPerGroup, groupRadius);

        currentRespawns++; // Increment the respawn count
    }
}
