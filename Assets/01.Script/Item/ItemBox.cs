using Hashira.Entities;
using Hashira.Entities.Interacts;
using Hashira.Items;
using UnityEngine;
using Crogen.CrogenPooling;
using Hashira.Items.Weapons;
using Hashira.Items.PartsSystem;
using Doryu.CustomAttributes;
using System.Collections.Generic;

namespace Hashira
{
    public class ItemBox : KeyInteractObject
    {
        [SerializeField] private bool _isRandomItem;
        [SerializeField, ToggleField("_isRandomItem", false)] private ItemSO _item;
        [SerializeField, ToggleField("_isRandomItem", true)] private ItemGroupSO _itemGroup;
        [SerializeField] private string objectName;

        protected override void Awake()
        {
            base.Awake();
            Initialized();
        }

        public void Initialized()
        {
            _nameText.text = objectName;
        }

        public override void Interaction(Entity entity)
        {
            base.Interaction(entity);

            ItemSO itemSO = _isRandomItem ? _itemGroup[Random.Range(0, _itemGroup.Length)] : _item;
            Item item = itemSO.GetItemClass();
            DroppedItem droppedItem = ItemDropUtility.DroppedItem(item, transform.position);
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
