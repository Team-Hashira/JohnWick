using Doryu.CustomAttributes;
using Hashira.Entities;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class BonusDamageProjectileModifier : ProjectileModifier
    {
        [SerializeField] private float _damageValue;
        [SerializeField] private bool _withProjectileDamage;
        [SerializeField] private bool _isPercent;
        [SerializeField] private EAttackType _attackType;

        public override void OnProjectileHit(Projectile projectile, HitInfo hitInfo)
        {
            base.OnProjectileHit(projectile, hitInfo);
            float damage;
            if (_withProjectileDamage)
            {
                if (_isPercent)
                    damage = projectile.Damage * (_damageValue / 100);
                else
                    damage = projectile.Damage + _damageValue;
            }
            else
                damage = _damageValue;
            hitInfo.damageable?.ApplyDamage(Mathf.CeilToInt(damage),hitInfo.raycastHit, projectile.transform, attackType: _attackType);
        }
    }
}
