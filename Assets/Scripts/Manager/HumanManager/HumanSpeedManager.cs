// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class HumanSpeedManager : MonoBehaviour
// {
//     public static HumanSpeedManager Instance { get; private set; } // Singleton instance

//     private List<NavMeshAgent> humanAgents = new List<NavMeshAgent>(); // List for all the agents

//     public float currentSpeed;  // Local variable for speed

//     private void Update()
//     {
//         GetCurrentSpeed();
//     }

//     public void GetCurrentSpeed()
//     {
//         if (ZombieSpeedManager.Instance != null)
//         {
//             // Calculate the target speed based on the current zombie speed
//             currentSpeed = ZombieSpeedManager.Instance.GetCurrentSpeed() * 0.8f;
//         }
//         else
//         {
//             Debug.LogError("ZombieSpeedManager in HumanSpeedManager is null");
//         }

//         // Update the speed of all human agents
//         foreach (NavMeshAgent agent in humanAgents)
//         {
//             agent.speed = currentSpeed;
//         }
//     }

// }
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanSpeedManager : MonoBehaviour
{
    public static HumanSpeedManager Instance { get; private set; } // Singleton instance

    private List<NavMeshAgent> humanAgents = new List<NavMeshAgent>(); // List for all the agents

    private bool overrideSpeed = false; // Flag to override current speed

    private float currentSpeed;  // Local variable for speed
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

    public void OverrideSpeed(float newSpeed)
    {
        // Store current speed before overriding
        originalSpeed = currentSpeed;
        currentSpeed = newSpeed;
        overrideSpeed = true;

        // Update the speed of all human agents
        foreach (NavMeshAgent agent in humanAgents)
        {
            agent.speed = currentSpeed;
        }
    }

    public void ResetSpeed()
    {
        if (overrideSpeed)
        {
            currentSpeed = originalSpeed;
            overrideSpeed = false;

            // Update the speed of all human agents
            foreach (NavMeshAgent agent in humanAgents)
            {
                agent.speed = currentSpeed;
            }
        }
    }

    public bool IsSpeedOverridden()
    {
        return overrideSpeed;
    }
}
