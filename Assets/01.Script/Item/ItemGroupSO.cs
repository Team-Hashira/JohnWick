using Hashira.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    [CreateAssetMenu(fileName = "ItemGruop", menuName = "SO/Item/ItemGroup")]
    public class ItemGroupSO : ScriptableObject
    {
        public List<ItemSO> itemSOList;

        public ItemSO this[int index] { get => itemSOList[index]; }
        public int Length => itemSOList.Count;
    }
}
