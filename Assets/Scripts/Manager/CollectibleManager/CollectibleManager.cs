using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    [Header("Number of Collectibles")]
    [SerializeField] private int healthBoosterNo; // Number of health boosters player has
    [SerializeField] private int speedBoosterNo; // Number of speed boosters player has
    [SerializeField] private int starCollectibleNo; // Number of star collectibles player has

    // Initial number of collectibles when the player first starts
    private int initialHealthBoosterNo = 0; 
    private int initialSpeedBoosterNo = 0; 
    private int initialStarCollectibleNo = 0; 

    [Header("Boosters")]
    [SerializeField] public float delayBeforeTrigger = 0.5f; // Delay for non-persistent collectibles to become collectible
    [SerializeField] public int extraPointsAdded; // Extra points added to zombie count to finish game
    [SerializeField] public float timeDurationForInvincibility; // Time duration of zombies invincibility
    [SerializeField] private float healthBoostMultiplier; // Multiplier for health boost
    [SerializeField] private float healthBoostTimeDuration; // Duration of health boost
    [SerializeField] private float speedBoostMultiplier; // Multiplier for speed boost
    [SerializeField] private float speedBoostTimeDuration; // Duration of speed boost

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI healthBoosterText; // UI Text for health boosters count
    [SerializeField] private TextMeshProUGUI speedBoosterText; // UI Text for speed boosters count
    [SerializeField] private TextMeshProUGUI starCollectibleText; // UI Text for star collectibles count
    [SerializeField] private Button healthBoosterButton; // UI Button for health booster
    [SerializeField] private Button speedBoosterButton; // UI Button for speed booster
    [SerializeField] private Button starCollectibleButton; // UI Button for star collectible
    [SerializeField] private TextMeshProUGUI boostUsedText; // Reference to the text object to indicate the used Boost

    private bool isGamePlayLoaded = false;

    private void OnEnable()
    {
        // Register event handlers when the script is enabled
        GameManager.OnGameStateChanged += OnGameStateChanged;
        SceneManager.sceneLoaded += OnSceneLoaded; // Register the scene loaded event
    }

    private void OnDisable()
    {
        // Unregister event handlers when the script is disabled
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister the scene loaded event
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        // Find UI references initially
        FindUIReferences();

        // Set initial player preferences for collectibles
        SetPlayerPrefsForCollectibles();
        
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;

        ActivateUIReferences();
       
        LoadCollectibleData();
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        if(state == GameState.ActualGamePlay)
        {
            Debug.Log("State is actual game play");
            
        }
    }

    // Update UI with current booster counts
    private void ActivateUIReferences()
    {
        UpdateBoosterCount();
        UpdateHealthBoosterButtonInteractability();
        UpdateSpeedBoosterButtonInteractability();
        UpdateStarCollectibleButtonInteractability();
    }

    void Update()
    {
        // Continuously update the booster count
        UpdateBoosterCount();
    }

    // Find UI elements in the scene and assign them
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

        if (starCollectibleText == null)
        {
            GameObject starCollectibleTextObject = GameObject.Find("StarCollectibleText");
            if (starCollectibleTextObject != null)
            {
                starCollectibleText = starCollectibleTextObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("StarCollectibleText GameObject not found in the scene.");
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

        if (starCollectibleButton == null)
        {
            GameObject starCollectibleButtonObject = GameObject.Find("StarCollectible");
            if (starCollectibleButtonObject != null)
            {
                starCollectibleButton = starCollectibleButtonObject.GetComponent<Button>();
            }
            else
            {
                Debug.LogError("StarCollectibleButton GameObject not found in the scene.");
            }
        }

        // Find booster used text

        if (boostUsedText == null)
        {
            GameObject boostUsedTextObject = GameObject.Find("BoosterUsedText");
            if(boostUsedTextObject != null)
            {
                boostUsedText = boostUsedTextObject.GetComponent<TextMeshProUGUI>();
            }
            else 
            {
                Debug.Log("Booster used text is null");
            }
        }
    }

    // Set the health booster button interactability based on available boosters and game state
    private void UpdateHealthBoosterButtonInteractability()
    {    
        if (healthBoosterButton != null)
        {
            healthBoosterButton.interactable = healthBoosterNo > 0 && GameManager.Instance.State == GameState.ActualGamePlay  || GameManager.Instance.State == GameState.Countdown;
        }
    }

    // Increase the health booster count and update UI/button state
    public void IncrementHealthBoosterCount()
    {
        healthBoosterNo++;
        UpdateBoosterCount();
        UpdateHealthBoosterButtonInteractability(); 
    }

    // Decrease the health booster count and update UI/button state
    public void DecrementHealthBoosterCount()
    {
        healthBoosterNo--;
        UpdateBoosterCount();
        UpdateHealthBoosterButtonInteractability(); 
    }

    // Called on healthBooster clicked
    public void OnHealthBoosterButton()
    {
        // Apply health boost to all zombies and decrement the health booster count
        BoostHealth[] zombies = FindObjectsOfType<BoostHealth>();
        foreach (BoostHealth zombie in zombies)
        {
            zombie.ApplyHealthBoost(healthBoostMultiplier, healthBoostTimeDuration);
        }

        DecrementHealthBoosterCount();

        // Disable the button and start cooldown
        healthBoosterButton.interactable = false;
        StartCoroutine(CooldownHealthBoosterButton(healthBoostTimeDuration));

        // Display boost used message
        StartCoroutine(DisplayBoostUsed("Health Booster", 2.0f)); // Display for 2 seconds
    }

    // Wait for the cooldown duration and then re-enable the button
    private IEnumerator CooldownHealthBoosterButton(float duration)
    {     
        yield return new WaitForSeconds(duration);
        UpdateHealthBoosterButtonInteractability(); 
    }
    
    // Set the speed booster button interactability based on available boosters and game state
    private void UpdateSpeedBoosterButtonInteractability()
    {
        if (speedBoosterButton != null)
        {
            speedBoosterButton.interactable = speedBoosterNo > 0 && GameManager.Instance.State == GameState.ActualGamePlay || GameManager.Instance.State == GameState.Countdown;
        }
    }

    // Increase the speed booster count and update UI/button state
    public void IncrementSpeedBoosterCount()
    {
        speedBoosterNo++;
        UpdateBoosterCount();
        UpdateSpeedBoosterButtonInteractability();
    }

    // Decrease the speed booster count and update UI/button state
    public void DecrementSpeedBoosterCount()
    {
        speedBoosterNo--;
        UpdateBoosterCount();
        UpdateSpeedBoosterButtonInteractability();
    }

    // Called on speedbooster button click
    public void OnSpeedBoosterButton()
    {
        // Apply speed boost to all zombies and decrement the speed booster count
        BoostSpeed(speedBoostMultiplier, speedBoostTimeDuration);

        DecrementSpeedBoosterCount();

        // Disable the button and start cooldown
        speedBoosterButton.interactable = false;
        StartCoroutine(CooldownSpeedBoosterButton(speedBoostTimeDuration));
        
        // Display boost used message
        StartCoroutine(DisplayBoostUsed("Speed Booster", 2.0f)); // Display for 2 seconds
    }

    // Apply speed boost to zombies
    private void BoostSpeed(float multiplier, float duration)
    {
        ZombieSpeedManager.Instance.ApplySpeedBoost(multiplier, duration);
    }

    // Wait for the cooldown duration and then re-enable the button
    private IEnumerator CooldownSpeedBoosterButton(float duration)
    {
        yield return new WaitForSeconds(duration);
        UpdateSpeedBoosterButtonInteractability(); 
    }

    // Set the star collectible button interactability based on available collectibles and game state
    private void UpdateStarCollectibleButtonInteractability()
    {
        if (starCollectibleButton != null)
        {
            starCollectibleButton.interactable = starCollectibleNo > 0 && GameManager.Instance.State == GameState.ActualGamePlay || GameManager.Instance.State == GameState.Countdown;
        }
    }

    // Increase the star collectible count and update UI/button state
    public void IncrementStarCollectibleCount()
    {
        starCollectibleNo++;
        UpdateBoosterCount();
        UpdateStarCollectibleButtonInteractability();
    }

    // Decrease the star collectible count and update UI/button state
    public void DecrementStarCollectibleCount()
    {
        starCollectibleNo--;
        UpdateBoosterCount();
        UpdateStarCollectibleButtonInteractability();
    }

    // Called on star button clicked
    public void OnStarCollectibleButton()
    {
        // Apply invincibility to all zombies and decrement the star collectible count
        MakeZombiesInvincible(timeDurationForInvincibility);

        DecrementStarCollectibleCount();

        // Disable the button and start cooldown
        starCollectibleButton.interactable = false;
        StartCoroutine(CooldownStarCollectibleButton(timeDurationForInvincibility));

        // Display boost used message
        StartCoroutine(DisplayBoostUsed("Star Booster", 2.0f)); // Display for 2 seconds
    }

    // Make all zombies invincible for the specified duration
    private void MakeZombiesInvincible(float duration)
    {
        MakeInvincible[] allZombies = FindObjectsOfType<MakeInvincible>();
        foreach (MakeInvincible zombie in allZombies)
        {
            zombie.BecomeInvincible(duration);
        }
    }

    // Wait for the cooldown duration and then re-enable the button
    private IEnumerator CooldownStarCollectibleButton(float duration)
    {
        yield return new WaitForSeconds(duration);
        UpdateStarCollectibleButtonInteractability(); 
    }
    
    // Update UI texts with current booster counts
    private void UpdateBoosterCount()
    {
        if (healthBoosterText != null)
        {
            healthBoosterText.text = healthBoosterNo.ToString();
        }

        if (speedBoosterText != null)
        {
            speedBoosterText.text = speedBoosterNo.ToString();
        }

        if (starCollectibleText != null)
        {
            starCollectibleText.text = starCollectibleNo.ToString();
        }
    }

    // Handle changes in game state
    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.ActualGamePlay:
                // Check if this is the first time entering gameplay or if the state was reset
                if (!isGamePlayLoaded)
                {
                    LoadCollectibleData(); // Load collectible data when gameplay starts
                    isGamePlayLoaded = true; // Mark gameplay as loaded
                }
                break;

            case GameState.Victory:
                SaveCollectibleData(); // Save collectible data when player achieves victory
                isGamePlayLoaded = false; // Reset flag for next gameplay session
                break;

            case GameState.MainMenu:
            case GameState.LevelMenu:
            case GameState.Lose:
                ResetCollectibles(); // Reset collectibles when returning to menus or losing
                isGamePlayLoaded = false; // Reset flag for next gameplay session
                break;

            case GameState.PauseState:
                // No change to collectibles when pausing
                break;
        }
    }

    // Handle changes in new scene load
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Refind UI components and update booster counts when a new scene is loaded
        FindUIReferences(); 
        ActivateUIReferences();

        // Reload collectible data when a new scene is loaded
        LoadCollectibleData();
    }

    // Initialize or load player preferences for collectibles
    private void SetPlayerPrefsForCollectibles()
    {
        if (!PlayerPrefs.HasKey("HealthBoost"))
        {
            PlayerPrefs.SetInt("HealthBoost", initialHealthBoosterNo);
            Debug.Log("Initialized HealthBoost to " + initialHealthBoosterNo);
        }
        else
        {
            healthBoosterNo = PlayerPrefs.GetInt("HealthBoost");
            Debug.Log("Loaded HealthBoost: " + healthBoosterNo);
        }

        if (!PlayerPrefs.HasKey("SpeedBoost"))
        {
            PlayerPrefs.SetInt("SpeedBoost", initialSpeedBoosterNo);
            Debug.Log("Initialized SpeedBoost to " + initialSpeedBoosterNo);
        }
        else
        {
            speedBoosterNo = PlayerPrefs.GetInt("SpeedBoost");
            Debug.Log("Loaded SpeedBoost: " + speedBoosterNo);
        }

        if (!PlayerPrefs.HasKey("StarBoost"))
        {
            PlayerPrefs.SetInt("StarBoost", initialStarCollectibleNo);
            Debug.Log("Initialized StarBoost to " + initialStarCollectibleNo);
        }
        else
        {
            starCollectibleNo = PlayerPrefs.GetInt("StarBoost");
            Debug.Log("Loaded StarBoost: " + starCollectibleNo);
        }
    }

    // Load the current collectible counts from PlayerPrefs
    private void LoadCollectibleData()
    {
        healthBoosterNo = PlayerPrefs.GetInt("HealthBoosterNo", initialHealthBoosterNo);
        speedBoosterNo = PlayerPrefs.GetInt("SpeedBoosterNo", initialSpeedBoosterNo);
        starCollectibleNo = PlayerPrefs.GetInt("StarCollectibleNo", initialStarCollectibleNo);
        UpdateBoosterCount();
    }

    // Save the current collectible counts to PlayerPrefs
    private void SaveCollectibleData()
    {
        PlayerPrefs.SetInt("HealthBoosterNo", healthBoosterNo);
        PlayerPrefs.SetInt("SpeedBoosterNo", speedBoosterNo);
        PlayerPrefs.SetInt("StarCollectibleNo", starCollectibleNo);
    }

    // Reset collectible counts to their initial values
    private void ResetCollectibles()
    {
        healthBoosterNo = initialHealthBoosterNo;
        speedBoosterNo = initialSpeedBoosterNo;
        starCollectibleNo = initialStarCollectibleNo;
        UpdateBoosterCount();
    }

    // Save the current collectible data when the application is closed
    public void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HealthBoost", healthBoosterNo);
        PlayerPrefs.SetInt("SpeedBoost", speedBoosterNo);
        PlayerPrefs.SetInt("StarBoost", starCollectibleNo);
        PlayerPrefs.Save();
    }

    // Display the name of boost 
    private IEnumerator DisplayBoostUsed(string boostName, float displayDuration)
    {
        if (boostUsedText != null)
        {
            boostUsedText.text = $"{boostName} Used";
            yield return new WaitForSeconds(displayDuration);
            boostUsedText.text = "";
        }
        else
        {
            Debug.LogError("boostUsedText is not assigned.");
        }
    }
}
