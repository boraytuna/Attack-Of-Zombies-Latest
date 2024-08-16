using UnityEngine;

public class SoldierAnimationController : AnimatorControllerBase
{
    public override void PlayIdle()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("m_weapon_idle_A");
        }
        
    }

    public override void PlayRun()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("m_weapon_run_rm");
        }
    }

    public override void PlayAttack()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("m_weapon_shoot");
        }
    }
}
