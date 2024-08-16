using UnityEngine;

public class PoliceAnimationController : AnimatorControllerBase
{
    public override void PlayIdle()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("m_pistol_idle_A");
        }
    }

    public override void PlayRun()
    {
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("m_pistol_run");
        }   
    }

    public override void PlayAttack()
    {       
        if (gameObject.activeInHierarchy) // Check if the game object is active
        {
            _animator.Play("m_pistol_shoot");
        } 
    }
}
