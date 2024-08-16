// using UnityEngine;
// using UnityEngine.AI;

// public class EscapePointCalculator : MonoBehaviour, IHuman
// {
//     [SerializeField] private float escapeDistanceMultiplier = 10f; // Multiplier for calculating escape point distance from zombie
//     [SerializeField] private float maxSampleDistance = 10f; // Maximum distance to sample for NavMesh
//     [SerializeField] private float boundaryCheckDistance = 10f; // Distance for boundary checking with raycast
//     [SerializeField] private LayerMask boundaryLayerMask; // Layer mask for detecting boundaries

//     public Vector3 zombiePosition;
//     private HumanDetector humanDetector;

//     private void Start()
//     {
//         humanDetector = GetComponent<HumanDetector>();
//     }

//     // Public method to set the zombie position
//     public void SetZombiePosition(Vector3 position)
//     {
//         zombiePosition = position;
//     }

//     public void HandleDetection()
//     {
//         CalculateEscapePoint(zombiePosition);
//         humanDetector.MoveHumans();
//     }

//     private void CalculateEscapePoint(Vector3 zombiePosition)
//     {
//         if(zombiePosition != null)
//         {
//             Vector3 directionToZombie = transform.position - zombiePosition; // Calculate direction from escape point to zombie
//             Vector3 rawEscapePoint = transform.position + directionToZombie.normalized * escapeDistanceMultiplier; // Calculate raw escape point

//             // Check if the escape point is on the NavMesh
//             if (NavMesh.SamplePosition(rawEscapePoint, out NavMeshHit hit, maxSampleDistance, NavMesh.AllAreas) && !IsOutsideBoundary(hit.position))
//             {
//                 HumanMovement.SetEscapePoint(hit.position); // Set escape point for humans
//             }
//             else
//             {
//                 // If the initial point is not valid, adjust it
//                 Vector3 validEscapePoint = FindValidEscapePoint(directionToZombie);
//                 HumanMovement.SetEscapePoint(validEscapePoint); // Set adjusted escape point for humans
//             }
//         }else
//         {
//             Debug.LogError("Zombie Position is null");
//         }

//     }

//     private Vector3 FindValidEscapePoint(Vector3 directionToZombie)
//     {
//         Vector3 basePoint = transform.position;
//         for (float multiplier = escapeDistanceMultiplier; multiplier > 0; multiplier -= 1f)
//         {
//             Vector3 candidatePoint = basePoint + directionToZombie.normalized * multiplier;
//             if (NavMesh.SamplePosition(candidatePoint, out NavMeshHit hit, maxSampleDistance, NavMesh.AllAreas) && !IsOutsideBoundary(hit.position))
//             {
//                 return hit.position; // Return valid escape point found on NavMesh
//             }
//         }
//         // Return current position if no valid point is found
//         return basePoint;
//     }

//     private bool IsOutsideBoundary(Vector3 position)
//     {
//         // Cast a ray in the direction of the calculated escape point
//         if (Physics.Raycast(transform.position, position - transform.position, out RaycastHit hit, boundaryCheckDistance, boundaryLayerMask))
//         {
//             // If the ray hits something within the boundary layer, it means the point is outside the boundary
//             return true;
//         }
//         return false;
//     }
// }
using UnityEngine;
using UnityEngine.AI;

public class EscapePointCalculator : MonoBehaviour, IHuman
{
    [SerializeField] private float escapeDistanceMultiplier = 10f; // Multiplier for calculating escape point distance from zombie
    [SerializeField] private float maxSampleDistance = 10f; // Maximum distance to sample for NavMesh
    [SerializeField] private float boundaryCheckDistance = 10f; // Distance for boundary checking with raycast
    [SerializeField] private LayerMask boundaryLayerMask; // Layer mask for detecting boundaries
    [SerializeField] private Transform zombieTransform;

    private HumanDetector humanDetector;

    private void Start()
    {
        humanDetector = GetComponent<HumanDetector>();
        zombieTransform = GameObject.FindWithTag("Player").transform;
    }

    public void HandleDetection()
    {
        if (zombieTransform != null)
        {
            CalculateEscapePoint(zombieTransform.position);  
            humanDetector.MoveHumans(); 
        }
        else
        {
            Debug.LogError("Zombie Transform is not set.");
        }
    }

    public void CalculateEscapePoint(Vector3 zombiePosition)
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
