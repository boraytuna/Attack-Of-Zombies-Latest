// using UnityEngine;

// public class HumanManager : MonoBehaviour
// {
//     public static HumanManager Instance { get; private set; }
//     [SerializeField] private HumanSpawner humanSpawner;
//     [SerializeField] private MixedAttackerSpawner mixedAttackerSpawner;
//     [SerializeField] private HumanSpeedManager humanSpeedManager;
//     [SerializeField] private AttackerDamageManager attackerDamageManager; // Add reference here

//     // private void Awake()
//     // {
//     //     // Singleton pattern to ensure only one instance of HumanManager
//     //     if (Instance == null)
//     //     {
//     //         Instance = this;
//     //         DontDestroyOnLoad(gameObject);
//     //     }
//     //     else
//     //     {
//     //         Destroy(gameObject);
//     //     }
//     // }
    
//     public void Initialize()
//     {
//         if (humanSpawner != null)
//         {
//             humanSpawner.Spawn();
//         }
//         else
//         {
//             Debug.LogError("Human spawner is null");
//         }

//         if (mixedAttackerSpawner != null)
//         {
//             mixedAttackerSpawner.Spawn();
//         }
//         else
//         {
//             Debug.LogError("Mixed attacker spawner is null");
//         }

//         if (humanSpeedManager != null)
//         {   
//             humanSpeedManager.GetCurrentSpeed();
//         }
//         else
//         {
//             Debug.LogError("Human speed manager is null");
//         }

//         if (attackerDamageManager != null)
//         {
//             attackerDamageManager.GetMultiplier(); 
//         }
//         else
//         {
//             Debug.LogError("Attacker damage manager is null");
//         }
//     }
// }
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    public static HumanManager Instance { get; private set; }
    [SerializeField] private HumanSpawner humanSpawner;
    [SerializeField] private MixedAttackerSpawner mixedAttackerSpawner;
    [SerializeField] private HumanSpeedManager humanSpeedManager;
    [SerializeField] private AttackerDamageManager attackerDamageManager; // Add reference here

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of HumanManager
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
        if (humanSpawner == null)
        {
            humanSpawner = FindObjectOfType<HumanSpawner>();
        }

        if (mixedAttackerSpawner == null)
        {
            mixedAttackerSpawner = FindObjectOfType<MixedAttackerSpawner>();
        }

        if (humanSpeedManager == null)
        {
            humanSpeedManager = FindObjectOfType<HumanSpeedManager>();
        }

        if (attackerDamageManager == null)
        {
            attackerDamageManager = FindObjectOfType<AttackerDamageManager>();
        }

        if (humanSpawner != null)
        {
            humanSpawner.FindPlayerAndNavMesh();
            humanSpawner.Spawn();
        }
        else
        {
            Debug.LogError("Human spawner is null");
        }

        if (mixedAttackerSpawner != null)
        {
            mixedAttackerSpawner.FindPlayerAndNavMesh();
            mixedAttackerSpawner.Spawn();
        }
        else
        {
            Debug.LogError("Mixed attacker spawner is null");
        }

        if (humanSpeedManager != null)
        {
            humanSpeedManager.GetCurrentSpeed();
        }
        else
        {
            Debug.LogError("Human speed manager is null");
        }

        if (attackerDamageManager != null)
        {
            attackerDamageManager.GetMultiplier();
        }
        else
        {
            Debug.LogError("Attacker damage manager is null");
        }
    }
}
