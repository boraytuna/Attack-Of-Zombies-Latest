using UnityEngine;
using UnityEngine.AI;

public class EscapePointCalculator : MonoBehaviour
{
    public float escapeDistanceMultiplier = 10f; // Multiplier for calculating escape point distance from zombie
    public float maxSampleDistance = 10f; // Maximum distance to sample for NavMesh
    public float boundaryCheckDistance = 10f; // Distance for boundary checking with raycast
    public LayerMask boundaryLayerMask; // Layer mask for detecting boundaries

    private GameObject centralHuman; // Reference to the central game object

    public void SetCentralHuman(GameObject human)
    {
        this.centralHuman = human;
    }

    private void OnEnable()
    {
        ZombieDetection.OnZombieDetected += CalculateEscapePoint; // Subscribe to zombie detection event
    }

    private void OnDisable()
    {
        ZombieDetection.OnZombieDetected -= CalculateEscapePoint; // Unsubscribe from zombie detection event
    }

    private void CalculateEscapePoint(Vector3 zombiePosition)
    {
        Vector3 directionToZombie = transform.position - zombiePosition; // Calculate direction from escape point to zombie
        Vector3 rawEscapePoint = transform.position + directionToZombie.normalized * escapeDistanceMultiplier; // Calculate raw escape point

        // Check if the escape point is on the NavMesh
        if (NavMesh.SamplePosition(rawEscapePoint, out NavMeshHit hit, maxSampleDistance, NavMesh.AllAreas) && !IsOutsideBoundary(hit.position))
        {
            HumanMovement.SetEscapePoint(hit.position); // Set escape point for humans
        }
        else
        {
            // If the initial point is not valid, adjust it
            Vector3 validEscapePoint = FindValidEscapePoint(directionToZombie);
            HumanMovement.SetEscapePoint(validEscapePoint); // Set adjusted escape point for humans
        }
    }

    private Vector3 FindValidEscapePoint(Vector3 directionToZombie)
    {
        Vector3 basePoint = transform.position;
        for (float multiplier = escapeDistanceMultiplier; multiplier > 0; multiplier -= 1f)
        {
            Vector3 candidatePoint = basePoint + directionToZombie.normalized * multiplier;
            if (NavMesh.SamplePosition(candidatePoint, out NavMeshHit hit, maxSampleDistance, NavMesh.AllAreas) && !IsOutsideBoundary(hit.position))
            {
                return hit.position; // Return valid escape point found on NavMesh
            }
        }
        // Return current position if no valid point is found
        return basePoint;
    }

    private bool IsOutsideBoundary(Vector3 position)
    {
        // Cast a ray in the direction of the calculated escape point
        if (Physics.Raycast(transform.position, position - transform.position, out RaycastHit hit, boundaryCheckDistance, boundaryLayerMask))
        {
            // If the ray hits something within the boundary layer, it means the point is outside the boundary
            return true;
        }
        return false;
    }
}