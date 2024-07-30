using UnityEngine;

public abstract class ArmoredVehicle : MonoBehaviour, IAttacker
{
    [SerializeField] protected float baseDamage = 50f;

    // Implement the Attack method from IAttacker interface
    public virtual void Attack(Collider targetCollider)
    {
        // Deal damage to the target if it has an IDamagable component
        IDamagable damagable = targetCollider.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(baseDamage);
            Debug.Log("ArmoredVehicle dealt " + baseDamage + " damage to " + targetCollider.name);
        }
        else
        {
            Debug.LogWarning("No IDamagable component found on " + targetCollider.name);
        }
    }
}
