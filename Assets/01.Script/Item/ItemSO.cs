using UnityEngine;

namespace Hashira.Weapons
{
    public abstract class ItemSO : ScriptableObject
    {
        [Header("Item setting")]
        public Sprite itemSprite;
        public string itemName;
        [TextArea]
        public string itemDescription;
    }
}
