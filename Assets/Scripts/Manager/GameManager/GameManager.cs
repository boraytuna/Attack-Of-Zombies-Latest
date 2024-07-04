using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private HumanManager humanManager;
    [SerializeField] private ZombieManager zombieManager;

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

    private void Start()
    {
        // Initialize systems
        InitializeSystems();
    }

    private void InitializeSystems()
    {
        if (humanManager != null)
        {
            humanManager.Initialize();
        }
        else
        {
            Debug.LogWarning("Human Manager is null");
        }

        if (playerManager != null)
        {
            playerManager.Initialize();
        }
        else
        {
            Debug.LogWarning("Player Manager is null");
        }

        if (zombieManager != null)
        {
            zombieManager.Initialize();
        }
        else
        {
            Debug.LogWarning("Zombie Manager is null");
        }
    }
}
