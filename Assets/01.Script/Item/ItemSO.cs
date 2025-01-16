using UnityEngine;

namespace Hashira.Weapons
{
    public abstract class ItemSO : ScriptableObject
    {
        [Header("Item setting")]
        public Sprite itemIcon;
        public Sprite itemSprite;
        [Tooltip("영어명")]
        public string itemName;
        [Tooltip("한글명")]
        public string itemDisplayName;
        [TextArea]
        public string itemDescription;
    }
}
