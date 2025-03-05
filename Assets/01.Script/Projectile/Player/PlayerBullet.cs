using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles.Player
{
    public class PlayerBullet : Bullet
    {
        PlayerBulletManager _playerBulletManager;

        public override int CalculateDamage(float damage)
        {
            return base.CalculateDamage(damage);
        }

        public override void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner, List<ProjectileModifier> projectileModifiers = null, AnimationCurve damageOverDistance = null)
        {
            base.Init(whatIsTarget, direction, speed, damage, penetration, owner, projectileModifiers, damageOverDistance);
            _playerBulletManager = PlayerBulletManager.Instance;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void OnHited(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnHited(hit, damageable);
        }
    }
}
