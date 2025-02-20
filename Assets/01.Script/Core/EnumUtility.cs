using Hashira.Entities;
using Hashira.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core
{
    public static class EnumUtility
    {
        public const float CommonPixelPerUnit = 16f; //임시
        public static readonly Dictionary<EItemRank, Color> ItemRatingColorDict 
            = new Dictionary<EItemRank, Color>()
            {
                { EItemRank.Common, new Color(1, 1, 1, 1)},
                { EItemRank.UnCommon, new Color(0, 1, 0, 1)},
                { EItemRank.Rare, new Color(0, 0, 1, 1)},
                { EItemRank.Epic, new Color(1, 0, 1, 1)},
                { EItemRank.Legendary, new Color(1, 1, 0, 1)},
            };
        public static readonly Dictionary<EAttackType, Color> AttackTypeColorDict 
            = new Dictionary<EAttackType, Color>()
            {
                { EAttackType.Default, new Color(1, 1, 1, 1)},
                { EAttackType.HeadShot, new Color(1, 1, 0, 1)},
                { EAttackType.Fixed, new Color(0.5f, 0.5f, 0.5f, 1)},
                { EAttackType.Fire, new Color(1, 0, 0, 1)},
            };
        public static readonly Dictionary<EWeaponPartsType, string> WeaponPartsTypeNameDict
            = new Dictionary<EWeaponPartsType, string>()
            {
                { EWeaponPartsType.CartridgeBelt, "탄띠" },
                { EWeaponPartsType.Grip, "손잡이"},
                { EWeaponPartsType.Magazine, "탄창"},
                { EWeaponPartsType.Muzzle, "총구"},
                { EWeaponPartsType.Scope, "조준경"},
                { EWeaponPartsType.Stock, "개머리판"},
            };
    }
}
