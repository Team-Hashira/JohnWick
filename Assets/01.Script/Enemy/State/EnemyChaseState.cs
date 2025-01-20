using Hashira.Entities;
using Hashira.LatestFSM;
using Hashira.Pathfind;
using System;
using UnityEngine;

namespace Hashira.Enemies
{
    public class EnemyChaseState : EntityState
    {
        private EnemyPathfinder _enemyPathfinder;

        public EnemyChaseState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyPathfinder = entity.GetEntityComponent<EnemyPathfinder>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Node targetNode = _entityStateMachine.GetShareVariable<Node>("TargetNode");
            _enemyPathfinder.PathfindAndMove(targetNode);
            _enemyPathfinder.OnMoveEndEvent += HandleOnMoveEndEvent;
        }

        private void HandleOnMoveEndEvent()
        {
            _entityStateMachine.ChangeState("Idle");
        }
    }
}
