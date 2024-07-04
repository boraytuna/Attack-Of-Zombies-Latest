using UnityEngine;

public class ShootTheZombiePolice : Shoot
{
    [SerializeField] private PoliceAnimationController policeAnimationController; // Reference to the animator controller 

    protected override void PlayAttackAnimation()
    {
        policeAnimationController.PlayAttack();
    }

    protected override void PlayIdleAnimation()
    {
        policeAnimationController.PlayIdle();
    }
}
