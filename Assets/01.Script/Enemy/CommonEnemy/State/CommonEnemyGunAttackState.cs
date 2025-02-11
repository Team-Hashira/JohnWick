using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyGunAttackState : EntityState
    {
        private CommonEnemy _commonEnemy;
        private Player _target;

        private EntityGunWeapon _entityGunWeapon;
        private EntityStat _entityStat;
        private EnemyPathfinder _enemyPathfinder;

        private StatElement _attackPowerElement;
        private StatElement _attackSpeedElement;

        private float _lastAttackTime = 0;

        public CommonEnemyGunAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _commonEnemy = entity as CommonEnemy;

            _entityGunWeapon = entity.GetEntityComponent<EntityGunWeapon>();
            _entityStat = entity.GetEntityComponent<EntityStat>();
            _enemyPathfinder = entity.GetEntityComponent<EnemyPathfinder>();

            _attackSpeedElement = _entityStat.StatDictionary["AttackSpeed"];
            _attackPowerElement = _entityStat.StatDictionary["AttackPower"];

            _lastAttackTime = 1.5f;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target = _entityStateMachine.GetShareVariable<Player>("Target");
            _enemyPathfinder.StopMove();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _entityGunWeapon.LookTarget(_target.transform.position);   
            if (_commonEnemy.IsTargetOnAttackRange(_target.transform))
            {
                if (_lastAttackTime + 1.5f < Time.time)
                {
                    _entityGunWeapon.Attack(_attackPowerElement.IntValue, false, _commonEnemy.WhatIsPlayer);
                    _lastAttackTime = Time.time;
                    Debug.Log("����");
                }
            }
            else
            {
                _entityStateMachine.ChangeState("Chase");
            }
        }
    }
}
