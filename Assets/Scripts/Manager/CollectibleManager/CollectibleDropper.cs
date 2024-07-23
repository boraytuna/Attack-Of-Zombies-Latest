using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDropper : MonoBehaviour
{
    public GameObject[] collectibles; // Array to hold collectible prefabs
    public float[] dropProbabilities; // Array to hold drop probabilities for each collectible
    public float dropChance = 0.1f; // Chance for any collectible to drop (e.g., 0.5 for 50%)
    public LayerMask groundLayer; // Layer mask to define what layers are considered ground

    private void Start()
    {
        if (collectibles.Length != dropProbabilities.Length)
        {
            Debug.LogError("Collectibles and drop probabilities arrays must have the same length.");
            return;
        }
    }

    public void TryDropCollectible(Vector3 position)
    {
        // Check if any collectible should drop based on dropChance
        if (Random.Range(0f, 1f) <= dropChance)
        {
            // Proceed to determine which collectible to drop
            float randomValue = Random.Range(0f, 1f);
            float cumulativeProbability = 0f;

            for (int i = 0; i < collectibles.Length; i++)
            {
                cumulativeProbability += dropProbabilities[i];
                if (randomValue <= cumulativeProbability)
                {
                    DropCollectible(collectibles[i], position);
                    break;
                }
            }
        }
    }

    private void DropCollectible(GameObject collectiblePrefab, Vector3 position)
    {
        Vector3 groundPosition = FindGroundPosition(position);
        Instantiate(collectiblePrefab, groundPosition, Quaternion.identity);
    }

    private Vector3 FindGroundPosition(Vector3 originalPosition)
    {
        Vector3 rayStart = originalPosition + Vector3.up * 10f;
        Ray ray = new Ray(rayStart, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 20f, groundLayer))
        {
            return hit.point;
        }

        return originalPosition;
    }
}
