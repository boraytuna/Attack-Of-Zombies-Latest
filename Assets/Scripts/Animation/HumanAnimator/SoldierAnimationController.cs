// using UnityEngine;

// // This script plays animation for the zombie
// [RequireComponent(typeof(Animator))]
// public class SoldierAnimationController : MonoBehaviour
// {
//     private Animator _animator;

//     private void Awake()
//     {
//         _animator = GetComponent<Animator>();
//     }

//     public void PlayIdle()
//     {
//         _animator.Play("m_weapon_idle_A");
//     }

//     public void PlayRun()
//     {
//         _animator.Play("m_weapon_run_rm");
//     }

//     public void PlayAttack()
//     {
//         _animator.Play("m_weapon_shoot");
//     }
// }
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
