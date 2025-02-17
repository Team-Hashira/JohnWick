using Hashira.Items.Weapons;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class FlashSuppressorParts : WeaponParts
    {
        public override object Clone()
        {
            return base.Clone();
        }

        public override void Equip(GunWeapon weapon)
        {
            base.Equip(weapon);
        }

        public override void UnEquip()
        {
            base.UnEquip();
        }
    }
}
