using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MixedAttackerSpawner : Spawner, ISpawner
{
    [Header("Prefabs Tags")]
    public string centralPoliceTag = "CentralPolice"; // Tag used to identify the central police pool
    public string policeTag = "Police"; // Tag used to identify the police pool
    public string centralSoldierTag = "CentralSoldier"; // Tag used to identify the central soldier pool
    public string soldierTag = "Soldier"; // Tag used to identify the soldier pool

    private List<GameObject> centralAttackers = new List<GameObject>();

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
        ClearCentralAttackers();
        SpawnAttackers();
    }

    void SpawnAttackers()
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

            if (Random.value > 0.5f)
            {
                SpawnGroup(groupCenter, Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1), Random.Range(minGroupRadius, maxGroupRadius), centralPoliceTag, policeTag);
            }
            else
            {
                SpawnGroup(groupCenter, Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1), Random.Range(minGroupRadius, maxGroupRadius), centralSoldierTag, soldierTag);
            }
        }
    }

    private void SpawnGroup(Vector3 center, int attackersPerGroup, float groupRadius, string centralPrefabTag, string attackerPrefabTag)
    {
        Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);

        // Spawn central attacker
        GameObject centralAttackerObject = objectPooler.SpawnFromPool(centralPrefabTag, centralPosition, Quaternion.identity);
        if (centralAttackerObject != null)
        {
            centralAttackers.Add(centralAttackerObject);
        }

        for (int i = 1; i < attackersPerGroup; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
            GameObject attackerObject = objectPooler.SpawnFromPool(attackerPrefabTag, spawnPosition, Quaternion.identity);
        }
    }

    public void OnCentralAttackerKilled(GameObject centralAttacker)
    {
        if (currentRespawns >= maxRespawns)
        {
            return;
        }

        Vector3 newGroupCenter = GetRandomSpawnPositionOnNavMesh();
        int attackersPerGroup = Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1);
        float groupRadius = Random.Range(minGroupRadius, maxGroupRadius);

        groupCenters.Add(newGroupCenter);

        if (Random.value > 0.5f)
        {
            SpawnGroup(newGroupCenter, attackersPerGroup, groupRadius, centralPoliceTag, policeTag);
        }
        else
        {
            SpawnGroup(newGroupCenter, attackersPerGroup, groupRadius, centralSoldierTag, soldierTag);
        }

        Debug.Log("Respawning Attackers");
        currentRespawns++;
    }

    public void ClearCentralAttackers()
    {
        centralAttackers.Clear();
    }
}
