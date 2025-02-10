using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.CommonEnemy
{
    public class CommonEnemy : Enemy
    {
        [Header("Detect Player Setting")]
        [SerializeField]
        private Transform _eye;
        [SerializeField]
        private LayerMask _whatIsPlayer;
        [SerializeField]
        private LayerMask _whatIsGround;

        private StatElement _fovElement;
        private StatElement _sightElement;
        private StatElement _attackRangeElement;

        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
            _fovElement = _entityStat.StatDictionary["FieldOfView"];
            _sightElement = _entityStat.StatDictionary["Sight"];
            _attackRangeElement = _entityStat.StatDictionary["AttackRange"];
        }


        public override Player DetectPlayer()
        {
            Collider2D coll = Physics2D.OverlapCircle(transform.position, _sightElement.Value, _whatIsPlayer);
            if (coll == null)
                return null;
            Vector3 direction = coll.transform.position - _eye.transform.position;
            float distance = direction.magnitude;
            direction.Normalize();
            if (!Physics2D.Raycast(_eye.transform.position, direction, distance, _whatIsGround))
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (angle < 0)
                    angle += 360f;

                float facingAngle = _entityRenderer.FacingDirection == 1 ? 0 : 180;
                float minAngle = facingAngle - _fovElement.Value;
                float maxAngle = facingAngle + _fovElement.Value;

                if (minAngle <= angle && angle <= maxAngle)
                {
                    return coll.GetComponent<Player>();
                }
            }
            return null;
        }

        public override bool IsTargetOnAttackRange(Transform target)
        {
            float distanceSqr = (transform.position - target.position).sqrMagnitude;
            float attackRangeSqr = _attackRangeElement.Value * _attackRangeElement.Value;

            return distanceSqr < attackRangeSqr;
        }
    }
}
