using UnityEngine;

public class GamePlayZombieMovement : MonoBehaviour, IMoveable
{
    private Rigidbody _rigidbody;
    private ZombieAnimatorController zombieAnimatorController;

    private Transform playerTransform; // Reference to Player's transform
    private ZombieCounter zombieCounter; // Reference to zombie counter script

    private float rotateSpeed = 6f;
    private float followRadius = 5f; // Radius around the player to follow input
    private bool isFollowingInput = true;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        zombieAnimatorController = GetComponent<ZombieAnimatorController>();

        // Initialize playerTransform with the player's transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Find the zombie counter script
        zombieCounter = FindObjectOfType<ZombieCounter>();
        Debug.Log("Entering Move() method.");
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Increase the radius if necessary
        IncreaseFollowRadius();

        // Use the calculated follow radius
        if (distanceToPlayer > followRadius)
        {
            // If outside the follow radius, catch up to the player
            isFollowingInput = false;
            MoveTowardsPlayer();
        }
        else
        {
            // If inside the follow radius, follow player input
            isFollowingInput = true;
            Move();
        }
    }

    public void Move()
    {
        if (isFollowingInput)
        {
            Vector3 moveVector = PlayerInputManager.Instance.MoveVector;

            if (ZombieSpeedManager.Instance != null && moveVector != null)
            {
                float moveSpeed = ZombieSpeedManager.Instance.GetCurrentSpeed();

                if (moveVector.magnitude > 0)
                {
                    Vector3 direction = new Vector3(moveVector.x, 0, moveVector.z).normalized;
                    Vector3 targetVelocity = direction * moveSpeed;

                    // Clamp the velocity to ensure it doesn't exceed the maximum speed
                    targetVelocity = Vector3.ClampMagnitude(targetVelocity, moveSpeed);

                    // Apply the target velocity to the Rigidbody
                    _rigidbody.velocity = new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.z);

                    // Rotate the zombie towards the movement direction
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);

                    // Play the run animation
                    zombieAnimatorController.PlayRun();
                }
                else
                {
                    // Play the idle animation
                    zombieAnimatorController.PlayIdle();

                    // Stop the zombie's movement
                    _rigidbody.velocity = Vector3.zero;
                }
            }

        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        if (ZombieSpeedManager.Instance != null)
        {
            float moveSpeed = ZombieSpeedManager.Instance.GetCurrentSpeed();
            Vector3 targetVelocity = direction * moveSpeed * 1.5f;

            // Apply the target velocity to the Rigidbody
            _rigidbody.velocity = new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.z);

            // Rotate the zombie towards the player
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            // Play the run animation
            zombieAnimatorController.PlayRun();
        }

    }

    // Method to increase the follow radius based on the number of zombies
    public void IncreaseFollowRadius()
    {
        int zombieNumber = zombieCounter.GetZombieCount();

        // Adjust follow radius based on the number of zombies
        if (zombieNumber >= 129 && zombieNumber <= 249)
        {
            followRadius = 7f;
        }
        if (zombieNumber >= 250 && zombieNumber <= 349)
        {
            followRadius = 9f;
        }
        if (zombieNumber >= 350)
        {
            followRadius = 11f;
        }
    }

}