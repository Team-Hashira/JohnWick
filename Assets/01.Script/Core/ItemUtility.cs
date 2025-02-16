using Hashira.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core
{
    public static class ItemUtility
    {
        public const float CommonPixelPerUnit = 16f;
        public static readonly Dictionary<EItemRating, Color> ItemRatingColorDict 
            = new Dictionary<EItemRating, Color>()
            {
                { EItemRating.Common, new Color(1, 1, 1, 1)},
                { EItemRating.UnCommon, new Color(0, 1, 0, 1)},
                { EItemRating.Rare, new Color(0, 0, 1, 1)},
                { EItemRating.Epic, new Color(1, 0, 1, 1)},
                { EItemRating.Legendary, new Color(1, 1, 0, 1)},
            };
    }
}
