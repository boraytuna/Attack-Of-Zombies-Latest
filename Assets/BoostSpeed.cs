using System.Collections;
using UnityEngine;

public class BoostSpeed : MonoBehaviour
{
    private ZombieSpeedManager speedManager; // Reference to the ZombieSpeedManager

    private void Start()
    {
        speedManager = ZombieSpeedManager.Instance;
        if (speedManager == null)
        {
            Debug.LogError("ZombieSpeedManager instance not found.");
            enabled = false; // Disable this script if manager is not found
        }
    }

    public void ApplySpeedBoost(float boostMultiplier, float duration)
    {
        speedManager.BoostZombieSpeed(boostMultiplier, duration);
    }
}
