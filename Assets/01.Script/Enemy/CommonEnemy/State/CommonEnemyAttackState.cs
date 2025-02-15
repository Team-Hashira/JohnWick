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
        private EnemyPathfinder _enemyPathfinder;
        private DamageCaster2D _damageCaster;
        private EntityStat _entityStat;

        private StatElement _attackPowerElement;

        public CommonEnemyAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyPathfinder = entity.GetEntityComponent<EnemyPathfinder>();
            _damageCaster = entity.GetEntityComponent<EntityRenderer>().VisualTrm.Find("DamageCaster").GetComponent<DamageCaster2D>();
            _entityStat = entity.GetEntityComponent<EntityStat>();

            _attackPowerElement = _entityStat.StatDictionary["AttackPower"];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggered;
            _enemyPathfinder.StopMove();
        }

        private void HandleOnAnimationTriggered(EAnimationTriggerType trigger, int hash)
        {
            if (trigger == EAnimationTriggerType.Trigger)
            {
                _damageCaster.CastDamage(_attackPowerElement.IntValue);
            }
            if (trigger == EAnimationTriggerType.End)
            {
                _entityStateMachine.ChangeState("Chase");
            }
        }

        public override void OnExit()
        {
            Debug.Log("³ª°¡±â");
            _entityAnimator.OnAnimationTriggeredEvent -= HandleOnAnimationTriggered;
            base.OnExit();
        }
    }
}
