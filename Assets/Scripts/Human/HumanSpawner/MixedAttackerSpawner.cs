using System.Collections.Generic;
using UnityEngine;

public class MixedAttackerSpawner : Spawner, ISpawner
{
    [Header("Prefabs")]
    [SerializeField] private GameObject centralPolicePrefab;
    [SerializeField] private GameObject policePrefab;
    [SerializeField] private GameObject centralSoldierPrefab;
    [SerializeField] private GameObject soldierPrefab;

    private Dictionary<int, GameObject> centralAttackersPerGroup = new Dictionary<int, GameObject>();

    public void Initialize()
    {
        Spawn();
    }

    public override void Spawn()
    {
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

            // Randomly choose between spawning a police or soldier group
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
        // Spawn the central attacker first
        Vector3 centralPosition = GetValidSpawnPosition(center, groupRadius);
        GameObject centralAttackerObject = Instantiate(centralPrefab, centralPosition, Quaternion.identity);
        AddToCentralAttackerList(centralAttackerObject);

        // Spawn the other attackers in the group
        for (int i = 1; i < attackersPerGroup; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(center, groupRadius);
            Instantiate(attackerPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach (Vector3 groupCenter in groupCenters)
        {
            Gizmos.DrawWireSphere(groupCenter, minGroupSeparationDistance);
        }
    }

    private void AddToCentralAttackerList(GameObject centralAttacker)
    {
        int instanceID = centralAttacker.GetInstanceID();
        if (!centralAttackersPerGroup.ContainsKey(instanceID))
        {
            centralAttackersPerGroup.Add(instanceID, centralAttacker);
        }
        else
        {
            Debug.LogError("Trying to add a central attacker that already exists in the list.");
        }
    }

    private void RemoveFromCentralAttackerList(GameObject centralAttacker)
    {
        int instanceID = centralAttacker.GetInstanceID();
        if (centralAttackersPerGroup.ContainsKey(instanceID))
        {
            centralAttackersPerGroup.Remove(instanceID);
        }
        else
        {
            Debug.LogWarning("Attempted to remove a central attacker that does not exist in the list. Instance ID: " + instanceID);
        }
    }

    // Implementing the method for handling central human killed
    public void OnCentralAttackerKilled(GameObject centralHuman)
    {
        RemoveFromCentralAttackerList(centralHuman);

        if (currentRespawns >= maxRespawns)
        {
            return; // Do not spawn a new group if the limit is reached
        }

        Vector3 newGroupCenter = GetRandomSpawnPositionOnNavMesh();
        int attackersPerGroup = Random.Range(minEntitiesPerGroup, maxEntitiesPerGroup + 1);
        float groupRadius = Random.Range(minGroupRadius, maxGroupRadius);

        groupCenters.Add(newGroupCenter);

        // Randomly choose between spawning a police or soldier group
        if (Random.value > 0.5f)
        {
            SpawnGroup(newGroupCenter, attackersPerGroup, groupRadius, centralPolicePrefab, policePrefab);
        }
        else
        {
            SpawnGroup(newGroupCenter, attackersPerGroup, groupRadius, centralSoldierPrefab, soldierPrefab);
        }

        currentRespawns++; // Increment the respawn count
    }
}