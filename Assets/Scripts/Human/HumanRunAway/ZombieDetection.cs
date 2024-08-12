// using UnityEngine;
// using System;

// // This script is attached to the main human object 
// // and it checks if zombie objects are in range.
// public class ZombieDetection : MonoBehaviour
// {
//     [SerializeField] private float detectionRange; // Range for detecting zombies
//     [SerializeField] private LayerMask zombieLayer; // Layer mask for detecting zombies

//     public static event Action<Vector3> OnZombieDetected; // Event triggered when a zombie is detected

//     void Update()
//     {
//         // Find all colliders within the detection range that are on the zombie layer
//         Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, zombieLayer);
//         foreach (Collider collider in colliders)
//         {
//             Transform zombieTransform = collider.transform;
//             if (zombieTransform != null)
//             {
//                 //Debug.Log("Zombie detected"); // Log detection
//                 OnZombieDetected?.Invoke(zombieTransform.position); // Trigger zombie detection event
//                 break; // Break after detecting the first zombie in range
//             }
//         }
//     }
// }
using System;
using UnityEngine;

public class ZombieDetection : MonoBehaviour
{
    [SerializeField] private float detectionRange; // Range for detecting zombies
    [SerializeField] private LayerMask zombieLayer; // Layer mask for detecting zombies
    private CentralHumanNotifier notifier;

    void OnEnable()
    {
        notifier = GameObject.FindWithTag("Player").GetComponent<CentralHumanNotifier>();
        if (notifier != null)
        {
            notifier.Subscribe(this); // Subscribe to the notifier
        }
    }

    void OnDisable()
    {
        if (notifier != null)
        {
            notifier.Unsubscribe(this); // Unsubscribe from the notifier
        }
    }

    // Method called by the notifier when a zombie is in range
    public void OnZombieInRange(Vector3 zombiePosition)
    {
        // Check if the zombie is within the detection range of this human
        if (Vector3.Distance(transform.position, zombiePosition) <= detectionRange)
        {
            // Trigger the event for zombie detection
            OnZombieDetected?.Invoke(zombiePosition);
        }
    }

    public static event Action<Vector3> OnZombieDetected; // Event triggered when a zombie is detected
}
