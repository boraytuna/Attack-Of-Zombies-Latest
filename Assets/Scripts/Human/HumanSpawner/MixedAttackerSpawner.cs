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

    private Dictionary<int, GameObject> centralAttackersPerGroup = new Dictionary<int, GameObject>();

    public override void Spawn()
    {
        InitializePrefabs();
        SpawnAttackers();
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

    void SpawnAttackers()
    {
        // Ensure prefabs are loaded
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
        // Ensure prefabs are loaded
        if (centralPrefab == null || attackerPrefab == null)
        {
            Debug.LogError("One or more prefabs are not loaded.");
            return;
        }

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
