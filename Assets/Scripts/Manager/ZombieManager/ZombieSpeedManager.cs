using UnityEngine;

public class ZombieSpeedManager : MonoBehaviour
{
    public static ZombieSpeedManager Instance { get; private set; }

    [SerializeField]
    private GameObject player; // Refence to player object for central speed
    private PlayerMovement playerMovement; // Reference to movement script to get the isMoving variable

    public float currentSpeed;  // Local speed variable
    
    [Range(1f,20f)]
    [SerializeField] private float minSpeed; // Minumum speed variable
    [Range(1f,20f)]
    [SerializeField] private float middleSpeed; // New middle speed variable
    [Range(1f,20f)]
    [SerializeField] private float maxSpeed; // Maximum speed variable
    private float speedIncreaseRate = 1f; // The rate speed increases

    public bool maxSpeedReached = false; // Bool for checking if the player reached to the max speed

    private void Awake()
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

    public void Initialize()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        GetCurrentSpeed();
    }

    private void Update()
    {
        if (playerMovement != null && playerMovement.isMoving)
        {
            IncreaseSpeedOverTime();
        }
        else
        {
            ResetSpeed();
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

    // Reset the speed to minumum if the max speed has not been reached, if it has set it to middle speed for the next time.
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
}
