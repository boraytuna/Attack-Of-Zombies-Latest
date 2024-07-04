using UnityEngine;
using System;

// This script is attached to the main human object 
// and it checks if zombie objects are in range.
public class ZombieDetection : MonoBehaviour
{
    [SerializeField] private float detectionRange; // Range for detecting zombies
    [SerializeField] private LayerMask zombieLayer; // Layer mask for detecting zombies

    public static event Action<Vector3> OnZombieDetected; // Event triggered when a zombie is detected

    void Update()
    {
        // Find all colliders within the detection range that are on the zombie layer
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, zombieLayer);
        foreach (Collider collider in colliders)
        {
            Transform zombieTransform = collider.transform;
            if (zombieTransform != null)
            {
                //Debug.Log("Zombie detected"); // Log detection
                OnZombieDetected?.Invoke(zombieTransform.position); // Trigger zombie detection event
                break; // Break after detecting the first zombie in range
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw detection range sphere
    }
}
