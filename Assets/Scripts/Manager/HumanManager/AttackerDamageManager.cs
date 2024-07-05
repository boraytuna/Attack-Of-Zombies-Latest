using UnityEngine;

public class AttackerDamageManager : MonoBehaviour
{
    public static AttackerDamageManager Instance { get; private set; }

    [System.Serializable]
    public struct DamageThreshold
    {
        public int zombieCountThreshold;
        public float multiplier;
    }

    [SerializeField] private DamageThreshold[] damageThresholds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of AttackerDamageManager found. Ensure there is only one.");
            Destroy(this.gameObject);
        }
    }
    
    public float GetMultiplier()
    {
        int zombieCount = ZombieManager.Instance != null ? ZombieManager.Instance.GetZombieCount() : 0;

        // Default multiplier is 1 (no change)
        float effectiveMultiplier = 1f;

        // Find the appropriate multiplier based on the current zombie count
        foreach (var threshold in damageThresholds)
        {
            if (zombieCount >= threshold.zombieCountThreshold)
            {
                effectiveMultiplier = threshold.multiplier;
            }
            else
            {
                // Since thresholds are sorted, break early if current count is less than threshold
                break;
            }
        }

        // // Log the current multiplier for debugging purposes
        // Debug.Log("Current multiplier: " + effectiveMultiplier);

        return effectiveMultiplier;
    }
}
