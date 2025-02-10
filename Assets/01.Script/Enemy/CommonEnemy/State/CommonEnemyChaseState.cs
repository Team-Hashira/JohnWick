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
            _target = _entityStateMachine.GetShareVariable<Player>("Target");
            if (_target != null)
            {
                _targetMover = _target?.GetEntityComponent<PlayerMover>();
            }
            else // Target이 null일 경우엔 무조건 TargetNode가 넘어옴.
            {
                Node targetNode = _entityStateMachine.GetShareVariable<Node>("TargetNode");
                _enemyPathfinder.PathfindAndMove(targetNode);
                _enemyPathfinder.OnMoveEndEvent += HandleOnMoveEndEvent;
            }
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
                    _enemyPathfinder.OnMoveEndEvent -= HandleOnMoveEndEvent;
                }
            }
            else
            {
                if(_enemy.IsTargetOnAttackRange(_target.transform))
                {
                    _entityStateMachine.ChangeState("Attack");
                    return;
                }
                if (_enemyPathfinder.TargetNode != _targetMover.CurrentNode)
                    _enemyPathfinder.PathfindAndMove(_targetMover.CurrentNode);
            }
        }

        public override void OnExit()
        {
            if (_target == null)
                _enemyPathfinder.OnMoveEndEvent -= HandleOnMoveEndEvent;
            base.OnExit();
        }

        private void HandleOnMoveEndEvent()
        {
            _entityStateMachine.ChangeState("Idle");
        }
    }
}
