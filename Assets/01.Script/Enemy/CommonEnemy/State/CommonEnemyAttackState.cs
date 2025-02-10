using Hashira.Core.StatSystem;
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
        private EntityWeapon _entityWeapon;
        private EntityStat _entityStat;

        private StatElement _attackPowerElement;

        public CommonEnemyAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _entityAnimationTrigger = entity.GetEntityComponent<EntityAnimationTrigger>();
            _entityWeapon = entity.GetEntityComponent<EntityWeapon>();
            _entityStat = entity.GetEntityComponent<EntityStat>();

            _attackPowerElement = _entityStat.StatDictionary["AttackPower"];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _entityAnimationTrigger.OnAnimationTriggeredEvent += HandleOnAnimationTriggered;
        }

        private void HandleOnAnimationTriggered(EAnimationTrigger trigger)
        {
            if (trigger.HasFlag(EAnimationTrigger.Attack))
            {
                _entityWeapon.Attack(_attackPowerElement.IntValue, false);
            }
            if (trigger.HasFlag(EAnimationTrigger.End))
            {
                _entityStateMachine.ChangeState("Chase");
            }
        }
    }
}
