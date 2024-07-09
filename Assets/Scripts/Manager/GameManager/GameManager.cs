// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance { get; private set; }

//     [SerializeField] private PlayerManager playerManager; // Reference to PlayerManager
//     [SerializeField] private HumanManager humanManager; // Reference to HumanManager
//     [SerializeField] private ZombieManager zombieManager; // Reference to ZombieManager 
//     [SerializeField] private UIManager uIManager; // Reference to UIManager
    
//     private void Awake()
//     {
//         // Singleton pattern to ensure only one instance of GameManager
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         // Initialize systems
//         InitializeSystems();
//     }

//     // Method to initialize various systems
//     public void InitializeSystems()
//     {
//         if (humanManager != null)
//         {
//             humanManager.Initialize();
//         }
//         else
//         {
//             Debug.LogWarning("Human Manager is null");
//         }

//         if (playerManager != null)
//         {
//             playerManager.Initialize();
//         }
//         else
//         {
//             Debug.LogWarning("Player Manager is null");
//         }

//         if (zombieManager != null)
//         {
//             zombieManager.Initialize();
//         }
//         else
//         {
//             Debug.LogWarning("Zombie Manager is null");
//         }

//         if (uIManager != null)
//         {
//             uIManager.LoadCanvas();
//         }
//         else
//         {
//             Debug.LogWarning("UIManager is null");
//         }
//     }

//     // Method to pause all game actions
//     public void PauseAllGameActions()
//     {
//         GamePauser.Instance.PauseGame();
//         uIManager.ActivatePausePanel();
//     }

//     // Method to resume all game actions
//     public void ResumeAllGameActions()
//     {
//         GamePauser.Instance.ResumeGame();
//         uIManager.DeactivatePausePanel();
//     }

//     // Method to handle player death
//     public void PlayerDead()
//     {
//         GamePauser.Instance.PauseGame();
//         uIManager.OnPlayerDeath();
//     }

//     // Method to restart the current level
//     public void RestartLevel()
//     {
//         SceneReloader.Instance.ReloadCurrentScene();
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerManager playerManager; // Reference to PlayerManager
    [SerializeField] private HumanManager humanManager; // Reference to HumanManager
    [SerializeField] private ZombieManager zombieManager; // Reference to ZombieManager 
    [SerializeField] private UIManager uIManager; // Reference to UIManager

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

    // Method to initialize various systems
    public void InitializeSystems()
    {
        if (humanManager != null)
        {
            humanManager.Initialize();
        }
        else
        {
            Debug.LogWarning("Human Manager is null");
        }

        // if (playerManager != null)
        // {
        //     playerManager.Initialize();
        // }
        // else
        // {
        //     Debug.LogWarning("Player Manager is null");
        // }

        if (zombieManager != null)
        {
            zombieManager.Initialize();
        }
        else
        {
            Debug.LogWarning("Zombie Manager is null");
        }

        if (uIManager != null)
        {
            uIManager.LoadCanvas();
        }
        else
        {
            Debug.LogWarning("UIManager is null");
        }
    }

    // Method to pause all game actions
    public void PauseAllGameActions()
    {
        GamePauser.Instance.PauseGame();
        uIManager.ActivatePausePanel();
    }

    // Method to resume all game actions
    public void ResumeAllGameActions()
    {
        GamePauser.Instance.ResumeGame();
        uIManager.DeactivatePausePanel();
    }

    // Method to handle player death
    public void PlayerDead()
    {
        GamePauser.Instance.PauseGame();
        uIManager.OnPlayerDeath();
    }

    // Method to restart the current level
    public void RestartLevel()
    {
        SceneReloader.Instance.ReloadCurrentScene();
        uIManager.DeactivateDeathPanel();   
    }
}
