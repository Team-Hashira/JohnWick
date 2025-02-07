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

        public override Player DetectPlayer()
        {
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 5, _whatIsPlayer);
            if (coll == null)
                return null;
            Vector3 direction = coll.transform.position - _eye.transform.position;
            float distance = direction.magnitude;
            direction.Normalize();
            if (!Physics2D.Raycast(_eye.transform.position, direction, distance, _whatIsGround))
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                float facingAngle = _entityRenderer.FacingDirection == 1 ? 0 : 180;
                float minAngle = facingAngle - 30;
                float maxAngle = -facingAngle + 30;

                if(minAngle <= angle && angle <= maxAngle)
                {
                    return coll.GetComponent<Player>();
                }
            }
            return null;
        }
    }
}
