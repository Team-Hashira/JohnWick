using Hashira.Weapons;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedItem : KeyInteractObject
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        protected virtual void SetItemSO(ItemSO itemSO)
        {
            _spriteRenderer.sprite = itemSO?.itemIcon;
            string itemName = itemSO == null ? "" : itemSO.itemDisplayName;
            _nameText.text = $"{itemName}";
        }
    }
}
