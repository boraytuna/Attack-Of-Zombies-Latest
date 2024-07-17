using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanSpeedManager : MonoBehaviour
{
    public static HumanSpeedManager Instance { get; private set; } // Singleton instance

    private List<NavMeshAgent> humanAgents = new List<NavMeshAgent>(); // List for all the agents

    private bool overrideSpeed = false; // Flag to override current speed

    [SerializeField] private float currentSpeed;  // Local variable for speed
    private float originalSpeed; // Original speed before any overrides

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        GetCurrentSpeed();
    }

    public void GetCurrentSpeed()
    {
        if (overrideSpeed)
        {
            return; // Return if speed override is active
        }

        if (ZombieSpeedManager.Instance != null)
        {
            if (ZombieSpeedManager.Instance.IsSpeedBoostActive)
            {
                // Keep current speed during the boost period
                return;
            }

            // Calculate the target speed based on the current zombie speed
            currentSpeed = ZombieSpeedManager.Instance.GetCurrentSpeed() * 0.8f;
        }
        else
        {
            Debug.LogError("ZombieSpeedManager in HumanSpeedManager is null");
            return;
        }

        // Update the speed of all human agents
        foreach (NavMeshAgent agent in humanAgents)
        {
            agent.speed = currentSpeed;
        }
    }
}
