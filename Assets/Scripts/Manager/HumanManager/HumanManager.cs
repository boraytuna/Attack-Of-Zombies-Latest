using UnityEngine;
using System.Collections.Generic;

public class HumanManager : MonoBehaviour
{
    public static HumanManager Instance { get; private set; }
    [SerializeField] private HumanSpawner humanSpawner;
    [SerializeField] private MixedAttackerSpawner mixedAttackerSpawner;
    [SerializeField] private HumanSpeedManager humanSpeedManager;

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
        if (humanSpawner != null)
        {
            humanSpawner.Initialize();
        }
        else
        {
            Debug.LogError("Human spawner is null");
        }

        if (mixedAttackerSpawner != null)
        {
            mixedAttackerSpawner.Initialize();
        }
        else
        {
            Debug.LogError("Mixed attacker spawner is null");
        }

        if (humanSpeedManager != null)
        {   
            humanSpeedManager.Initialize();
        }
        else
        {
            Debug.LogError("Human speed manager is null");
        }
    }
}
