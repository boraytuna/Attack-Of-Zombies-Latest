// using System.Collections;
// using UnityEngine;

// public class ArmoredVehicleMovement : MonoBehaviour
// {
//     [SerializeField] private Transform[] policeObjects; 
//     [SerializeField] private float speed = 5f;
//     [SerializeField] private float radius = 10f;
//     private float angle = 0f;
//     private bool isMoving = false;
//     private Collider carCollider;
//     private Rigidbody rb;
//     public bool allObjectsDestroyed = false;
//     [SerializeField] private Transform startPosition;
//     protected AudioManager audioManager;

//     private Transform playerTransform; 
//     [SerializeField] private float soundTriggerDistance = 15f; 

//     protected bool isSoundPlaying = false; // Track if the sound is currently playing

//     void Start()
//     {
//         audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
//         carCollider = GetComponent<Collider>();
//         rb = GetComponent<Rigidbody>();
    
//         playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

//         GameManager.OnGameStateChanged += OnGameStateChanged;
//         GamePlayEvents.OnPlayerDeath += OnPlayerDeath; // Subscribe to the player death event
        
//         // Debugging step: Log objects already in the scene
//         foreach (var obj in policeObjects)
//         {
//             Debug.Log($"{obj.name} is already in the scene at position {obj.position}");
//         }

//         StartCar();
//     }

//     void Update()
//     {
//         if (!allObjectsDestroyed && AreAllRelevantObjectsDisabled())
//         {
//             StopCar();
//             allObjectsDestroyed = true;
//             enabled = false;

//             StartCoroutine(DestroyGameObject());
//             return;
//         }

//         if (isMoving)
//         {
//             MoveCar();
//         }
//         else
//         {
//             StopCar();
//         }

//         CheckToPlaySound(); // Check sound play status every frame
//     }

//     protected virtual void PlayMoveSound()
//     {
//         if (!isSoundPlaying)
//         {
//             Debug.Log("Base PlayMoveSound called");
//             isSoundPlaying = true;
//             // Add code here to actually play the sound using audioManager
//         }
//     }

//     protected virtual void StopMoveSound()
//     {
//         if (isSoundPlaying)
//         {
//             Debug.Log("Base StopMoveSound called");
//             isSoundPlaying = false;
//         }
//     }

//     private void CheckToPlaySound()
//     {
//         if (playerTransform != null)
//         {
//             float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
//             // Debug.Log($"Distance to player: {distanceToPlayer}");

//             if (distanceToPlayer <= soundTriggerDistance)
//             {
//                 PlayMoveSound();
//             }
//             else
//             {
//                 StopMoveSound();
//             }
//         }
//     }

//     private IEnumerator DestroyGameObject()
//     {
//         yield return new WaitForSeconds(6f);
//         gameObject.SetActive(false);
//     }

//     private void MoveCar()
//     {
//         angle += speed * Time.deltaTime;

//         float x = startPosition.position.x + Mathf.Cos(angle) * radius;
//         float z = startPosition.position.z + Mathf.Sin(angle) * radius;
//         Vector3 newPosition = new Vector3(x, transform.position.y, z);

//         transform.position = newPosition;

//         Vector3 direction = newPosition - startPosition.position;
//         if (direction != Vector3.zero)
//         {
//             Quaternion targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
//             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
//         }
//     }

//     private void StopCar()
//     {
//         if (isMoving)
//         {
//             isMoving = false;
//             if (carCollider != null)
//             {
//                 carCollider.isTrigger = false;
//             }

//             if (rb != null)
//             {
//                 rb.isKinematic = true; // Disable physics interactions
//             }
            
//             StopMoveSound(); // Ensure sound stops if car stops
//         }
//     }

//     private void StartCar()
//     {
//         isMoving = true;
//         // if (carCollider != null)
//         // {
//         //     carCollider.isTrigger = true;
//         // }
//     }

//     private bool AreAllRelevantObjectsDisabled()
//     {
//         foreach (Transform obj in policeObjects)
//         {
//             if (obj != null && obj.gameObject.activeInHierarchy)
//             {
//                 return false; // If any object is active, return false
//             }
//         }
//         return true; // All objects are disabled
//     }

//     private void OnGameStateChanged(GameState newState)
//     {
//         if (newState == GameState.ActualGamePlay)
//         {
//             StartCar();
//         }
//         else
//         {
//             StopCar();
//         }
//     }

//     private void OnPlayerDeath()
//     {
//         StopMoveSound(); // Stop the sound when the player dies
//     }

//     private void OnDestroy()
//     {
//         GameManager.OnGameStateChanged -= OnGameStateChanged;
//         GamePlayEvents.OnPlayerDeath -= OnPlayerDeath; // Unsubscribe from the player death event
//         // StopMoveSound(); // Ensure sound stops when object is destroyed
//     }
// }
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
    public bool allObjectsDestroyed = false;
    [SerializeField] private Transform startPosition;
    protected AudioManager audioManager;

    private Transform playerTransform; 
    [SerializeField] private float soundTriggerDistance = 15f; 

    [SerializeField] protected bool isSoundPlaying = false; // Track if the sound is currently playing

    void Start()
    {
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        carCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        GameManager.OnGameStateChanged += OnGameStateChanged;
        GamePlayEvents.OnPlayerDeath += OnPlayerDeath; // Subscribe to the player death event
        
        // Debugging step: Log objects already in the scene
        foreach (var obj in policeObjects)
        {
            Debug.Log($"{obj.name} is already in the scene at position {obj.position}");
        }

        StartCar();
    }

    void Update()
    {
        CheckToPlaySound();

        if (!allObjectsDestroyed && AreAllRelevantObjectsDisabled())
        {
            StopCar();
            allObjectsDestroyed = true;
            enabled = false;

            StartCoroutine(DestroyGameObject());
            return;
        }
    }

    protected virtual void PlayMoveSound()
    {
    }

    protected virtual void StopMoveSound()
    {
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
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }

    private IEnumerator RotateCar()
    {
        while (isMoving)
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

            yield return null; // Continue on the next frame
        }
    }

    private void StopCar()
    {
        if (isMoving)
        {
            isMoving = false;
            isSoundPlaying = false;
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
        isSoundPlaying = true;
        StartCoroutine(RotateCar());
        // if (carCollider != null)
        // {
        //     carCollider.isTrigger = true;
        // }
    }

    private bool AreAllRelevantObjectsDisabled()
    {
        foreach (Transform obj in policeObjects)
        {
            if (obj != null && obj.gameObject.activeInHierarchy)
            {
                return false; // If any object is active, return false
            }
        }
        return true; // All objects are disabled
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

    private void OnPlayerDeath()
    {
        audioManager.Stop("PoliceCar");
        audioManager.Stop("SoldierTank");
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        GamePlayEvents.OnPlayerDeath -= OnPlayerDeath; // Unsubscribe from the player death event
    }
}
