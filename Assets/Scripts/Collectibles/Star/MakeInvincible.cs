using UnityEngine;

public class MakeInvincible : MonoBehaviour
{
    private Health healthComponent;

    private void Awake()
    {
        healthComponent = GetComponent<Health>();
        if (healthComponent == null)
        {
            Debug.LogError("Health component not found on this game object.");
        }
    }

    public void BecomeInvincible(float duration)
    {
        if (healthComponent != null)
        {
            healthComponent.BecomeInvincible(duration);
        }
    }
}
