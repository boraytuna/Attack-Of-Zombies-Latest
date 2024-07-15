using System.Collections;
using UnityEngine;

public class ZombieSpeedManager : MonoBehaviour
{
    public static ZombieSpeedManager Instance { get; private set; }

    [SerializeField] private string playerHolderName = "PlayerHolder"; // Name of the empty GameObject holding the player
    private GameObject playerHolder; // Reference to the empty GameObject holding the player
    private GameObject player; // Reference to player object for central speed
    private PlayerMovement playerMovement; // Reference to movement script to get the isMoving variable

    public float currentSpeed;  // Local speed variable
    
    [Range(1f, 20f)]
    [SerializeField] private float minSpeed; // Minimum speed variable
    [Range(1f, 20f)]
    [SerializeField] private float middleSpeed; // New middle speed variable
    [Range(1f, 20f)]
    [SerializeField] private float maxSpeed; // Maximum speed variable
    private float speedIncreaseRate = 1f; // The rate speed increases

    public bool maxSpeedReached = false; // Bool for checking if the player reached the max speed
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerHolder = GameObject.Find(playerHolderName); // Find the PlayerHolder object by name
        if (playerHolder != null)
        {
            player = playerHolder.GetComponentInChildren<PlayerMovement>().gameObject;
            playerMovement = player.GetComponent<PlayerMovement>();
            
            if (player == null)
            {
                Debug.LogError("Player object not found in playerHolder.");
            }
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement component not found on player.");
            }
        }
        else
        {
            Debug.LogError("Player holder is not assigned.");
        }
    }

    void Update()
    {
        if (playerMovement != null)
        {
            if (playerMovement.isMoving)
            {
                IncreaseSpeedOverTime();
            }
            else
            {
                ResetSpeed();
            }
        }
    }

    // Method to increase the speed.
    private void IncreaseSpeedOverTime()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        }

        if (currentSpeed >= maxSpeed)
        {
            maxSpeedReached = true;
        }
    }

    // Reset the speed to minimum if the max speed has not been reached; if it has, set it to middle speed for the next time.
    private void ResetSpeed()
    {
        if (maxSpeedReached)
        {
            currentSpeed = middleSpeed;
        }
        else
        {
            currentSpeed = minSpeed;
        }
    }

    // Return float for movement scripts.
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void BoostZombieSpeed(float boostMultiplier, float duration)
    {
        StartCoroutine(ApplySpeedBoost(boostMultiplier, duration));
    }

    private IEnumerator ApplySpeedBoost(float boostMultiplier, float duration)
    {
        // Increase the speed temporarily
        currentSpeed *= boostMultiplier;

        yield return new WaitForSeconds(duration);

        // Reset speed back to normal
        ResetSpeed();
    }
}