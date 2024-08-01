using UnityEngine;

public class HelperZombieHealth : Health, IDamagable
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    protected override void Die()
    {
        audioManager.Play("ZombieDeath");
        Destroy(gameObject);
    }
}
