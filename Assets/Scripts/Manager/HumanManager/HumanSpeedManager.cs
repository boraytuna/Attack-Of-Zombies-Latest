using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanSpeedManager : MonoBehaviour
{
    private List<NavMeshAgent> humanAgents = new List<NavMeshAgent>(); // List for all the agents

    public float currentSpeed;  // Local variable for speed

    public void Initialize()
    {
        Update();
    }

    void Update()
    {
        if(ZombieSpeedManager.Instance != null)
        {
            // Calculate the target speed based on the current zombie speed
            currentSpeed = ZombieSpeedManager.Instance.GetCurrentSpeed() * 0.8f;
        }
        else
        {
            Debug.LogError("ZombieSpeedManager in human manager is null");
        }

        // Update the speed of all human agents
        foreach (NavMeshAgent agent in humanAgents)
        {
            agent.speed = currentSpeed;
        }
    }
}
