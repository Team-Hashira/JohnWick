using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class SplitProjectileModifier : ProjectileModifier
    {
        private int _addProjectileCount;

        public void Init(int count)
        {
            _addProjectileCount = count;
        }

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            for (int i = 0; i < _addProjectileCount; i++)
                _attacker.AddBurstBullets();
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
            for (int i = 0; i < _addProjectileCount; i++)
                _attacker.RemoveBurstBullets();
        }
    }
}
