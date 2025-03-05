using Hashira.Entities;
using Hashira.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core
{
    public static class EnumUtility
    {
        public const float CommonPixelPerUnit = 16f; //임시
        public static readonly Dictionary<EAttackType, Color> AttackTypeColorDict 
            = new Dictionary<EAttackType, Color>()
            {
                { EAttackType.Default, new Color(1, 1, 1, 1)},
                { EAttackType.HeadShot, new Color(1, 1, 0, 1)},
                { EAttackType.Fixed, new Color(0.5f, 0.5f, 0.5f, 1)},
                { EAttackType.Fire, new Color(1, 0, 0, 1)},
                { EAttackType.Electricity, new Color(0.8f, 0, 0.8f, 1)},
            };
    }
}
