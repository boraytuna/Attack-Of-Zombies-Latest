using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MixedAttackerSpawner : Spawner, ISpawner
{
    [Header("Prefabs")]
    [SerializeField] private string centralPolicePrefabPath = "AttackerPrefabs/CentralPolice";
    [SerializeField] private string policePrefabPath = "AttackerPrefabs/Police";
    [SerializeField] private string centralSoldierPrefabPath = "AttackerPrefabs/CentralSoldier";
    [SerializeField] private string soldierPrefabPath = "AttackerPrefabs/Soldier";

    private GameObject centralPolicePrefab;
    private GameObject policePrefab;
    private GameObject centralSoldierPrefab;
    private GameObject soldierPrefab;

    private List<GameObject> centralAttackers = new List<GameObject>();

    void Start()
    {
        Spawn();
    }

    public override void Spawn()
    {
        InitializePrefabs();
        SpawnAttackers();
        ClearCentralAttackers();
    }

    private void InitializePrefabs()
    {
        centralPolicePrefab = Resources.Load<GameObject>(centralPolicePrefabPath);
        if (centralPolicePrefab == null)
        {
            Debug.LogError("Central police prefab not found at path: " + centralPolicePrefabPath);
        }

        policePrefab = Resources.Load<GameObject>(policePrefabPath);
        if (policePrefab == null)
        {
            Debug.LogError("Police prefab not found at path: " + policePrefabPath);
        }

        centralSoldierPrefab = Resources.Load<GameObject>(centralSoldierPrefabPath);
        if (centralSoldierPrefab == null)
        {
            Debug.LogError("Central soldier prefab not found at path: " + centralSoldierPrefabPath);
        }

        soldierPrefab = Resources.Load<GameObject>(soldierPrefabPath);
        if (soldierPrefab == null)
        {
            Debug.LogError("Soldier prefab not found at path: " + soldierPrefabPath);
        }
    }

    void SpawnAttackers()
    {
        if (centralPolicePrefab == null || policePrefab == null || centralSoldierPrefab == null || soldierPrefab == null)
        {
            Debug.LogError("One or more prefabs are not loaded.");
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

            if (Random.value > 0.5f)
            {
                SpawnGroup(groupCenter, Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1), Random.Range(minGroupRadius, maxGroupRadius), centralPolicePrefab, policePrefab);
            }
            else
            {
                SpawnGroup(groupCenter, Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1), Random.Range(minGroupRadius, maxGroupRadius), centralSoldierPrefab, soldierPrefab);
            }
        }
    }

    private void SpawnGroup(Vector3 center, int attackersPerGroup, float groupRadius, GameObject centralPrefab, GameObject attackerPrefab)
    {
        if (centralPrefab == null || attackerPrefab == null)
        {
            Debug.LogError("One or more prefabs are not loaded.");
            return;
        }

        Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);
        GameObject centralAttackerObject = Instantiate(centralPrefab, centralPosition, Quaternion.identity);
        centralAttackers.Add(centralAttackerObject);

        for (int i = 1; i < attackersPerGroup; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
            Instantiate(attackerPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void OnCentralAttackerKilled(GameObject centralAttacker)
    {
        // Implement logic here if needed
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
            SpawnGroup(newGroupCenter, attackersPerGroup, groupRadius, centralPolicePrefab, policePrefab);
        }
        else
        {
            SpawnGroup(newGroupCenter, attackersPerGroup, groupRadius, centralSoldierPrefab, soldierPrefab);
        }

        currentRespawns++;
    }

    public void ClearCentralAttackers()
    {
        centralAttackers.Clear();
    }
}
