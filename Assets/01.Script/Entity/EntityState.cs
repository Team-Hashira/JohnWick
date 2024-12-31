using UnityEngine;

public class EntityState<T> : EntityStateBase where T : Entity
{
    protected T _owner;

    private readonly int _AnimationHash;

    public EntityState(T owner, StateMachine stateMachine, string animationName)
    {
        _owner = owner;
        _stateMachine = stateMachine;

        _AnimationHash = Animator.StringToHash(animationName);
    }
}

public class EntityStateBase
{
    protected StateMachine _stateMachine;

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
