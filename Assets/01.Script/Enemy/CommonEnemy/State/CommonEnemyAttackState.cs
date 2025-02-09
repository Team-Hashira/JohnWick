using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyAttackState : EntityState
    {
        private EntityAnimationTrigger _entityAnimationTrigger;

        public CommonEnemyAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _entityAnimationTrigger = entity.GetEntityComponent<EntityAnimationTrigger>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _entityAnimationTrigger.OnAnimationTriggeredEvent += HandleOnAnimationTriggered;
        }

        private void HandleOnAnimationTriggered(EAnimationTrigger trigger)
        {
        }
    }
}
