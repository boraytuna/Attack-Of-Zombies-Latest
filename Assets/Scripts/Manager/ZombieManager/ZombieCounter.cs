using UnityEngine;
using TMPro;

public class ZombieCounter : MonoBehaviour
{
    public static ZombieCounter Instance { get; private set; }
    [SerializeField] private int zombieCount = 1;
    public TextMeshProUGUI zombieCountText; // Reference to the UI Text component

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UpdateZombieCountText(); // Update the UI text initially
    }

    public int GetZombieCount()
    {
        return zombieCount;
    }

    public void IncrementZombieCount()
    {
        zombieCount++;
        UpdateZombieCountText(); // Update UI text when count changes
    }

    public void DecrementZombieCount()
    {
        if (zombieCount > 0)
        {
            zombieCount--;
            UpdateZombieCountText(); // Update UI text when count changes
        }
        else
        {
            Debug.LogWarning("Zombie Count is already at zero or negative");
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        zombieCount += pointsToAdd;
        UpdateZombieCountText(); // Update UI text when points are added
    }

    void UpdateZombieCountText()
    {
        if (zombieCountText != null)
        {
            zombieCountText.text = ": " + zombieCount.ToString();
        }
        else
        {
            Debug.LogWarning("UI Text component for zombie count is not assigned.");
        }
    }
}
