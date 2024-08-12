using UnityEngine;

public class ShootTheZombiePolice : Shoot, IAttacker
{
    [SerializeField] private PoliceAnimationController policeAnimationController; // Reference to the animator controller

    protected override void Start()
    {
        base.Start();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    protected override void PlayAttackAnimation()
    {
        policeAnimationController.PlayAttack();
        PlayShootingSound();
    }

    protected override void PlayIdleAnimation()
    {
        policeAnimationController.PlayIdle();
    }

    protected override void PlayShootingSound()
    {
        if (audioManager != null)
        {
            audioManager.Play("PoliceShoot");
        }
        else
        {
            Debug.LogError("AudioManager is null");
        }
    }
}
