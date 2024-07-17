using System.Collections;
using UnityEngine;

public class StarCollectible : Collectible, ICollectible
{
    private float timeDuration;
    private float delayBeforeTrigger;
    private Collider col;

    private void Start()
    {
        timeDuration = CollectibleManager.Instance.timeDurationForInvincibility;
        delayBeforeTrigger = CollectibleManager.Instance.delayBeforeTrigger;
        col = GetComponent<Collider>();
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
            ApplyEffect(other.gameObject);
            Destroy(gameObject); // Optionally destroy the collectible after it's collected
        }
    }

    public override void ApplyEffect(GameObject collector)
    {
        Debug.Log("Non-persistent collectible collected!");
        MakeAllZombiesInvincible(timeDuration);
    }

    private void MakeAllZombiesInvincible(float duration)
    {
        MakeInvincible[] allZombies = FindObjectsOfType<MakeInvincible>();
        foreach (MakeInvincible zombie in allZombies)
        {
            zombie.BecomeInvincible(duration);
        }
    }
}
