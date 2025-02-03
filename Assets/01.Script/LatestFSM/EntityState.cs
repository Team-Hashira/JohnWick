using Hashira.Entities;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.LatestFSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        protected StateSO _stateSO;
        protected EntityStateMachine _entityStateMachine;
        protected EntityAnimator _entityAnimator;

        public EntityState(Entity entity, StateSO stateSO)
        {
            _entity = entity;
            _stateSO = stateSO;
            _entityStateMachine = entity.GetEntityComponent<EntityStateMachine>();
            _entityAnimator = entity.GetEntityComponent<EntityAnimator>();
        }

        public virtual void OnEnter()
        {
            _entityAnimator?.ClearAnimationTriggerDictionary();
            _entityAnimator?.SetParam(_stateSO.animatorParam, true);
        }

        public virtual void OnUpdate() { }

        public virtual void OnExit()
        {
            _entityAnimator?.SetParam(_stateSO.animatorParam, false);
        }
    }
}
