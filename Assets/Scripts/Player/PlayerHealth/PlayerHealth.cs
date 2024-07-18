using UnityEngine;

public class PlayerHealth : Health, IDamagable
{
    private ZombieAnimatorController zombieAnimatorController;

    void Start()
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
        
        // Notify the GameManager that the player has died
        UIManager.Instance.OnPlayerDeath();
        
        // Trigger the player death event
        GamePlayEvents.TriggerPlayerDeath();

        Destroy(gameObject);
    }

    // Reset method to reset player health
    public void Reset()
    {
        currentHealth = maxHealth;  // Reset health to max health
        if (zombieAnimatorController != null)
        {
            zombieAnimatorController.PlayIdle();  // Reset animation to idle
        }
    }
}
