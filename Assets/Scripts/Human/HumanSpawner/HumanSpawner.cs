using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class HumanSpawner : Spawner, ISpawner
{
    [Header("Prefab Paths")]
    [SerializeField] private string centralHumanPrefabPath = "HumanPrefabs/CentralHuman";
    [SerializeField] private string humanPrefabPath = "HumanPrefabs/BasicHuman";

    private GameObject centralHumanPrefab;
    private GameObject humanPrefab;

    private List<GameObject> centralHumans = new List<GameObject>();

    void Start()
    {
        Spawn();
    }
    
    public override void Spawn()
    {
        InitializePrefabs();
        SpawnHumans();
        ClearCentralHumans();
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

    void SpawnHumans()
    {
        if (centralHumanPrefab == null || humanPrefab == null)
        {
            Debug.LogError("One or more prefabs are not loaded. Aborting spawn.");
            return;
        }

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

        Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);
        GameObject centralHumanObject = Instantiate(centralHumanPrefab, centralPosition, Quaternion.identity);
        centralHumanObject.GetComponent<HumanToZombie>().isCentralHuman = true;
        centralHumans.Add(centralHumanObject);

        for (int i = 1; i < humansPerGroup; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
            Instantiate(humanPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void OnCentralHumanKilled(GameObject centralHuman)
    {
        // Implement logic here if needed
        if (currentRespawns >= maxRespawns)
        {
            return;
        }

        Vector3 newGroupCenter = GetRandomSpawnPositionOnNavMesh();
        int humansPerGroup = Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1);
        float groupRadius = Random.Range(minGroupRadius, maxGroupRadius);

        groupCenters.Add(newGroupCenter);
        SpawnGroup(newGroupCenter, humansPerGroup, groupRadius);

        currentRespawns++;
    }

    public void ClearCentralHumans()
    {
        centralHumans.Clear();
    }
}
