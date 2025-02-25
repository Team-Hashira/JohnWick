using System;
using UnityEngine;

namespace Hashira.Projectiles
{
    [Serializable]
    public class SplitProjectileModifier : ProjectileModifier
    {
        [SerializeField] private int splitCount;

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            for (int i = 0; i < splitCount; i++)
                _attacker.AddBurstBullets();
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
            for (int i = 0; i < splitCount; i++)
                _attacker.RemoveBurstBullets();
        }
    }
}
