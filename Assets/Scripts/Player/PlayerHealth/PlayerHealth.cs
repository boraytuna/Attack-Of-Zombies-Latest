using UnityEngine;

public class PlayerHealth : Health, IDamagable
{
    void Start()
    {
        currentHealth = maxHealth;  // Initialize current health to max health
    }

    protected override void Die()
    {
        Debug.Log("Player died!"); // Example death behavior for player

        // Play the zombie death sound
        FindObjectOfType<AudioManager>().Play("ZombieDeath");
        
        // Notify the GameManager that the player has died
        UIManager.Instance.OnPlayerDeath();
        
        // Trigger the player death event
        GamePlayEvents.TriggerPlayerDeath();

        Destroy(gameObject);
    }

}
