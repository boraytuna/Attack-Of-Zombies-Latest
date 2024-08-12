// using UnityEngine;

// public class PlayerHealth : Health, IDamagable
// {
//     void Start()
//     {
//         currentHealth = maxHealth;  // Initialize current health to max health
//     }

//     protected override void Die()
//     {
//         Debug.Log("Player died!"); // Example death behavior for player

//         // Play the zombie death sound
//         FindObjectOfType<AudioManager>().Play("ZombieDeath");

//         // Notify the GameManager that the player has died
//         UIManager.Instance.OnPlayerDeath();

//         // Trigger the player death event
//         GamePlayEvents.TriggerPlayerDeath();

//         Destroy(gameObject);
//     }

// }
using System;
using UnityEngine;

public class PlayerHealth : Health, IDamagable
{
    public bool isAlive { get; private set; } = true; // Add a flag to indicate if the player is alive
    public static Action OnPlayerDeath { get; internal set; }

    private AudioManager audioManager;

    void Start()
    {
        currentHealth = maxHealth;  // Initialize current health to max health
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    protected override void Die()
    {
        Debug.Log("Player died!"); // Example death behavior for player

        // Play the zombie death sound
        audioManager.Play("RoundLose");
        
        // Notify the GameManager that the player has died
        UIManager.Instance.OnPlayerDeath();
        
        // Trigger the player death event
        GamePlayEvents.TriggerPlayerDeath();

        isAlive = false; // Set the flag to false when the player dies
        Destroy(gameObject);
    }
}
