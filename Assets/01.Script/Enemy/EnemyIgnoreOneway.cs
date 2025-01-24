using Hashira.Entities;
using UnityEngine;

namespace Hashira.Enemies
{
    public class EnemyIgnoreOneway : MonoBehaviour, IEntityComponent
    {
        private Enemy _enemy;

        [SerializeField]
        private Collider2D _onewayCollider;
        [SerializeField]
        private Collider2D _commonCollider;

        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;
        }

        public void SetIgnoreOneway(bool isIgnore)
        {
            _onewayCollider.enabled = !isIgnore;
        }
    }
}
