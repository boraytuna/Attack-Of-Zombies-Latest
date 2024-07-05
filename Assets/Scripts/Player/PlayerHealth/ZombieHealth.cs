using System.Collections;
using UnityEngine;

public class ZombieHealth : Health, IDamagable
{
    [SerializeField] private ZombieList zombieList; // Reference to the zombie list manager
    [SerializeField] private ZombieCounter zombieCounter; // Reference to the zombie counter manager
    [SerializeField] private ZombieAnimatorController zombieAnimatorController;

    // Initialize method to set up zombie health
    public void Initialize(ZombieList list, ZombieCounter counter)
    {
        zombieList = list;
        zombieCounter = counter;
        currentHealth = maxHealth;  // Initialize current health to max health
        zombieCounter = FindObjectOfType<ZombieCounter>();
        zombieList = FindObjectOfType<ZombieList>();
    }

    protected override void Die()
    {
        Debug.Log("Zombie died!");

        // Play the zombie death sound
        FindObjectOfType<AudioManager>().Play("ZombieDeath");

        // Play Death animation if the controller is available
        if (zombieAnimatorController != null)
        {
            zombieAnimatorController.PlayDie();
        }
        else
        {
            Debug.LogError("Animator is null");
        }

        // Decrement the number of zombies if the counter is available
        if (zombieCounter != null)
        {
            zombieCounter.DecrementZombieCount();
        }
        else
        {
            Debug.LogError("Zombie counter is null");
        }

        // Remove from the zombie list if the list is available
        if (zombieList != null)
        {
            zombieList.RemoveZombie(gameObject);
        }
        else
        {
            Debug.LogError("ZombieList is null");
        }

        // Destroy the zombie GameObject
        Destroy(gameObject);
    }

}
