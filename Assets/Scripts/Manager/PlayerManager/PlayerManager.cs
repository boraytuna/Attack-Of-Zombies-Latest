using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [SerializeField] private PlayerMovement playerMovement; // Manages the player movement
    [SerializeField] private PlayerHealth playerHealth; // Manages the player health

    private Vector3 initialPosition; // Store initial position to respawn

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of PlayerManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            initialPosition = transform.position; // Store the initial position
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Initialize player components
    public void Initialize()
    {
        if (playerMovement != null)
        {
            playerMovement.GetRigidBody();
        }
        else
        {
            Debug.LogError("PlayerMovement component not found on the player GameObject.");
        }

        if (playerHealth != null)
        {
            playerHealth.Initialize();
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on the player GameObject.");
        }
    }

    // Method to handle player death, notify game manager
    public void PlayerDied()
    {
        GameManager.Instance.PlayerDead();
    }
}
