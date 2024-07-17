using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }
    [SerializeField] public float delayBeforeTrigger = 0.5f; // Delay for non-persistent collectibles to become collectible
    [SerializeField] public int extraPointsAdded; // Extra points added to zombie count to finish game
    [SerializeField] public float timeDurationForInvincibility; // Time duration of zombies invincibility
    [SerializeField] private float healthBoostMultiplier; // The number current health gets multiplied with 
    [SerializeField] private float healthBoostTimeDuration; // The time duration of how long the health boosted
    [SerializeField] private float speedBoostMultiplier; // The number current speed gets multiplied with
    [SerializeField] private float speedBoostTimeDuration;  // Time duration for the speed boost
    [SerializeField] private int healthBoosterNo; // Number of health boosters player has
    [SerializeField] private int speedBoosterNo; // Number of speed boosters player has
    [SerializeField] private TextMeshProUGUI healthBoosterText;
    [SerializeField] private TextMeshProUGUI speedBoosterText;
    [SerializeField] private Button healthBoosterButton;
    [SerializeField] private Button speedBoosterButton;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        SceneManager.sceneLoaded += OnSceneLoaded; // Register the scene loaded event
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister the scene loaded event
    }

    void Awake()
    {
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

    void Start()
    {
        // Find UI references initially
        FindUIReferences();

        // Update UI text initially
        UpdateBoosterCount();
        UpdateHealthBoosterButtonInteractability();
        UpdateSpeedBoosterButtonInteractability();
    }

    void Update()
    {
        UpdateBoosterCount();
    }

    private void FindUIReferences()
    {
        if (healthBoosterText == null)
        {
            GameObject healthBoosterTextObject = GameObject.Find("HealthBoosterText");
            if (healthBoosterTextObject != null)
            {
                healthBoosterText = healthBoosterTextObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("HealthBoosterText GameObject not found in the scene.");
            }
        }

        if (speedBoosterText == null)
        {
            GameObject speedBoosterTextObject = GameObject.Find("SpeedBoosterText");
            if (speedBoosterTextObject != null)
            {
                speedBoosterText = speedBoosterTextObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("SpeedBoosterText GameObject not found in the scene.");
            }
        }

        if (healthBoosterButton == null)
        {
            GameObject healthBoosterButtonObject = GameObject.Find("HealthBooster");
            if (healthBoosterButtonObject != null)
            {
                healthBoosterButton = healthBoosterButtonObject.GetComponent<Button>();
            }
            else
            {
                Debug.LogError("HealthBoosterButton GameObject not found in the scene.");
            }
        }

        if (speedBoosterButton == null)
        {
            GameObject speedBoosterButtonObject = GameObject.Find("SpeedBooster");
            if (speedBoosterButtonObject != null)
            {
                speedBoosterButton = speedBoosterButtonObject.GetComponent<Button>();
            }
            else
            {
                Debug.LogError("SpeedBoosterButton GameObject not found in the scene.");
            }
        }
    }

    private void UpdateHealthBoosterButtonInteractability()
    {
        // Enable the health booster button if there is at least one health booster available and the game state is ActualGamePlay
        if (healthBoosterButton != null)
        {
            healthBoosterButton.interactable = healthBoosterNo > 0 && GameManager.Instance.State == GameState.ActualGamePlay;
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
        // Apply health boost to all zombies
        BoostHealth[] zombies = FindObjectsOfType<BoostHealth>();
        foreach (BoostHealth zombie in zombies)
        {
            zombie.ApplyHealthBoost(healthBoostMultiplier, healthBoostTimeDuration);
        }

        // Decrement the health booster count after using it
        DecrementHealthBoosterCount();

        // Disable the button and start cooldown
        healthBoosterButton.interactable = false;
        StartCoroutine(CooldownHealthBoosterButton(healthBoostTimeDuration));
    }

    private IEnumerator CooldownHealthBoosterButton(float duration)
    {
        yield return new WaitForSeconds(duration);
        UpdateHealthBoosterButtonInteractability(); // Re-enable the button after cooldown
    }

    private void UpdateSpeedBoosterButtonInteractability()
    {
        // Enable the speed booster button if there is at least one speed booster available and the game state is ActualGamePlay
        if (speedBoosterButton != null)
        {
            speedBoosterButton.interactable = speedBoosterNo > 0 && GameManager.Instance.State == GameState.ActualGamePlay;
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
        // Apply speed boost to all zombies
        BoostSpeed(speedBoostMultiplier, speedBoostTimeDuration);

        // Decrement the speed booster count after using it
        DecrementSpeedBoosterCount();

        // Disable the button and start cooldown
        speedBoosterButton.interactable = false;
        StartCoroutine(CooldownSpeedBoosterButton(speedBoostTimeDuration));
    }

    private IEnumerator CooldownSpeedBoosterButton(float duration)
    {
        yield return new WaitForSeconds(duration);
        UpdateSpeedBoosterButtonInteractability(); // Re-enable the button after cooldown
    }

    private void BoostSpeed(float multiplier, float duration)
    {
        ZombieSpeedManager.Instance.ApplySpeedBoost(multiplier, duration);
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

    private void OnGameStateChanged(GameState newState)
    {
        // Update the button interactability based on the new game state
        UpdateHealthBoosterButtonInteractability();
        UpdateSpeedBoosterButtonInteractability();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-find UI references in the new scene
        FindUIReferences();
        UpdateHealthBoosterButtonInteractability();
        UpdateSpeedBoosterButtonInteractability();
        UpdateBoosterCount();
    }
}
