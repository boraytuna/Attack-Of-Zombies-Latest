using UnityEngine;

public class ZombieHealth : Health, IDamagable
{
    // [SerializeField] private ZombieList zombieList;
    [SerializeField] private ZombieCounter zombieCounter;
    [SerializeField] private ZombieAnimatorController zombieAnimatorController;
    private AudioManager audioManager;

    private void Awake()
    {
        zombieCounter = FindObjectOfType<ZombieCounter>();
        // zombieList = FindObjectOfType<ZombieList>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        currentHealth = maxHealth;

        // if (zombieList != null)
        // {
        //     zombieList.AddZombie(gameObject);
        // }
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

        if (zombieCounter != null)
        {
            zombieCounter.DecrementZombieCount();
        }
        else
        {
            Debug.LogError("Zombie counter is null");
        }

        // if (zombieList != null)
        // {
        //     zombieList.RemoveZombie(gameObject);
        // }
        // else
        // {
        //     Debug.LogError("ZombieList is null");
        // }

        Destroy(gameObject);
    }
}
