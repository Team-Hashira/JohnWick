using Hashira.Entities;
using Hashira.FSM;
using Hashira.Pathfind;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies
{
    public class EnemyChaseState : EntityState
    {
        private Enemy _enemy;
        private EnemyPathfinder _enemyPathfinder;

        private Player _target;
        private EntityMover _targetMover;

        public EnemyChaseState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemy = entity as Enemy;
            _enemyPathfinder = entity.GetEntityComponent<EnemyPathfinder>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Node targetNode = _entityStateMachine.GetShareVariable<Node>("TargetNode");
            _enemyPathfinder.PathfindAndMove(targetNode);
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
