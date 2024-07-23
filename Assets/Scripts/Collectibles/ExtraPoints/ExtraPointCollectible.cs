using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtraPointCollectible : Collectible, ICollectible
{
    private int extraPointsAdded;
    private float delayBeforeTrigger;
    private Collider col;
    private AudioManager audioManager;
    [SerializeField] private ExtraPointsDisplayer extraPointsDisplayer;
    
    private void Start()
    {
        delayBeforeTrigger = CollectibleManager.Instance.delayBeforeTrigger;
        extraPointsAdded = CollectibleManager.Instance.extraPointsAdded;
        col = GetComponent<Collider>();
        audioManager = FindObjectOfType<AudioManager>();
        extraPointsDisplayer = FindObjectOfType<ExtraPointsDisplayer>();
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

    public override void ApplyEffect(GameObject collector)
    {
        AddExtraPoints();
        if (extraPointsDisplayer != null)
        {
            extraPointsDisplayer.DisplayBoostUsed();
        }
        else
        {
            Debug.LogError("ExtraPointsDisplayer instance is not assigned.");
        }
    }
}
