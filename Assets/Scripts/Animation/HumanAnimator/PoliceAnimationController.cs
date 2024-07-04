// using UnityEngine;

// // This script plays animation for the zombie
// [RequireComponent(typeof(Animator))]
// public class PoliceAnimationController : MonoBehaviour
// {
//     private Animator _animator;

//     private void Awake()
//     {
//         _animator = GetComponent<Animator>();
//     }

//     public void PlayIdle()
//     {
//         _animator.Play("m_pistol_idle_A");
//     }

//     public void PlayRun()
//     {
//         _animator.Play("m_pistol_run");
//     }

//     public void PlayAttack()
//     {
//         _animator.Play("m_pistol_shoot");
//     }
// }
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
