using Hashira.Entities;
using Hashira.Entities.Interacts;
using Hashira.Items;
using UnityEngine;
using Crogen.CrogenPooling;
using Hashira.Items.Weapons;
using Hashira.Items.PartsSystem;
using Doryu.CustomAttributes;

namespace Hashira
{
    public class ItemBox : KeyInteractObject
    {
        [SerializeField] private bool _isRandomItem;
        [SerializeField, ToggleField("_isRandomItem", false)] private ItemSO _item;
        [SerializeField, ToggleField("_isRandomItem", true)] private ItemGroupSO _itemGroup;

        protected override void Awake()
        {
            base.Awake();
            Initialized();
        }

        public void Initialized()
        {
            _nameText.text = "쓰레기통";
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            ItemSO itemSO = _isRandomItem ? _itemGroup[Random.Range(0, _itemGroup.Length)] : _item;
            Item item = itemSO.GetItemClass();

            DroppedItem droppedItem = null;
            if (item is Weapon weapon)
            {
                droppedItem = gameObject.Pop(ItemPoolType.WeaponItem, transform.position, Quaternion.identity) as DroppedWeapon;
                droppedItem.SetItem(weapon);
            }
            else if (item is WeaponParts weaponParts)
            {
                droppedItem = gameObject.Pop(ItemPoolType.WeaponPartsItem, transform.position, Quaternion.identity) as DroppedParts;
                droppedItem.SetItem(weaponParts);
            }
            Vector2 velocity = Random.insideUnitCircle + Vector2.up * 2;
            droppedItem.Rigidbody2D.AddForce(velocity.normalized * 5, ForceMode2D.Impulse);

            Destroy(gameObject);
        }

        public override void OffInteractable()
        {
            base.OffInteractable();
        }

        public override void OnInteractable()
        {
            base.OnInteractable();
        }
    }
}
