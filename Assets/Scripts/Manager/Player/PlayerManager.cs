using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [SerializeField] private PlayerMovement playerMovement; // Manages the player movement
    [SerializeField] private PlayerHealth playerHealth; // Manages the player health
    [SerializeField] private PlayerInputManager playerInputManager; // Manages the input for gameplay zombies movement

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Initialize()
    {
        if (playerMovement != null)
        {
            playerMovement.Initialize();
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
}
