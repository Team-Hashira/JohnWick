using System;
using UnityEngine;

namespace Hashira.Projectiles
{
    [Serializable]
    public class SplitProjectileModifier : ProjectileModifier
    {
        [SerializeField] private int addProjectileCount;
        [SerializeField] private float cooldown;

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            for (int i = 0; i < addProjectileCount; i++)
                _attacker.AddBurstBullets();


        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
            for (int i = 0; i < addProjectileCount; i++)
                _attacker.RemoveBurstBullets();
        }
    }
}
