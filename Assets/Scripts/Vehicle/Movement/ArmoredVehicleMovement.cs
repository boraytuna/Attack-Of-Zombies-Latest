using System.Collections;
using UnityEngine;

public class ArmoredVehicleMovement : MonoBehaviour
{
    [SerializeField] private Transform[] policeObjects; 
    [SerializeField] private float speed = 5f;
    [SerializeField] private float radius = 10f;
    private float angle = 0f;
    private bool isMoving = false;
    private Collider carCollider;
    private Rigidbody rb;
    private bool allObjectsDestroyed = false;
    [SerializeField] private Transform startPosition;
    
    private CarSpawner carSpawner;
    protected AudioManager audioManager;

    private Transform playerTransform; 
    [SerializeField] private float soundTriggerDistance = 15f; 

    protected bool isSoundPlaying = false; // Track if the sound is currently playing

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        carCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
  
        playerTransform = FindObjectOfType<PlayerMovement>()?.transform;

        GameManager.OnGameStateChanged += OnGameStateChanged;
        StartCar();
    }

    void Update()
    {
        if (!allObjectsDestroyed && AreAllRelevantObjectsDestroyed())
        {
            StopCar();
            allObjectsDestroyed = true;
            enabled = false;

            if (carSpawner != null)
            {
                carSpawner.RequestRespawn();
            }

            StartCoroutine(DestroyGameObject());
            return;
        }

        if (isMoving)
        {
            MoveCar();
        }
        else
        {
            StopCar();
        }

        CheckToPlaySound(); // Check sound play status every frame
    }

    protected virtual void PlayMoveSound()
    {
        if (!isSoundPlaying)
        {
            Debug.Log("Base PlayMoveSound called");
            isSoundPlaying = true;
            // Add code here to actually play the sound using audioManager
        }
    }

    protected virtual void StopMoveSound()
    {
        if (isSoundPlaying)
        {
            Debug.Log("Base StopMoveSound called");
            isSoundPlaying = false;
            // Add code here to actually stop the sound using audioManager
        }
    }

    private void CheckToPlaySound()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            // Debug.Log($"Distance to player: {distanceToPlayer}");

            if (distanceToPlayer <= soundTriggerDistance)
            {
                PlayMoveSound();
            }
            else
            {
                StopMoveSound();
            }
        }
    }

    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void MoveCar()
    {
        angle += speed * Time.deltaTime;

        float x = startPosition.position.x + Mathf.Cos(angle) * radius;
        float z = startPosition.position.z + Mathf.Sin(angle) * radius;
        Vector3 newPosition = new Vector3(x, transform.position.y, z);

        transform.position = newPosition;

        Vector3 direction = newPosition - startPosition.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    private void StopCar()
    {
        if (isMoving)
        {
            isMoving = false;
            if (carCollider != null)
            {
                carCollider.isTrigger = false;
            }

            if (rb != null)
            {
                rb.isKinematic = true; // Disable physics interactions
            }
            
            StopMoveSound(); // Ensure sound stops if car stops
        }
    }

    private void StartCar()
    {
        isMoving = true;
        // if (carCollider != null)
        // {
        //     carCollider.isTrigger = true;
        // }
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
            StartCar();
        }
        else
        {
            StopCar();
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        StopMoveSound(); // Ensure sound stops when object is destroyed
    }
}
