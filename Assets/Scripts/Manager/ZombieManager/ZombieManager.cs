using System;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }
    [SerializeField] private ZombieList zombieList; // Manages the zombies
    [SerializeField] private ZombieCounter zombieCounter; // Manages the zombie count
    [SerializeField] private ZombieSpeedManager zombieSpeedManager; // Manages central speed for zombies

    public void Start()
    {
        // Attempt to find components if they are not assigned in the Inspector
        if (zombieCounter == null)
        {
            zombieCounter = GetComponent<ZombieCounter>();
        }

        // Initialize components if they are found
        if (zombieCounter != null)
        {
            zombieCounter.GetZombieCount();
        }
        else
        {
            Debug.LogError("Zombie Counter is still null.");
        }
    }

    public int GetZombieCount()
    {
        return zombieCounter != null ? zombieCounter.GetZombieCount() : 0;
    }
}
