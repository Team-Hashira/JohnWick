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

        private EntityRenderer _entityRenderer;
        private EntityWeaponHolder _entityGunWeapon;
        private EntityStat _entityStat;
        private EnemyPathfinder _enemyPathfinder;

        private StatElement _attackPowerElement;
        private StatElement _attackSpeedElement;

        private float _lastAttackTime = 0;

        public CommonEnemyGunAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _commonEnemy = entity as CommonEnemy;

            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _entityGunWeapon = entity.GetEntityComponent<EntityWeaponHolder>();
            _entityStat = entity.GetEntityComponent<EntityStat>();
            _enemyPathfinder = entity.GetEntityComponent<EnemyPathfinder>();

            _attackSpeedElement = _entityStat.StatDictionary["AttackSpeed"];
            _attackPowerElement = _entityStat.StatDictionary["AttackPower"];

            _lastAttackTime = -1.5f;
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
            _entityRenderer.LookTarget(_target.transform.position);
            _entityGunWeapon.LookTarget(_target.transform.position);
            if (_commonEnemy.IsTargetOnAttackRange(_target.transform))
            {
                if (_lastAttackTime + 1.5f < Time.time)
                {
                    _entityGunWeapon.Attack(1, true, _commonEnemy.WhatIsPlayer);
                    _lastAttackTime = Time.time;
                }
            }
            else
            {
                _entityStateMachine.ChangeState("Chase");
            }
        }
    }
}
