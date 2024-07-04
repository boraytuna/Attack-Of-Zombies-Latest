using System.Collections;
using UnityEngine;

public class PlayerHealth : Health, IDamagable
{
    private ZombieAnimatorController zombieAnimatorController;

    // Initialize method to set up player health
    public void Initialize()
    {
        currentHealth = maxHealth;  // Initialize current health to max health
        zombieAnimatorController = GetComponent<ZombieAnimatorController>();
    }

    protected override void Die()
    {
        Debug.Log("Player died!"); // Example death behavior for player

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

        // Destroy the player GameObject
        Destroy(gameObject);
    }
}
