using Hashira.Entities;
using Hashira.FSM;
using Hashira.Pathfind;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemyChaseState : EntityState
    {
        private CommonEnemy _enemy;
        private EnemyPathfinder _enemyPathfinder;

        private Player _target;
        private EntityMover _targetMover;

        public CommonEnemyChaseState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemy = entity as CommonEnemy;
            _enemyPathfinder = entity.GetEntityComponent<EnemyPathfinder>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Node targetNode = _entityStateMachine.GetShareVariable<Node>("TargetNode");
            if (targetNode != null)
                _enemyPathfinder.PathfindAndMove(targetNode);
            else
            {
                _target = _entityStateMachine.GetShareVariable<Player>("Target"); // 없으면 null임.
                _targetMover = _target?.GetEntityComponent<PlayerMover>();
            }
            _enemyPathfinder.OnMoveEndEvent += HandleOnMoveEndEvent;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_target == null)
            {
                Player target = _enemy.DetectPlayer();
                if (target != null)
                {
                    _target = target;
                    _targetMover = target.GetEntityComponent<EntityMover>(true);
                }
            }
            else
            {
                if (_enemyPathfinder.TargetNode != _targetMover.CurrentNode)
                    _enemyPathfinder.PathfindAndMove(_targetMover.CurrentNode);
            }
        }

        private void HandleOnMoveEndEvent()
        {
            _entityStateMachine.ChangeState("Idle");
        }
    }
}
