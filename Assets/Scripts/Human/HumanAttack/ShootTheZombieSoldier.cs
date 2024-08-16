using UnityEngine;

public class ShootTheZombieSoldier : Shoot
{
    [SerializeField] private SoldierAnimationController soldierAnimationController; // Reference to the animator controller 

    protected override void Start()
    {
        base.Start();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    protected override void PlayAttackAnimation()
    {
        soldierAnimationController.PlayAttack();
    }

    protected override void PlayIdleAnimation()
    {
        soldierAnimationController.PlayIdle();
    }

    protected override void PlayShootingSound()
    {
        if (audioManager != null)
        {
            audioManager.Play("SoldierShoot");
        }
        else
        {
            Debug.LogError("AudioManager is null");
        }
    }
}
