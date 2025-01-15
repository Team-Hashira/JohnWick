using Hashira.Core.StatSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Weapons
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/Weapon")]
    public class WeaponSO : ItemSO
    {
        [Header("Weapon setting")]
        public EWeaponPartsType equippableWeaponPartsType;
        public List<StatElement> statElementLsit = new List<StatElement>();
    }
}
