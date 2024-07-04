using System.Collections.Generic;
using UnityEngine;

public class ZombieDetectionForAttackers : MonoBehaviour
{
    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask zombieLayer;
    private IAttacker attacker;

    void Start()
    {
        attacker = GetComponent<IAttacker>();
    }

    void Update()
    {
        DetectZombiesInRange();
    }

    void DetectZombiesInRange()
    {
        // Use Physics.OverlapSphere to detect all colliders within the detection range
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, zombieLayer);

        // Iterate through all detected colliders
        foreach (Collider collider in colliders)
        {
            transform.LookAt(collider.transform);
            Debug.Log("Zombie detected: " + collider.gameObject.name);

            // Attack the detected zombie
            attacker.Attack(collider);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
