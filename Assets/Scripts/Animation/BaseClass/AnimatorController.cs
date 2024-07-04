using UnityEngine;

public abstract class AnimatorControllerBase : MonoBehaviour, IAnimate
{
    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public abstract void PlayIdle();
    public abstract void PlayRun();
    public abstract void PlayAttack();
}
