using UnityEngine;

public class HeklperZombieHealth : Health, IDamagable
{
    [SerializeField] private ZombieAnimatorController zombieAnimatorController;
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
        Debug.Log("Zombie died!");
        audioManager.Play("ZombieDeath");

        if (zombieAnimatorController != null)
        {
            zombieAnimatorController.PlayDie();
        }
        else
        {
            Debug.LogError("Animator is null");
        }

        Destroy(gameObject);
    }
}
