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

        private void Update()
        {
            for (int i = 0; i < PlayerBulletManager.Instance.GetModifierList.Count; i++)
                PlayerBulletManager.Instance.GetModifierList[i].OnUpdate(this);
        }

        protected override void OnHited(HitInfo hitInfo)
        {
            base.OnHited(hitInfo);
            for (int i = 0; i < PlayerBulletManager.Instance.GetModifierList.Count; i++)
                PlayerBulletManager.Instance.GetModifierList[i].OnProjectileHit(this, hitInfo);
        }

        public override void OnPop()
        {
            base.OnPop();
            for (int i = 0; i < PlayerBulletManager.Instance.GetModifierList.Count; i++)
                PlayerBulletManager.Instance.GetModifierList[i].OnProjectileCreate(this);
        }
    }
}
