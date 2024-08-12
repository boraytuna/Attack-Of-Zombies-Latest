using UnityEngine;
using System.Collections.Generic;

public class CentralHumanNotifier : MonoBehaviour
{
    [SerializeField] private float detectionRange; // Range for detecting zombies
    [SerializeField] private LayerMask humanLayer; // Layer mask for detecting zombies
    private Collider[] collidersBuffer = new Collider[50]; // Buffer to store detected colliders, adjust size as needed
    private List<ZombieDetection> subscribedDetectors = new List<ZombieDetection>(); // List of subscribed ZombieDetection instances

    void Update()
    {
        // Detect humans within range using OverlapSphereNonAlloc
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRange, collidersBuffer, humanLayer);
        for (int i = 0; i < colliderCount; i++)
        {
            Collider collider = collidersBuffer[i];
            Transform zombieTransform = collider.transform;
            if (zombieTransform != null)
            {
                // Notify all subscribed ZombieDetection instances
                NotifyZombieDetectors(zombieTransform.position);
                break; // Stop after detecting the first zombie in range
            }
        }
    }

    // Method for ZombieDetection instances to subscribe to notifications
    public void Subscribe(ZombieDetection detector)
    {
        if (!subscribedDetectors.Contains(detector))
        {
            subscribedDetectors.Add(detector);
        }
    }

    // Method for ZombieDetection instances to unsubscribe from notifications
    public void Unsubscribe(ZombieDetection detector)
    {
        if (subscribedDetectors.Contains(detector))
        {
            subscribedDetectors.Remove(detector);
        }
    }

    // Notify all subscribed ZombieDetection instances
    private void NotifyZombieDetectors(Vector3 zombiePosition)
    {
        foreach (ZombieDetection detector in subscribedDetectors)
        {
            detector.OnZombieInRange(zombiePosition);
        }
    }
}
