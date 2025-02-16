using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyIdleState : EnemyListeningSoundState
    {
        private CommonEnemy _enemy;
        private EnemyPathfinder _enemyPathfinder;
        private EntityEmoji _entityEmoji;

        private float _idleStartTime;
        private float _waitDelay = 2f;

        public CommonEnemyIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyPathfinder = entity.GetEntityComponent<EnemyPathfinder>();
            _entityEmoji = entity.GetEntityComponent<EntityEmoji>();
            _enemy = entity as CommonEnemy;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyPathfinder.StopMove();
            _idleStartTime = Time.time;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_idleStartTime + _waitDelay < Time.time)
            {
                _entityStateMachine.ChangeState("Patrol");
            }
            Player player = _enemy.DetectPlayer();
            if (player != null)
            {
                _entityEmoji?.ShowEmoji(EEmotion.Surprise, 1f);
                _entityStateMachine.SetShareVariable("Target", player);
                _entityStateMachine.ChangeState("Chase");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
