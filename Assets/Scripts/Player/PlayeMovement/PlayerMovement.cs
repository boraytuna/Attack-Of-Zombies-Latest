using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMoveable
{
    [SerializeField] private FloatingJoystick _joystick; // Reference to the floating joystick
    [SerializeField] private ZombieAnimatorController zombieAnimatorController; // Reference to the animator controller
    [SerializeField] private float rotateSpeed; // Rotation speed of the player

    private Rigidbody _rigidbody; // Reference to the Rigidbody component
    private Vector3 _moveVector; // Vector for storing movement input
    public bool isMoving; // Boolean to keep track of movement for speed

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        zombieAnimatorController = GetComponent<ZombieAnimatorController>();
    }

    private void Update()
    {
        Move();
    }

    // Method to handle player movement
    public void Move()
    {
        _moveVector = Vector3.zero;
        _moveVector.x = _joystick.Horizontal;
        _moveVector.z = _joystick.Vertical;

        // Update the InputManager with the player's input
        if (PlayerInputManager.Instance != null)
        {
            PlayerInputManager.Instance.UpdateMoveVector(_moveVector);
        }
        else
        {
            Debug.LogError("PlayerInputManager Instance is null.");
        }

        // If there's joystick input, rotate the player and play the run animation
        if (_moveVector.magnitude > 0)
        {
            // Set the boolean true
            isMoving = true;

            if(ZombieSpeedManager.Instance != null)
            {
                float moveSpeed = ZombieSpeedManager.Instance.GetCurrentSpeed();
                
                Vector3 direction = new Vector3(_moveVector.x, 0, _moveVector.z).normalized;
                Vector3 targetVelocity = direction * moveSpeed;

                // Clamp the velocity to ensure it doesn't exceed the maximum speed
                targetVelocity = Vector3.ClampMagnitude(targetVelocity, moveSpeed);

                // Apply the target velocity to the Rigidbody
                _rigidbody.velocity = new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.z);

                // Rotate the player towards the movement direction
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);

                zombieAnimatorController.PlayRun(); // Play the run animation
            }
            
        }
        else
        {
            // Set the boolean false
            isMoving = false;

            zombieAnimatorController.PlayIdle(); // Play the idle animation

            // Stop the player's movement
            _rigidbody.velocity = Vector3.zero;
        }
    }

    // Reset method to reset player movement
    public void Reset()
    {
        // Reset the movement vector and velocity
        _moveVector = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;

        // Reset the player's position and rotation if needed
        transform.position = Vector3.zero;  // Or any initial position
        transform.rotation = Quaternion.identity;

        // Reset the animation to idle
        if (zombieAnimatorController != null)
        {
            zombieAnimatorController.PlayIdle();
        }

        // Reset the moving state
        isMoving = false;
    }
}
