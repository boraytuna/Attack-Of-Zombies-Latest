using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCarSpawner : MonoBehaviour
{
    [Header("Car Prefab")]
    [SerializeField] private string policeCarPrefabPath = "AttackerPrefabs/PoliceCar";
    private GameObject policeCarPrefab;
    [SerializeField] private float minDistanceFromPlayer = 20f; // Minimum distance from the player for spawning
    [SerializeField] private float checkRadius = 15f; // Radius for checking nearby objects to avoid collision
    [SerializeField] private float movementRadius = 10f; // Radius of the circular movement path

    [Header("Spawn Settings")]
    [SerializeField] private int maxCars = 5; // Maximum number of police cars to spawn
    [SerializeField] private int maxRespawns = 3; // Maximum number of respawns allowed
    [SerializeField] private float initialDelay = 2f; // Delay before starting to spawn cars
    [SerializeField] private int maxSpawnAttempts = 100; // Maximum attempts to find a valid spawn position

    private Transform playerTransform;
    private int currentCarCount = 0; // Current number of cars
    private int respawnCount = 0;    // Count of respawns

    void Start()
    {
        LoadCarPrefab();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has the "Player" tag
        StartCoroutine(WaitForHumansAndSpawnCars());
    }

    private void LoadCarPrefab()
    {
        policeCarPrefab = Resources.Load<GameObject>(policeCarPrefabPath);
        if (policeCarPrefab == null)
        {
            Debug.LogError("Police car prefab not found at path: " + policeCarPrefab);
        }
    }

    private IEnumerator WaitForHumansAndSpawnCars()
    {
        yield return new WaitForSeconds(initialDelay); // Wait for human objects to spawn

        // Spawn initial cars
        int spawnAttempts = 0;
        while (currentCarCount < maxCars && spawnAttempts < maxSpawnAttempts)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                Instantiate(policeCarPrefab, spawnPosition, Quaternion.identity);
                currentCarCount++;
            }
            else
            {
                spawnAttempts++;
                if (spawnAttempts >= maxSpawnAttempts)
                {
                    Debug.LogWarning("Failed to find a valid spawn position for police car after maximum attempts.");
                }
            }
        }
    }

    public void RequestRespawn()
    {
        if (respawnCount < maxRespawns)
        {
            int spawnAttempts = 0;
            Vector3 spawnPosition = Vector3.zero;
            while (spawnAttempts < maxSpawnAttempts)
            {
                spawnPosition = GetValidSpawnPosition();
                if (spawnPosition != Vector3.zero)
                {
                    Instantiate(policeCarPrefab, spawnPosition, Quaternion.identity);
                    currentCarCount++;
                    respawnCount++;
                    return; // Exit after successful spawn
                }
                spawnAttempts++;
            }

            Debug.LogWarning("Failed to find a valid spawn position for police car during respawn.");
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        const int maxAttempts = 100;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            Vector3 randomPoint = GetRandomPointOnNavMesh();

            // Check if the point is far enough from the player and clear of tagged objects
            if (randomPoint != Vector3.zero &&
                Vector3.Distance(randomPoint, playerTransform.position) >= minDistanceFromPlayer &&
                !IsPathObstructed(randomPoint))
            {
                return randomPoint;
            }

            attempts++;
        }

        Debug.LogWarning("Max attempts reached while trying to find a valid spawn position for police car.");
        return Vector3.zero;
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 100f; // Adjust the range as needed
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }

    private bool IsPathObstructed(Vector3 position)
    {
        // Check if the car's circular movement path from the proposed spawn position intersects with any objects
        const int angleStep = 10; // Check every 10 degrees
        for (int angle = 0; angle < 360; angle += angleStep)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 point = position + direction * movementRadius;

            // Check if there are any colliders within the check radius at the given point
            Collider[] colliders = Physics.OverlapSphere(point, checkRadius);
            foreach (Collider collider in colliders)
            {
                // Check if the collider has one of the specified tags
                if (collider.CompareTag("Human") || collider.CompareTag("Building"))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
