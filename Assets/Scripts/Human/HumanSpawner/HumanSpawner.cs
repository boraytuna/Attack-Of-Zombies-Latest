// using System.Collections;
// using System.Collections.Generic;
// using Unity.AI.Navigation;
// using UnityEngine;

// public class HumanSpawner : Spawner, ISpawner
// {
//     [Header("Prefab Paths")]
//     public string centralHumanTag = "CentralHuman"; // Tag used to identify the centralHuman pool
//     public string HumanTag = "Human"; // Tag used to identify the Human pool
//     //[SerializeField] private string centralHumanPrefabPath = "HumanPrefabs/CentralHuman";
//     //[SerializeField] private string humanPrefabPath = "HumanPrefabs/BasicHuman";
//     // private GameObject centralHumanPrefab;
//     // private GameObject humanPrefab;

//     private List<GameObject> centralHumans = new List<GameObject>();
//     [SerializeField] private ObjectPooler objectPooler;

//     private void Start()
//     {
//         objectPooler = GameObject.FindWithTag("PoolManager").GetComponent<ObjectPooler>();
//         if (objectPooler == null)
//         {
//             Debug.LogError("ObjectPooler reference is not assigned.");
//         }
//     }

//     private IEnumerator WaitForPooler()
//     {
//         yield return new WaitUntil(() => objectPooler.ArePrefabsLoaded());
//         Spawn();
//     }

//     private void OnEnable()
//     {
//         if (objectPooler == null)
//         {
//             Debug.LogError("ObjectPooler reference is not assigned in OnEnable.");
//             return;
//         }

//         objectPooler.OnPrefabsLoaded += Spawn;
//     }

//     private void OnDisable()
//     {
//         if (objectPooler != null)
//         {
//             objectPooler.OnPrefabsLoaded -= Spawn;
//         }
//     }
    
//     public override void Spawn()
//     {
//         //InitializePrefabs();
//         SpawnHumans();
//         ClearCentralHumans();
//     }

//     // private void InitializePrefabs()
//     // {     
//     //     centralHumanPrefab = Resources.Load<GameObject>(centralHumanPrefabPath);
//     //     if (centralHumanPrefab == null)
//     //     {
//     //         Debug.LogError("Central human prefab not found at path: " + centralHumanPrefabPath);
//     //     }

//     //     humanPrefab = Resources.Load<GameObject>(humanPrefabPath);
//     //     if (humanPrefab == null)
//     //     {
//     //         Debug.LogError("Human prefab not found at path: " + humanPrefabPath);
//     //     }
//     // }

//     void SpawnHumans()
//     {
//         // if (centralHumanPrefab == null || humanPrefab == null)
//         // {
//         //     Debug.LogError("One or more prefabs are not loaded. Aborting spawn.");
//         //     return;
//         // }

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

//     // void SpawnGroup(Vector3 center, int humansPerGroup, float groupRadius)
//     // {
//     //     // if (centralHumanPrefab == null || humanPrefab == null)
//     //     // {
//     //     //     Debug.LogError("One or more prefabs are not loaded. Aborting group spawn.");
//     //     //     return;
//     //     // }


//     //     Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);
//     //     // Spawn a new central human from the pool
//     //     GameObject newCentralHumanObject = objectPooler.SpawnFromPool(centralHumanTag, centralPosition, transform.rotation);
//     //     //GameObject centralHumanObject = Instantiate(centralHumanPrefab, centralPosition, Quaternion.identity);
//     //     newCentralHumanObject.GetComponent<HumanToZombie>().isCentralHuman = true;
//     //     centralHumans.Add(newCentralHumanObject);

//     //     for (int i = 1; i < humansPerGroup; i++)
//     //     {
//     //         Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
//     //         // Spawn a new central human from the pool
//     //         GameObject newHumanObject = objectPooler.SpawnFromPool(HumanTag, centralPosition, transform.rotation);
//     //         //Instantiate(humanPrefab, spawnPosition, Quaternion.identity);
//     //     }
//     // }

//     void SpawnGroup(Vector3 center, int humansPerGroup, float groupRadius)
//     {
//         Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);
//         //Debug.Log($"Spawning central human at: {centralPosition}");

//         GameObject newCentralHumanObject = ObjectPooler.Instance.SpawnFromPool(centralHumanTag, centralPosition, Quaternion.identity);
//         if (newCentralHumanObject == null)
//         {
//             Debug.LogError("Failed to spawn central human object.");
//             return;
//         }

//         newCentralHumanObject.GetComponent<HumanToZombie>().isCentralHuman = true;
//         centralHumans.Add(newCentralHumanObject);

//         for (int i = 1; i < humansPerGroup; i++)
//         {
//             Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
//             //Debug.Log($"Spawning human at: {spawnPosition}");

//             GameObject newHumanObject = ObjectPooler.Instance.SpawnFromPool(HumanTag, spawnPosition, Quaternion.identity);
//             if (newHumanObject == null)
//             {
//                 Debug.LogError("Failed to spawn human object.");
//             }
//         }
//     }


//     public void OnCentralHumanKilled(GameObject centralHuman)
//     {
//         // Implement logic here if needed
//         if (currentRespawns >= maxRespawns)
//         {
//             return;
//         }

//         Vector3 newGroupCenter = GetRandomSpawnPositionOnNavMesh();
//         int humansPerGroup = Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1);
//         float groupRadius = Random.Range(minGroupRadius, maxGroupRadius);

//         groupCenters.Add(newGroupCenter);
//         SpawnGroup(newGroupCenter, humansPerGroup, groupRadius);

//         Debug.Log("Respawning Humans");
//         currentRespawns++;
//     }

//     public void ClearCentralHumans()
//     {
//         centralHumans.Clear();
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : Spawner, ISpawner
{
    [Header("Prefab Tags")]
    public string centralHumanTag = "CentralHuman"; // Tag for central human pool
    public string humanTag = "Human"; // Tag for human pool

    private List<GameObject> centralHumans = new List<GameObject>();
    [SerializeField] private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = GameObject.FindWithTag("PoolManager").GetComponent<ObjectPooler>();
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler reference is not assigned.");
        }
    }

    private void OnEnable()
    {
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler reference is not assigned in OnEnable.");
            return;
        }

        objectPooler.OnPrefabsLoaded += Spawn;
    }

    private void OnDisable()
    {
        if (objectPooler != null)
        {
            objectPooler.OnPrefabsLoaded -= Spawn;
        }
    }

    public override void Spawn()
    {
        SpawnHumans();
        ClearCentralHumans();
    }

    void SpawnHumans()
    {
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

    private void SpawnGroup(Vector3 center, int humansPerGroup, float groupRadius)
    {
        Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);
        GameObject newCentralHumanObject = objectPooler.SpawnFromPool(centralHumanTag, centralPosition, Quaternion.identity);
        if (newCentralHumanObject != null)
        {
            newCentralHumanObject.GetComponent<HumanToZombie>().isCentralHuman = true;
            centralHumans.Add(newCentralHumanObject);
        }
        else
        {
            Debug.LogError("Failed to spawn central human object.");
        }

        for (int i = 1; i < humansPerGroup; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
            GameObject newHumanObject = objectPooler.SpawnFromPool(humanTag, spawnPosition, Quaternion.identity);
            if (newHumanObject == null)
            {
                Debug.LogError("Failed to spawn human object.");
            }
        }
    }

    public void OnCentralHumanKilled(GameObject centralHuman)
    {
        if (currentRespawns >= maxRespawns) return;

        Vector3 newGroupCenter = GetRandomSpawnPositionOnNavMesh();
        int humansPerGroup = Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1);
        float groupRadius = Random.Range(minGroupRadius, maxGroupRadius);

        groupCenters.Add(newGroupCenter);
        SpawnGroup(newGroupCenter, humansPerGroup, groupRadius);

        Debug.Log("Respawning Humans");
        currentRespawns++;
    }

    public void ClearCentralHumans()
    {
        centralHumans.Clear();
    }
}
