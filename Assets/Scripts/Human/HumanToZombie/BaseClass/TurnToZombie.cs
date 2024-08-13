// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TurnToZombie : MonoBehaviour
// {
//     public LayerMask groundLayer; // Layer mask to define what layers are considered ground
//     public string zombiePrefabPath = "ZombiePrefabs/GamePlayZombieResourcesTest"; // Path to the zombie prefab in the Resources folder
//     private GameObject zombiePrefab; // Reference to the loaded zombie prefab
//     protected ZombieCounter zombieCounter; // Reference to the Zombie Counter script
//     protected CollectibleDropper collectibleDropper;
//     private RaycastHit[] raycastHits = new RaycastHit[1];  // Buffer for RaycastHit to use with non-allocating raycasting

//     protected virtual void Start()
//     {   
//         zombiePrefab = Resources.Load<GameObject>(zombiePrefabPath);
//         if (zombiePrefab == null)
//         {
//             Debug.LogError("Zombie prefab not found at path: " + zombiePrefabPath);
//             return;
//         }

//         zombieCounter = GameObject.FindWithTag("ZombieManager")?.GetComponent<ZombieCounter>();
//         if (zombieCounter == null)
//         {
//             Debug.LogError("ZombieCounter not found in the scene.");
//         }

//         collectibleDropper = GameObject.FindWithTag("HumanManager")?.GetComponent<CollectibleDropper>();
//         if (collectibleDropper == null)
//         {
//             Debug.LogError("CollectibleDropper is null");
//         }
//     }

//     protected void HandleOnTriggerEnter(Collider other, bool isCentralEntity, System.Action onCentralEntityKilled)
//     {
//         IMoveable moveable = other.gameObject.GetComponent<IMoveable>();
//         if (moveable != null)
//         {
//             if (zombiePrefab != null)
//             {
//                 // Play the human death sound
//                 FindObjectOfType<AudioManager>().Play("HumanDeath");

//                 // Find the ground position
//                 Vector3 spawnPosition = FindGroundPosition(transform.position);

//                 // Instantiate a new zombie at the correct ground position
//                 GameObject newZombieObject = Instantiate(zombiePrefab, spawnPosition, transform.rotation);

//                 // Increment the number of total zombies
//                 zombieCounter.IncrementZombieCount();

//                 // Notify the spawner if this is a central entity
//                 if (isCentralEntity)
//                 {
//                     onCentralEntityKilled.Invoke();
//                 }

//                 // Drop collectibles
//                 if (collectibleDropper != null)
//                 {
//                     collectibleDropper.TryDropCollectible(spawnPosition);
//                 }

//                 // Destroy the current game object (human or attacker)
//                 Destroy(gameObject);
//             }
//             else
//             {
//                 Debug.LogError("Zombie prefab is not assigned. Assign the zombie prefab in the inspector.");
//             }
//         }
//     }

//     public Vector3 FindGroundPosition(Vector3 originalPosition)
//     {
//         Vector3 rayStart = originalPosition + Vector3.up * 10f; // Start the raycast from above the object
//         Ray ray = new Ray(rayStart, Vector3.down); // Cast the ray downwards

//         // Perform a non-allocating raycast
//         int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 20f, groundLayer);

//         if (hitCount > 0)
//         {
//             Vector3 groundPosition = raycastHits[0].point; // Get the point where the ray hit the ground
//             if (groundPosition.y < 0.12f)
//             {
//                 groundPosition.y = 0.12f; // Adjust the y position if it's less than 0.12
//             }
//             return groundPosition; // Return the adjusted ground position
//         }

//         // If no ground was found, return the original position adjusted to at least 0.12 on the y-axis
//         if (originalPosition.y < 0.12f)
//         {
//             originalPosition.y = 0.12f;
//         }
//         return originalPosition;
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToZombie : MonoBehaviour
{
    public LayerMask groundLayer; 
    public string zombieTag = "Zombie"; // Tag used to identify the zombie pool
    protected ZombieCounter zombieCounter; 
    protected CollectibleDropper collectibleDropper;
    private RaycastHit[] raycastHits = new RaycastHit[1];  

    protected virtual void Start()
    {   
        zombieCounter = GameObject.FindWithTag("ZombieManager")?.GetComponent<ZombieCounter>();
        if (zombieCounter == null)
        {
            Debug.LogError("ZombieCounter not found in the scene.");
        }

        collectibleDropper = GameObject.FindWithTag("HumanManager")?.GetComponent<CollectibleDropper>();
        if (collectibleDropper == null)
        {
            Debug.LogError("CollectibleDropper is null");
        }
    }

    protected void HandleOnTriggerEnter(Collider other, bool isCentralEntity, System.Action onCentralEntityKilled)
    {
        IMoveable moveable = other.gameObject.GetComponent<IMoveable>();
        if (moveable != null)
        {
            // Play the human death sound
            FindObjectOfType<AudioManager>().Play("HumanDeath");

            // Find the ground position
            Vector3 spawnPosition = FindGroundPosition(transform.position);

            // Spawn a new zombie from the pool
            GameObject newZombieObject = ObjectPooler.Instance.SpawnFromPool(zombieTag, spawnPosition, transform.rotation);
            if (newZombieObject != null)
            {
                // Increment the number of total zombies
                zombieCounter.IncrementZombieCount();

                // Notify the spawner if this is a central entity
                if (isCentralEntity)
                {
                    onCentralEntityKilled.Invoke();
                }

                // Drop collectibles
                if (collectibleDropper != null)
                {
                    collectibleDropper.TryDropCollectible(spawnPosition);
                }

                // Deactivate the current game object (human or attacker)
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Failed to spawn zombie from pool.");
            }
        }
    }

    public Vector3 FindGroundPosition(Vector3 originalPosition)
    {
        Vector3 rayStart = originalPosition + Vector3.up * 12f; 
        Ray ray = new Ray(rayStart, Vector3.down); 
        float minimumOff = 0.2f;

        int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 20f, groundLayer);

        if (hitCount > 0)
        {
            Vector3 groundPosition = raycastHits[0].point; 
            if (groundPosition.y < minimumOff)
            {
                groundPosition.y = minimumOff; 
            }
            return groundPosition; 
        }

        if (originalPosition.y < minimumOff)
        {
            originalPosition.y = minimumOff;
        }
        return originalPosition;
    }
}
