using Hashira.Entities;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.FSM
{
    public class EntityState<T> : EntityStateBase where T : Entity
    {
        protected T _owner;

        public EntityState(T owner, StateMachine stateMachine, string animationName)
        {
            _EntityAnimator = owner.GetEntityComponent<EntityAnimator>();
            SetAnimationHash(animationName);
            _owner = owner;
            _stateMachine = stateMachine;
        }
    }

    public class EntityStateBase
    {
        protected StateMachine _stateMachine;
        protected EntityAnimator _EntityAnimator;

        private int _animationHash;

        protected void SetAnimationHash(string animationName)
            => _animationHash = Animator.StringToHash(animationName);

        public virtual void Enter() { _EntityAnimator?.SetParam(_animationHash, true); }
        public virtual void Update() { }
        public virtual void Exit() { _EntityAnimator?.SetParam(_animationHash, false); }
    }
}