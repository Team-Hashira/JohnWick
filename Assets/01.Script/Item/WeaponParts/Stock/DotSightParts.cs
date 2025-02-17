using Hashira.Items.Weapons;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class DotSightParts : WeaponParts
    {
        private DoubleAttackProjectileModifier _doubleAttackProjectileModifier;

        public override object Clone()
        {
            _doubleAttackProjectileModifier = new DoubleAttackProjectileModifier();
            _doubleAttackProjectileModifier.doubleAttackDamage = 100;
            return base.Clone();
        }

        public override void Equip(GunWeapon weapon)
        {
            base.Equip(weapon);
            _weapon.AddProjectileModifier(_doubleAttackProjectileModifier);
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _weapon.RemoveProjectileModifier(_doubleAttackProjectileModifier);
        }
    }
}
