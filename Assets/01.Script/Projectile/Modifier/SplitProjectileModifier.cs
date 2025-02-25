using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class SplitProjectileModifier : ProjectileModifier
    {
        [SerializeField] private int _addProjectileCount;
        private bool _isRemovedBurstBullets;

        public override void OnProjectileCreateReady()
        {
            base.OnProjectileCreateReady();
            for (int i = 0; i < _addProjectileCount; i++)
                _attacker.AddBurstBullets();
            _isRemovedBurstBullets = false;
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);

            projectile.DamageOverride(Mathf.CeilToInt(projectile.Damage * 0.8f));

            if (_isRemovedBurstBullets == false)
            {
                _isRemovedBurstBullets = true;
                for (int i = 0; i < _addProjectileCount; i++)
                    _attacker.RemoveBurstBullets();

                ModifierExecuter.Reset();
            }
        }
    }
}
