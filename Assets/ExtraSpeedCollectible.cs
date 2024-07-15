using UnityEngine;

public class ExtraSpeedCollectible : Collectible
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Zombie"))
        {
            // Apply the effect when collected by player or zombie
            ApplyEffect(other.gameObject);

            // Destroy the collectible after it's collected
            Destroy(gameObject);
        }
    }

    public override void ApplyEffect(GameObject collector)
    {
        // Increment health booster count in CollectibleManager
        CollectibleManager.Instance.IncrementSpeedBoosterCount();

        // Add this collectible to the list in CollectibleManager
        CollectibleManager.Instance.AddCollectibleForSpeed(this);
    }
}
