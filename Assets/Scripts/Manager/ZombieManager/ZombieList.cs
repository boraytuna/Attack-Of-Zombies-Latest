using System.Collections.Generic;
using UnityEngine;

public class ZombieList : MonoBehaviour
{
    private Dictionary<GameObject, int> zombieIds = new Dictionary<GameObject, int>(); // Dictionary to store zombies with their IDs
    private int nextId = 1; // Counter for assigning unique IDs

    // Method to initialize the zombie list
    void Start()
    {
        zombieIds.Clear(); // Clear existing zombie list
        nextId = 1; // Reset the ID counter
    }

    // Method to add a zombie to the list
    public void AddZombie(GameObject zombie)
    {
        if (!zombieIds.ContainsKey(zombie))
        {
            zombieIds.Add(zombie, nextId);
            nextId++;
        }
    }

    // Method to remove a zombie from the list
    public void RemoveZombie(GameObject zombie)
    {
        if (zombieIds.ContainsKey(zombie))
        {
            zombieIds.Remove(zombie);
        }
    }


}
