using System.Collections;
using UnityEngine;

public class ExtraPointCollectible : Collectible, ICollectible
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
        if (other.CompareTag("Zombie") || other.CompareTag("Player"))
        {
            // Play sound
            audioManager.Play("CollectiblePickUp");

            ApplyEffect(other.gameObject);
            Destroy(gameObject); // Optionally destroy the collectible after it's collected
        }
    }

    public override void ApplyEffect(GameObject collector)
    {
        Debug.Log("Non-persistent collectible collected!");
        AddExtraPoints();
    }

    private void AddExtraPoints()
    {
        // Find the ZombieCounter instance in the scene
        ZombieCounter zombieCounter = FindObjectOfType<ZombieCounter>();
        if (zombieCounter != null)
        {
            zombieCounter.AddPoints(extraPointsAdded);
        }
        else
        {
            Debug.LogError("ZombieCounter instance not found in the scene.");
        }
    }
}
