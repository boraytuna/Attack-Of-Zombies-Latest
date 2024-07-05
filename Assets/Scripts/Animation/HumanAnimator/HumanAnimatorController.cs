using UnityEngine;

public class HumanAnimatorController : AnimatorControllerBase
{
    public override void PlayIdle()
    {
        _animator.Play("idle");
    }

    public override void PlayRun()
    {
        _animator.Play("run");
    }

    public override void PlayAttack()
    {
        _animator.Play("wave"); // Just an example; adjust as needed
    }
}
