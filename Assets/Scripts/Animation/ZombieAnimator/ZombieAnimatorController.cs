using UnityEngine;

// This script plays animation for the zombie
public class ZombieAnimatorController : AnimatorControllerBase
{
    public override void PlayIdle()
    {
        _animator.Play("zombie_idle");
    }

    public override void PlayRun()
    {
        _animator.Play("zombie_walk_forward");
    }

    public override void PlayAttack()
    {
        _animator.Play("zombie_attack");
    }

    public void PlayDie()
    {
        _animator.Play("zombie_death_standing");
    }
}
