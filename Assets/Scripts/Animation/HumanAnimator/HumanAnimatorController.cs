using UnityEngine;

public class HumanAnimatorController : AnimatorControllerBase
{
    public override void PlayIdle()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("idle");
        }
    }

    public override void PlayRun()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("run");
        }
    }

    public override void PlayAttack()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("wave"); // Just an example
        }
    }
}
