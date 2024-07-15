using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    [SerializeField] public float delayBeforeTrigger = 0.5f; // Delay for non-persistent collectibles to become collectible
    [SerializeField] public int extraPointsAdded; // Extra points added to zombie count to finish game
    [SerializeField] public float timeDurationForInvicibility; // Time duration of zombies invisibility
    [SerializeField] private float healthBoostMultiplier; // The number current health gets multiplied with 
    [SerializeField] private float healthBoostTimeDuration; // The time duration of how long the health boosted
    [SerializeField] private float speedBoostMultiplier; // The multiplier for the current speed
    [SerializeField] private float speedBootTimeDuration;  // Time duration for the speed boost
    [SerializeField] private int healthBoosterNo; // Number of health boosters player has
    [SerializeField] private int speedBoosterNo; // Number of speed boosters player has
    public TextMeshProUGUI healthBoosterText;
    public TextMeshProUGUI speedBoosterText;
    public Button healthBoosterButton;
    public Button speedBoosterButton;

    [SerializeField] private List<Collectible> collectedHealthItems;
    [SerializeField] private List<Collectible> collectedSpeedItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            collectedHealthItems = new List<Collectible>();
        }
        else
        {
            Destroy(gameObject);
        }

        // Find UI references initially
        FindUIReferences();
    }

    private void FindUIReferences()
    {
        if (healthBoosterText == null)
        {
            GameObject healthBoosterObject = GameObject.Find("HealthBoosterText");
            if (healthBoosterObject != null)
            {
                healthBoosterText = healthBoosterObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("HealthBoosterText GameObject not found in the scene.");
            }
        }

        if (speedBoosterText == null)
        {
            GameObject speedBoosterObject = GameObject.Find("SpeedBoosterText");
            if (speedBoosterObject != null)
            {
                speedBoosterText = speedBoosterObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("SpeedBoosterText GameObject not found in the scene.");
            }
        }

        UpdateBoosterCount(); // Update UI text initially
    }

    public void AddCollectibleForHealth(Collectible collectible)
    {
        collectedHealthItems.Add(collectible);
    }

    public void RemoveCollectibleFromHealth(Collectible collectible)
    {
        collectedHealthItems.Remove(collectible);
    }

    public List<Collectible> GetCollectedHealthItems()
    {
        return collectedHealthItems;
    }

    public void AddCollectibleForSpeed(Collectible collectible)
    {
        collectedSpeedItems.Add(collectible);
    }

    public void RemoveCollectibleFromSpeed(Collectible collectible)
    {
        collectedSpeedItems.Remove(collectible);
    }

    public List<Collectible> GetCollectedSpeedItems()
    {
        return collectedSpeedItems;
    }

    private void UpdateHealthBoosterButtonInteractability()
    {
        // Enable the health booster button if there is at least one health booster available
        if (healthBoosterButton != null)
        {
            healthBoosterButton.interactable = healthBoosterNo > 0;
        }
    }

    public void IncrementHealthBoosterCount()
    {
        healthBoosterNo++;
        UpdateBoosterCount();
        UpdateHealthBoosterButtonInteractability(); // Update button interactability after incrementing count
    }

    public void DecrementHealthBoosterCount()
    {
        healthBoosterNo--;
        UpdateBoosterCount();
        UpdateHealthBoosterButtonInteractability(); // Update button interactability after decrementing count
    }

    public void OnHealthBoosterButton()
    {
        BoostHealth[] zombies = FindObjectsOfType<BoostHealth>();
        foreach (BoostHealth zombie in zombies)
        {
            zombie.ApplyHealthBoost(healthBoostMultiplier, healthBoostTimeDuration);
        }

        // Decrement the health booster count after using it
        DecrementHealthBoosterCount();

        // Remove the collectible from the list
        if (collectedHealthItems.Count > 0)
        {
            RemoveCollectibleFromHealth(collectedHealthItems[0]);
        }
    }

    private void UpdateSpeedBoosterButtonInteractability()
    {
        // Enable the health booster button if there is at least one health booster available
        if (speedBoosterButton != null)
        {
            speedBoosterButton.interactable = speedBoosterNo > 0;
        }
    }

    public void IncrementSpeedBoosterCount()
    {
        speedBoosterNo++;
        UpdateBoosterCount();
        UpdateSpeedBoosterButtonInteractability();
    }

    public void DecrementSpeedBoosterCount()
    {
        speedBoosterNo--;
        UpdateBoosterCount();
        UpdateSpeedBoosterButtonInteractability();
    }

    public void OnSpeedBoosterButton()
    {
        BoostSpeed[] zombies = FindObjectsOfType<BoostSpeed>();
        foreach (BoostSpeed zombie in zombies)
        {
            zombie.ApplySpeedBoost(speedBoostMultiplier, speedBootTimeDuration);
        }

        // Decrement the speed booster count after using it
        DecrementSpeedBoosterCount();

        // Remove the collectible from the list
        if (collectedSpeedItems.Count > 0)
        {
            RemoveCollectibleFromSpeed(collectedSpeedItems[0]);
        }
    }

    public void UpdateBoosterCount()
    {
        if (healthBoosterText != null)
        {
            healthBoosterText.text = healthBoosterNo.ToString();
        }
        else
        {
            Debug.LogWarning("UI Text component for health booster is not assigned or found.");
        }

        if (speedBoosterText != null)
        {
            speedBoosterText.text = speedBoosterNo.ToString();
        }
        else
        {
            Debug.LogWarning("UI Text component for speed booster is not assigned or found.");
        }
    }
}
