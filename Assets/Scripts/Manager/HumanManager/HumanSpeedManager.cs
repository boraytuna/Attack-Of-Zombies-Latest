using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanSpeedManager : MonoBehaviour
{
    public static HumanSpeedManager Instance { get; private set; } // Singleton instance

    private List<NavMeshAgent> humanAgents = new List<NavMeshAgent>(); // List for all the agents

    public float currentSpeed;  // Local variable for speed

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        GetCurrentSpeed();
    }

    public void GetCurrentSpeed()
    {
        if (ZombieSpeedManager.Instance != null)
        {
            // Calculate the target speed based on the current zombie speed
            currentSpeed = ZombieSpeedManager.Instance.GetCurrentSpeed() * 0.8f;
        }
        else
        {
            Debug.LogError("ZombieSpeedManager in HumanSpeedManager is null");
        }

        // Update the speed of all human agents
        foreach (NavMeshAgent agent in humanAgents)
        {
            agent.speed = currentSpeed;
        }
    }
}
