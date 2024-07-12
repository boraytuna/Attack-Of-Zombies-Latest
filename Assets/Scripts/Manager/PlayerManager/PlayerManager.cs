// using UnityEngine;

// public class PlayerManager : MonoBehaviour
// {
//     public static PlayerManager Instance { get; private set; }
//     [SerializeField] private string playerHolderName = "PlayerHolder"; // Name of the empty GameObject holding the player
//     private GameObject playerHolder; // Reference to the empty GameObject holding the player
//     private GameObject player; // Reference to player object for central speed
//     [SerializeField] private PlayerMovement playerMovement; // Manages the player movement
//     [SerializeField] private PlayerHealth playerHealth; // Manages the player health

//     private Vector3 initialPosition; // Store initial position to respawn
    
//     public void Initialize()
//     {   
//         playerHolder = GameObject.Find(playerHolderName);
//         if (playerHolder != null)
//         {
//             player = GameObject.FindGameObjectWithTag("Player");
//             if (player != null)
//             {
//                 playerMovement = player.GetComponent<PlayerMovement>();
//                 playerHealth = player.GetComponent<PlayerHealth>();

//                 if (playerMovement == null)
//                 {
//                     Debug.LogError("PlayerMovement component not found on player.");
//                 }
//                 if (playerHealth == null)
//                 {
//                     Debug.LogError("PlayerHealth component not found on player.");
//                 }
//             }
//             else
//             {
//                 Debug.LogError("PlayerHealth component not found in playerHolder children.");
//             }
//         }
//         else
//         {
//             Debug.LogError("PlayerHolder object not found with name: " + playerHolderName);
//         }

//         if (playerHealth != null)
//         {
//             playerHealth.Initialize();
//         }
//         else
//         {
//             Debug.LogError("Player health is null");
//         }
//     }

//     Method to handle player death, notify game manager
//     public void PlayerDied()
//     {
//         GameManager.Instance.PlayerDead();
//     }
// }
