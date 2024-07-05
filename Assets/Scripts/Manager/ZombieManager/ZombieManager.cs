using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }
    [SerializeField] private ZombieList zombieList; // Manages the zombies
    [SerializeField] private ZombieCounter zombieCounter; // Manages the zombie count
    [SerializeField] private ZombieSpeedManager zombieSpeedManager; // Manages central speed for zombies

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
        // Attempt to find components if they are not assigned in the Inspector
        if (zombieCounter == null)
        {
            zombieCounter = GetComponent<ZombieCounter>();
        }

        if (zombieList == null)
        {
            zombieList = GetComponent<ZombieList>();
        }

        if (zombieSpeedManager == null)
        {
            zombieSpeedManager = FindObjectOfType<ZombieSpeedManager>();
        }

        // Initialize components if they are found
        if (zombieCounter != null)
        {
            zombieCounter.GetZombieCount();
        }
        // else
        // {
        //     Debug.LogError("Zombie Counter is still null.");
        // }

        if (zombieList != null)
        {
            zombieList.Initialize();
        }
        // else
        // {
        //     Debug.LogError("Zombie List is null.");
        // }

        if (zombieSpeedManager != null)
        {
            zombieSpeedManager.Initialize();
        }
        // else
        // {
        //     Debug.LogError("Zombie Speed Manager is null");
        // }
    }

    public int GetZombieCount()
    {
        return zombieCounter != null ? zombieCounter.GetZombieCount() : 0;
    }
}
