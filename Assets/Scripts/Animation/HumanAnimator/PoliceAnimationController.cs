using UnityEngine;

public class PoliceAnimationController : AnimatorControllerBase
{
    public override void PlayIdle()
    {
        _animator.Play("m_pistol_idle_A");
    }

    public override void PlayRun()
    {
        _animator.Play("m_pistol_run");
    }

    public override void PlayAttack()
    {
        _animator.Play("m_pistol_shoot");
    }
}
