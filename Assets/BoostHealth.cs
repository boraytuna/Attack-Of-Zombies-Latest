using System.Collections;
using UnityEngine;

public class BoostHealth : MonoBehaviour
{
    private Health healthComponent;
    private Coroutine boostCoroutine;

    private void Awake()
    {
        healthComponent = GetComponent<Health>();
        if (healthComponent == null)
        {
            Debug.LogError("Health component not found on this game object.");
        }
    }

    // Apply health boost for a certain duration with a multiplier
    public void ApplyHealthBoost(float multiplier, float duration)
    {
        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine); // Stop the current boost coroutine if running
        }
        boostCoroutine = StartCoroutine(HealthBoostCoroutine(multiplier, duration));
    }

    private IEnumerator HealthBoostCoroutine(float multiplier, float duration)
    {
        healthComponent.BecomeBoostedHealth(duration); // Activate health boost state

        float originalHealth = healthComponent.currentHealth;
        healthComponent.currentHealth *= multiplier; // Apply health multiplier

        Debug.Log($"{gameObject.name}'s health boosted!");

        yield return new WaitForSeconds(duration);

        healthComponent.currentHealth = originalHealth; // Reset health back to original value
        healthComponent.BecomeInvincible(0f); // Deactivate health boost state

        Debug.Log($"{gameObject.name}'s health boost ended.");
    }
}
