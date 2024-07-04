using UnityEngine;
using UnityEngine.AI; // Ensure this namespace is included

public class HumanMovement : MonoBehaviour, IMoveToEscapePoint
{
    private static Vector3 escapePoint; // Static escape point shared by all humans
    private NavMeshAgent agent; // Reference to NavMeshAgent component
    private Transform mainHuman; // Reference to the main human of the group
    [SerializeField] private HumanAnimatorController humanAnimatorController; // Reference to the human animation controller

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get reference to NavMeshAgent component
        if (humanAnimatorController == null)
        {
            humanAnimatorController = GetComponent<HumanAnimatorController>(); // Get reference to HumanAnimatorController if not set
        }
    }

    private void OnEnable()
    {
        SetEscapePoint(escapePoint); // Ensure escape point is set when enabled
    }

    void Update()
    {
        // Check if the agent is moving
        if (agent.velocity.sqrMagnitude > 0.1f) // Adjust threshold as necessary
        {
            humanAnimatorController.PlayRun();
        }
        else
        {
            humanAnimatorController.PlayIdle();
        }
    }

    public static void SetEscapePoint(Vector3 newEscapePoint)
    {
        escapePoint = newEscapePoint; // Set the shared escape point for all humans
    }

    // Check if this human is the main human of the group
    public bool IsMainHuman(Transform human)
    {
        return human == mainHuman; // Check if provided human is the main human
    }

    public void MoveToEscapePoint()
    {
        if (agent != null)
        {
            agent.SetDestination(escapePoint); // Move towards the shared escape point
        }
    }
}