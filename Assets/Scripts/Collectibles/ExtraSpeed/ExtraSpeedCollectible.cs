using System.Collections;
using UnityEngine;

public class ExtraSpeedCollectible : Collectible
{
    private int extraPointsAdded;
    private float delayBeforeTrigger;
    private Collider col;
    private AudioManager audioManager;

    private void Start()
    {
        delayBeforeTrigger = CollectibleManager.Instance.delayBeforeTrigger;
        extraPointsAdded = CollectibleManager.Instance.extraPointsAdded;
        col = GetComponent<Collider>();
        audioManager = FindObjectOfType<AudioManager>();
        
        if (col != null)
        {
            col.isTrigger = false; // Ensure trigger is initially false
            StartCoroutine(EnableTriggerAfterDelay(delayBeforeTrigger)); // Start coroutine to set trigger after delay
        }
    }

    private IEnumerator EnableTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Zombie"))
        {
            // Play sound
            audioManager.Play("CollectiblePickUp");

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
    }
}
