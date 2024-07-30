using UnityEngine;

public class ArmoredVehicleMovement : MonoBehaviour
{
    [SerializeField] private Transform[] policeObjects; // Array of relevant objects
    [SerializeField] private float speed = 5f;            // Speed of the car
    [SerializeField] private float radius = 10f;          // Radius of the circular path
    private float angle = 0f;                             // Current angle of the car on the circular path
    private bool isMoving = false;                        // Flag to determine if the car should move
    private Collider carCollider;                         // Reference to the car's collider
    private bool allObjectsDestroyed = false;             // Flag to indicate if all objects are destroyed
    [SerializeField] private Transform startPosition;     // Start position for the car movement
    private PoliceCarSpawner carSpawner;                   // Reference to the car spawner

    void Start()
    {
        carCollider = GetComponent<Collider>();
        carSpawner = FindObjectOfType<PoliceCarSpawner>(); // Find the PoliceCarSpawner in the scene

        // Register to listen to game state changes
        GameManager.OnGameStateChanged += OnGameStateChanged;

        // Ensure the car starts moving as soon as it's instantiated
        StartCar();
    }

    void Update()
    {
        // Check if all relevant objects are destroyed only once
        if (!allObjectsDestroyed && AreAllRelevantObjectsDestroyed())
        {
            StopCar();
            allObjectsDestroyed = true; // Set the flag to true
            enabled = false; // Disable the script
            
            // Notify the spawner to respawn a new car
            if (carSpawner != null)
            {
                carSpawner.RequestRespawn();
            }
            return;
        }

        // Continue moving the car if all relevant objects are not destroyed
        if (isMoving)
        {
            MoveCar();
        }
        else
        {
            StopCar();
        }
    }

    private void MoveCar()
    {
        angle += speed * Time.deltaTime;

        // Calculate the new position of the car
        float x = startPosition.position.x + Mathf.Cos(angle) * radius;
        float z = startPosition.position.z + Mathf.Sin(angle) * radius;
        Vector3 newPosition = new Vector3(x, transform.position.y, z);

        // Move the car to the new position
        transform.position = newPosition;

        // Rotate the car to face the direction of movement
        Vector3 direction = newPosition - startPosition.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    private void StopCar()
    {
        isMoving = false;
        if (carCollider != null)
        {
            carCollider.isTrigger = false; // Turn off the collider
        }
    }

    private void StartCar()
    {
        isMoving = true;
        if (carCollider != null)
        {
            carCollider.isTrigger = true; // Ensure the collider is set as trigger
        }
    }

    private bool AreAllRelevantObjectsDestroyed()
    {
        foreach (Transform obj in policeObjects)
        {
            if (obj != null)
            {
                return false;
            }
        }
        return true;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.ActualGamePlay)
        {
            StartCar(); // Ensure the car starts moving when the game state is actual gameplay
        }
        else
        {
            StopCar();
        }
    }

    private void OnDestroy()
    {
        // Unregister from game state changes
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
}
