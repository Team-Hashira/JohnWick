using UnityEngine;

namespace Hashira.Weapons
{
    public abstract class ItemSO : ScriptableObject
    {
        [Header("Item setting")]
        public Sprite itemIcon;
        public Sprite itemSprite;
        [Tooltip("�����")]
        public string itemName;
        [Tooltip("�ѱ۸�")]
        public string itemDisplayName;
        [TextArea]
        public string itemDescription;
    }
}
