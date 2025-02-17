using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.SiegeEnemy
{
    public class SiegeEnemyAttackState : EntityState
    {
        private SiegeEnemy _siegeEnemy;
        private Player _target;

        private EntityRenderer _entityRenderer;
        private EntityGunWeapon _entityGunWeapon;
        private EntityStat _entityStat;
        private EnemyPathfinder _enemyPathfinder;

        private StatElement _attackPowerElement;
        private StatElement _attackSpeedElement;

        private float _lastAttackTime = 0;

        public SiegeEnemyAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _siegeEnemy = entity as SiegeEnemy;

            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
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
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _entityRenderer.LookTarget(_target.transform.position);
            _entityGunWeapon.LookTarget(_target.transform.position);
            if (_siegeEnemy.IsTargetOnAttackRange(_target.transform))
            {
                if (_lastAttackTime + 1.5f < Time.time)
                {
                    _entityGunWeapon.Attack(1, true, _siegeEnemy.WhatIsPlayer);
                    _lastAttackTime = Time.time;
                }
            }
            if (_siegeEnemy.IsWallBetweenTarget(_target.transform))
            {
                _entityStateMachine.ChangeState("Recon");
            }
        }
    }
}
