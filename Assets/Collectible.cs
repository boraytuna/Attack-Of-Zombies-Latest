using UnityEngine;

public abstract class Collectible : MonoBehaviour, ICollectible
{
    public abstract void ApplyEffect(GameObject collector);
}
