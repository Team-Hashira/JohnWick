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

        public override void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            base.OnProjectileHit(hit, damageable);
            float damage;
            if (_withProjectileDamage)
            {
                if (_isPercent)
                    damage = _projectile.Damage * (_damageValue / 100);
                else
                    damage = _projectile.Damage + _damageValue;
            }
            else
                damage = _damageValue;
            damageable?.ApplyDamage(Mathf.CeilToInt(damage), hit, _projectile.transform, attackType: _attackType);
        }
    }
}
