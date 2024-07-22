using System.Collections.Generic;
using UnityEngine;

public class ZombieDetectionForAttackers : MonoBehaviour
{
    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask zombieLayer;
    private IAttacker attacker;
    
    // Tag to identify the player zombie
    [SerializeField] private string playerZombieTag = "Player";

    void Start()
    {
        attacker = GetComponent<IAttacker>();
        if (attacker == null)
        {
            Debug.LogError("Attacker is null. Make sure the correct component is attached.");
        }
    }

    private void OnEnable()
    {
        GamePlayEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        GamePlayEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        this.enabled = false;
    }

    void Update()
    {
        DetectZombiesInRange();
    }

    void DetectZombiesInRange()
    {
        // Use Physics.OverlapSphere to detect all colliders within the detection range
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, zombieLayer);

        // Lists to hold other zombies and player zombie separately
        List<Collider> otherZombies = new List<Collider>();
        Collider playerZombie = null;

        // Iterate through all detected colliders
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(playerZombieTag))
            {
                playerZombie = collider; // Identify the player zombie
                //Debug.Log("Player Zombie detected: " + playerZombie.gameObject.name);
            }
            else
            {
                otherZombies.Add(collider); // Add other zombies to the list
                //Debug.Log("Other Zombie detected: " + collider.gameObject.name);
            }
        }

        // Attack other zombies first
        foreach (Collider collider in otherZombies)
        {
            if (attacker != null)
            {
                transform.LookAt(collider.transform);
                //Debug.Log("Attacking other zombie: " + collider.gameObject.name);
                attacker.Attack(collider);
            }
            else
            {
                Debug.LogError("Attacker is null. Cannot attack.");
            }
        }

        // Attack the player zombie last
        if (playerZombie != null)
        {
            if (attacker != null)
            {
                transform.LookAt(playerZombie.transform);
                //Debug.Log("Attacking player zombie: " + playerZombie.gameObject.name);
                attacker.Attack(playerZombie);
            }
            else
            {
                Debug.LogError("Attacker is null. Cannot attack player zombie.");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
