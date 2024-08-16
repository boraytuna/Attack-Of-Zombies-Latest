using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public abstract class Spawner : MonoBehaviour, ISpawner
{
    [Header("Zombie Position")]
    [SerializeField] protected Transform target; // Reference to the target object (e.g., player or a specific point)
    [SerializeField] protected float minDistanceFromTarget; // Minimum distance from the target

    [Header("Spawn Details")]
    [SerializeField] protected int minNumberOfGroups;
    [SerializeField] protected int maxNumberOfGroups;
    [SerializeField] protected int minEntitiesPerGroup;
    [SerializeField] protected int maxEntitiesPerGroup;
    [SerializeField] protected float minGroupRadius;
    [SerializeField] protected float maxGroupRadius;
    [SerializeField] public float minGroupSeparationDistance = 17.5f; // Minimum distance between groups
    [SerializeField] protected int maxRespawns; // Maximum number of times a new group can be spawned

    [Header("Navmesh")]
    public NavMeshSurface navMeshSurface;

    protected int currentRespawns = 0; // Current number of respawns that have occurred
    protected List<Vector3> groupCenters = new List<Vector3>();
    
    public abstract void Spawn();

    protected Vector3 GetValidSpawnPosition(Vector3 center, float groupRadius)
    {
        Vector3 spawnPosition = center;
        NavMeshHit hit = new NavMeshHit();
        int attempts = 0;
        const int maxAttempts = 100;

        while (attempts < maxAttempts)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(0f, groupRadius);
            Vector3 randomOffset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
            spawnPosition = center + randomOffset;

            if (Vector3.Distance(spawnPosition, target.position) >= minDistanceFromTarget && NavMesh.SamplePosition(spawnPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                //Debug.Log($"Spawn Position Found: {hit.position}, Distance from Target: {Vector3.Distance(hit.position, target.position)}");
                return hit.position;
            }

            attempts++;
        }

        Debug.LogWarning("Max attempts reached while trying to find a valid spawn position. Using the last tried position.");
        return hit.position;
    }

    protected Vector3 GetRandomSpawnPositionOnNavMesh()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        Vector3 randomPoint = Vector3.zero;
        int attempts = 0;
        const int maxAttempts = 100;

        // Pick a random triangle from the NavMesh and ensure it's not too close to the target
        while (attempts < maxAttempts)
        {
            int randomTriangleIndex = Random.Range(0, navMeshData.indices.Length / 3);
            randomPoint = (
                navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3]] +
                navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3 + 1]] +
                navMeshData.vertices[navMeshData.indices[randomTriangleIndex * 3 + 2]]
            ) / 3f;

            if (Vector3.Distance(randomPoint, target.position) >= minDistanceFromTarget)
            {
                return randomPoint;
            }

            attempts++;
        }

        Debug.LogWarning("Max attempts reached while trying to find a valid random spawn position. Using the last tried position.");
        return randomPoint; // Return the last position checked
    }

}
