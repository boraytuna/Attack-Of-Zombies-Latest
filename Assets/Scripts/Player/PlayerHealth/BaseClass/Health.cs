using UnityEngine;

public abstract class Health : MonoBehaviour, IDamagable
{
    [SerializeField] protected float maxHealth;  // Maximum health
    [SerializeField] protected float currentHealth;    // Current health
    protected float damage; // Damage field is only declared here

    protected abstract void Die();  // Abstract method for handling death

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Reduce current health by the damage amount

        // Check if the object is dead
        if (currentHealth <= 0)
        {
            Die(); // Call the Die function if health drops to or below zero
        }
    }
}
