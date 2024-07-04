using System.Collections.Generic;
using UnityEngine;

// This script detects humans around the main human in order to move together.
public class HumanDetector : MonoBehaviour
{
    public float detectionRange = 10f; // Range for detecting humans
    public List<Transform> detectedHumans = new List<Transform>(); // List to store detected humans

    private bool isZombieInRange = false; // Flag to indicate if zombie is in range
    private Transform centralHuman; // Reference to the main human associated with this group

    void OnEnable()
    {
        ZombieDetection.OnZombieDetected += HandleZombieDetected; // Subscribe to zombie detection event
    }

    void OnDisable()
    {
        ZombieDetection.OnZombieDetected -= HandleZombieDetected; // Unsubscribe from zombie detection event
    }

    // Set the main human associated with this group
    public void SetMainHuman(Transform centralHuman)
    {
        this.centralHuman = centralHuman;
    }

    void HandleZombieDetected(Vector3 zombiePosition)
    {
        // Check if the zombie is within range of the main human of this group
        if (Vector3.Distance(transform.position, zombiePosition) < detectionRange)
        {
            isZombieInRange = true; // Set flag to indicate zombie is in range
        }
    }

    void Update()
    {
        detectedHumans.Clear(); // Clear the list to avoid duplicates

        // Find all human objects within the detection range
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Human"))
            {
                detectedHumans.Add(collider.transform); // Add detected humans to the list
            }
        }

        // Move the detected humans to the escape point calculated by their main human
        if (isZombieInRange)
        {
            foreach (Transform human in detectedHumans)
            {
                if (human.GetComponent<HumanMovement>() != null)
                {
                    if (human.GetComponent<HumanMovement>().IsMainHuman(centralHuman))
                    {
                        human.GetComponent<HumanMovement>().MoveToEscapePoint(); // Move human to escape point
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw detection range sphere
    }
}
