using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script detects humans around the main human in order to move together.
public class HumanDetector : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f; // Range for detecting humans
    [SerializeField] private List<Transform> detectedHumans = new List<Transform>(); // List to store detected humans
    [SerializeField] private LayerMask humanLayer; // Layer mask for detecting boundaries
    [SerializeField] private Collider[] hitColliders = new Collider[20]; // Array to store detected colliders
    [SerializeField] private Transform zombieTransform;

    void Start()
    {
        zombieTransform = GameObject.FindWithTag("Player").transform;

        StartCoroutine(DelayedHumanDetection(1f));
    }

    private IEnumerator DelayedHumanDetection(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        DetectHumans(); // Call the detection method after the delay
    }

    void DetectHumans()
    {
        detectedHumans.Clear();

        // Use a HashSet to track unique humans
        HashSet<Transform> uniqueHumans = new HashSet<Transform>();

        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRange, hitColliders, humanLayer);
        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = hitColliders[i];
            if (collider != null)
            {
                Transform HumanTransform = collider.transform;

                if (uniqueHumans.Add(HumanTransform)) // Adds to HashSet if not already present
                {
                    detectedHumans.Add(HumanTransform); // Add to list
                }
            }
        }

        detectedHumans.Add(this.transform);
    }

    public void MoveHumans()
    {
        foreach (Transform human in detectedHumans)
        {
            if (human.GetComponent<HumanMovement>() != null)
            {
                human.GetComponent<HumanMovement>().MoveToEscapePoint(); // Move human to escape point
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw detection range sphere
    }
}
