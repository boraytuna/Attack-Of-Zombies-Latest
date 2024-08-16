using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class HumanSpawner : Spawner, ISpawner
{
    [Header("Prefab Paths")]
    public string centralHumanTag = "CentralHuman"; // Tag used to identify the centralHuman pool
    public string HumanTag = "BasicHuman"; // Tag used to identify the Human pool

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
        ClearCentralHumans();
          SpawnHumans();
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
            SpawnGroup(groupCenter, Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1), Random.Range(minGroupRadius, maxGroupRadius), centralHumanTag, HumanTag);
        }
    }

    private void SpawnGroup(Vector3 center, int humansPerGroup, float groupRadius, string centralHumanTag, string HumanTag)
    {
        Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);

        GameObject centralHumanObject = objectPooler.SpawnFromPool(centralHumanTag, centralPosition, Quaternion.identity);
        centralHumanObject.GetComponent<HumanToZombie>().isCentralHuman = true;
        centralHumans.Add(centralHumanObject);

        for (int i = 1; i < humansPerGroup; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
            GameObject humanObject = objectPooler.SpawnFromPool(HumanTag, spawnPosition, Quaternion.identity);
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
        SpawnGroup(newGroupCenter, humansPerGroup, groupRadius, centralHumanTag, HumanTag);

        currentRespawns++;
    }

    public void ClearCentralHumans()
    {
        centralHumans.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach (Vector3 groupCenter in groupCenters)
        {
            Gizmos.DrawWireSphere(groupCenter, minGroupSeparationDistance);
        }
    }
}