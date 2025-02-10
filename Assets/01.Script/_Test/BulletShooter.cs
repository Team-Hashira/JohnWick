using Crogen.CrogenPooling;
using Hashira.Projectile;
using UnityEngine;

namespace Hashira
{
    public class BulletShooter : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;

        private float _lastShootTime;

        private void Start()
        {
            _lastShootTime = Time.time;
        }

        private void Update()
        {
            if (_lastShootTime + _delay < Time.time)
            {
                _lastShootTime = Time.time;
                Bullet bullet = gameObject.Pop(ProjectilePoolType.Bullet, transform.position, transform.rotation) as Bullet;
                bullet.Init(_whatIsTarget, transform.right, _speed, _damage, 3, transform);
            }
        }
    }
}
