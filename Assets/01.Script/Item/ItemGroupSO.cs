using Hashira.Items;
using UnityEngine;

namespace Hashira
{
    [CreateAssetMenu(fileName = "ItemGruop", menuName = "SO/Item/ItemGroup")]
    public class ItemGroupSO : ScriptableObject
    {
        public ItemSO[] itemSOs;

        public ItemSO this[int index] { get => itemSOs[index]; }
        public int Length => itemSOs.Length;
    }
}
