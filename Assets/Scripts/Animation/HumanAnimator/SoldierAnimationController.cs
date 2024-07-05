using UnityEngine;

public class SoldierAnimationController : AnimatorControllerBase
{
    public override void PlayIdle()
    {
        _animator.Play("m_weapon_idle_A");
    }

    public override void PlayRun()
    {
        _animator.Play("m_weapon_run_rm");
    }

    public override void PlayAttack()
    {
        _animator.Play("m_weapon_shoot");
    }
}
