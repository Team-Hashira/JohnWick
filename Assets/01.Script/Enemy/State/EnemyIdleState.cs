using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.LatestFSM;
using System;
using UnityEngine;

namespace Hashira.Enemies
{
    public class EnemyIdleState : ListeningSoundState
    {
        private EnemyMover _enemyMover;

        private float _idleStartTime;
        private float _waitDelay = 2f;
        public EnemyIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
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
