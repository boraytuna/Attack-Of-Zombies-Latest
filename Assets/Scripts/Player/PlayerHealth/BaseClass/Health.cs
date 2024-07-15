using System.Collections;
using UnityEngine;

public abstract class Health : MonoBehaviour, IDamagable
{
    [SerializeField] protected float maxHealth;
    [SerializeField] public float currentHealth;
    protected float damage;
    private bool isInvincible;
    private bool isBoosted;

    protected abstract void Die();

    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void BecomeInvincible(float duration)
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine(duration));
        }
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        Debug.Log($"{gameObject.name} is now invincible!");
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        Debug.Log($"{gameObject.name} is no longer invincible.");
    }

    public void BecomeBoostedHealth(float duration)
    {
        if (!isBoosted)
        {
            StartCoroutine(BoosterCoroutine(duration));
        }
    }

    private IEnumerator BoosterCoroutine(float duration)
    {
        isBoosted = true;
        Debug.Log($"{gameObject.name} has boosted health!");

        yield return new WaitForSeconds(duration);

        isBoosted = false;
        Debug.Log($"{gameObject.name} health boost ended.");
    }
}
