using UnityEngine;

public class SafePointCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Zombie"))
        {
            Debug.Log("Player reached the safe point");
            GamePlayEvents.TriggerPlayerReachedSafePoint();

            Destroy(gameObject);
        }
    }
}
