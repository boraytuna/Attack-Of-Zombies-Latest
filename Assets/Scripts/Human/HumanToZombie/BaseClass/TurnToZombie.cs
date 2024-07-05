using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToZombie : MonoBehaviour
{
    public LayerMask groundLayer; // Layer mask to define what layers are considered ground
    public string zombiePrefabPath = "ZombiePrefabs/GamePlayZombieResourcesTest"; // Path to the zombie prefab in the Resources folder
    private GameObject zombiePrefab; // Reference to the loaded zombie prefab
    protected ZombieList zombieList; // Reference to zombie manager script
    protected ZombieCounter zombieCounter; // Reference to the Zombie Counter script
    private ZombieAnimatorController zombieAnimatorController;

    protected virtual void Start()
    {
        zombiePrefab = Resources.Load<GameObject>(zombiePrefabPath);
        if (zombiePrefab == null)
        {
            Debug.LogError("Zombie prefab not found at path: " + zombiePrefabPath);
            return;
        }

        zombieCounter = FindObjectOfType<ZombieCounter>();
        if (zombieCounter == null)
        {
            Debug.LogError("ZombieCounter not found in the scene.");
        }

        zombieList = FindObjectOfType<ZombieList>();
        if (zombieList == null)
        {
            Debug.LogError("ZombieManager not found in the scene.");
        }
    }

    protected void HandleOnTriggerEnter(Collider other, bool isCentralEntity, System.Action onCentralEntityKilled)
    {
        if (other.gameObject.CompareTag("Zombie") || other.gameObject.CompareTag("Player"))
        {
            if (zombiePrefab != null)
            {
                zombieAnimatorController = other.gameObject.GetComponent<ZombieAnimatorController>();
                if (zombieAnimatorController != null)
                {
                    // Play the attack animation
                    zombieAnimatorController.PlayAttack();
                }
                else
                {
                    Debug.LogError("ZombieAnimatorController not found on the triggering zombie.");
                }

                // Play the human death sound
                FindObjectOfType<AudioManager>().Play("HumanDeath");

                // Find the ground position
                Vector3 spawnPosition = FindGroundPosition(transform.position);

                // Instantiate a new zombie at the correct ground position
                GameObject newZombieObject = Instantiate(zombiePrefab, spawnPosition, transform.rotation);

                // Add the zombie to the ZombieManager
                zombieList.AddZombie(newZombieObject);

                // Increment the number of total zombies
                zombieCounter.IncrementZombieCount();

                // Notify the spawner if this is a central entity
                if (isCentralEntity)
                {
                    onCentralEntityKilled.Invoke();
                }

                // Destroy the current game object (human or attacker)
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Zombie prefab is not assigned. Assign the zombie prefab in the inspector.");
            }
        }
    }

    public Vector3 FindGroundPosition(Vector3 originalPosition)
    {
        Vector3 rayStart = originalPosition + Vector3.up * 10f; // Start the raycast from above the object
        Ray ray = new Ray(rayStart, Vector3.down); // Cast the ray downwards

        if (Physics.Raycast(ray, out RaycastHit hit, 20f, groundLayer))
        {
            return hit.point; // Return the point where the ray hit the ground
        }

        return originalPosition; // If no ground was found, return the original position
    }
}
