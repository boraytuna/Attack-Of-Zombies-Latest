using UnityEngine;

public class ShootTheZombieSoldier : Shoot
{
    [SerializeField] private SoldierAnimationController soldierAnimationController; // Reference to the animator controller 

    protected override void PlayAttackAnimation()
    {
        soldierAnimationController.PlayAttack();
    }

    protected override void PlayIdleAnimation()
    {
        soldierAnimationController.PlayIdle();
    }

    // Additional soldier-specific functionality can be added here if needed
}
