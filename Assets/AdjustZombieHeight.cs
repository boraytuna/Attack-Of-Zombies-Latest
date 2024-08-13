using UnityEngine;

public class AdjustZombieHeight : MonoBehaviour
{
    public float desiredHeight = 0.2f;
    private bool isActivated = false;

    private void OnEnable()
    {
        // Register to the activation event if needed
        if (!isActivated)
        {
            AdjustHeight();
            isActivated = true;
        }
    }

    private void OnBecameVisible()
    {
        // Ensure that height adjustment only happens when the zombie becomes visible
        if (!isActivated)
        {
            AdjustHeight();
            isActivated = true;
        }
    }

    private void AdjustHeight()
    {
        Vector3 position = transform.position;
        
        // Adjust the height
        position.y = desiredHeight;
        transform.position = position;

        // Handle Rigidbody if present
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;  // Ensure Rigidbody is properly handled
        }
    }
}
