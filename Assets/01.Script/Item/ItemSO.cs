using UnityEngine;

namespace Hashira.Weapons
{
    public abstract class ItemSO : ScriptableObject
    {
        public Sprite itemSprite;
        public string itemName;
        [TextArea]
        public string itemDescription;
        public int maxOverlapCount;
    }
}
