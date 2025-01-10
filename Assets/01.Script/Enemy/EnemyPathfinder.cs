using Hashira.Entities;
using Hashira.Pathfind;
using UnityEngine;

namespace Hashira.Enemies
{
    public class EnemyPathfinder : MonoBehaviour, IEntityComponent
    {
        private Enemy _enemy;
        private EntityMover _entityMover;

        private Pathfinder _pathfinder;

        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
            _entityMover = entity.GetEntityComponent<EntityMover>();
        }

        public void PathfindAndMove()
        {

        }
    }
}
