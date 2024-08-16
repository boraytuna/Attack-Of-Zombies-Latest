using UnityEngine;

// This script plays animation for the zombie
public class ZombieAnimatorController : AnimatorControllerBase
{
    public override void PlayIdle()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("zombie_idle");
        }
    }

    public override void PlayRun()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("zombie_walk_forward");
        }
    }

    public override void PlayAttack()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("zombie_attack");
        } 
    }

    public void PlayDie()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("zombie_death_standing");
        }
    }
}
