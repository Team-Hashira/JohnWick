using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyIdleState : EnemyListeningSoundState
    {
        private EnemyMover _enemyMover;

        private float _idleStartTime;
        private float _waitDelay = 2f;

        public CommonEnemyIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.StopImmediately();
            _idleStartTime = Time.time;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_idleStartTime + _waitDelay < Time.time)
            {
                _entityStateMachine.ChangeState("Patrol");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
