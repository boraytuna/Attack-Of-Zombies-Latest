using UnityEngine;

public class MakeInvincible : MonoBehaviour
{
    private Health healthComponent;
    private CollectibleManager collectibleManager;

    private void Awake()
    {
        healthComponent = GetComponent<Health>();
        if (healthComponent == null)
        {
            Debug.LogError("Health component not found on this game object.");
        }
    }

    void Start()
    {
        collectibleManager = CollectibleManager.Instance;
    }

    public void OnStarCollectibleButton()
    {
        collectibleManager.OnStarCollectibleButton();
    }

    public void BecomeInvincible(float duration)
    {
        if (healthComponent != null)
        {
            healthComponent.BecomeInvincible(duration);
        }
    }
}
