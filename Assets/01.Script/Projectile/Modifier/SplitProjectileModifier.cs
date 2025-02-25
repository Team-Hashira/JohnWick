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
            Debug.Log("추가");
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            if (_isRemovedBurstBullets == false)
            {
                _isRemovedBurstBullets = true;
                for (int i = 0; i < _addProjectileCount; i++)
                    _attacker.RemoveBurstBullets();
            }
        }
    }
}
