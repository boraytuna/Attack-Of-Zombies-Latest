// using UnityEngine;

// public abstract class ArmoredVehicle : MonoBehaviour, IAttacker
// {
//     [SerializeField] protected float baseDamage = 50f;
//     [SerializeField] protected float pushForce = 5f; // Force to apply to the player
//     private AudioManager audioManager;
//     void Start()
//     {
//         audioManager = FindObjectOfType<AudioManager>();
//     }

//     // Implement the Attack method from IAttacker interface
//     public virtual void Attack(Collider targetCollider)
//     {
//         // Deal damage to the target if it has an IDamagable component
//         IDamagable damagable = targetCollider.GetComponent<IDamagable>();
//         if (damagable != null)
//         {
//             damagable.TakeDamage(baseDamage);
//             Debug.Log("ArmoredVehicle dealt " + baseDamage + " damage to " + targetCollider.name);
//         }
//         else
//         {
//             Debug.LogWarning("No IDamagable component found on " + targetCollider.name);
//         }
//     }

//     // Method to apply push force to the player
//     protected void ApplyPushForce(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             audioManager.Play("ZombieDeath");
//             Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
//             if (playerRigidbody != null)
//             {
//                 Vector3 pushDirection = other.transform.position - transform.position;
//                 pushDirection.y = 0; // Keep the push force horizontal for more natural behavior

//                 // Apply a more subtle force
//                 float forceMagnitude = Mathf.Clamp(pushForce * (1f - (pushDirection.magnitude / 10f)), 0f, pushForce);
//                 playerRigidbody.AddForce(pushDirection.normalized * forceMagnitude, ForceMode.Impulse);

//                 // Optionally, you could add some damping here
//             }
//         }
//     }

// }
using System.Collections;
using UnityEngine;

public abstract class ArmoredVehicle : MonoBehaviour, IAttacker
{
    [SerializeField] protected float baseDamage = 50f;
    [SerializeField] protected float pushForce = 5f; // Initial force applied to the player
    [SerializeField] protected float dampingDuration = 0.5f; // Time over which to damp the force
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Implement the Attack method from IAttacker interface
    public virtual void Attack(Collider targetCollider)
    {
        // Deal damage to the target if it has an IDamagable component
        IDamagable damagable = targetCollider.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(baseDamage);
            Debug.Log("ArmoredVehicle dealt " + baseDamage + " damage to " + targetCollider.name);
        }
        else
        {
            Debug.LogWarning("No IDamagable component found on " + targetCollider.name);
        }
    }

    // Method to apply push force to the player
    protected void ApplyPushForce(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.Play("ZombieDeath");
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                Vector3 pushDirection = other.transform.position - transform.position;
                pushDirection.y = 0; // Keep the push force horizontal for more natural behavior
                
                // Apply an initial impulse force
                playerRigidbody.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
                
                // Start damping the force over time
                StartCoroutine(DampVelocity(playerRigidbody));
            }
        }
    }

    // Coroutine to damp the player's velocity over time
    private IEnumerator DampVelocity(Rigidbody playerRigidbody)
    {
        float startTime = Time.time;
        Vector3 initialVelocity = playerRigidbody.velocity;

        while (Time.time < startTime + dampingDuration)
        {
            // Linearly interpolate the velocity to zero
            float t = (Time.time - startTime) / dampingDuration;
            playerRigidbody.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, t);
            yield return null; // Wait for the next frame
        }

        // Ensure the velocity is fully zeroed out
        playerRigidbody.velocity = Vector3.zero;
    }
}
