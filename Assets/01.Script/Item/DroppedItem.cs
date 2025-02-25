using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Entities.Interacts;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Items
{
    public class DroppedItem : KeyInteractObject, IPoolingObject
    {
        public Rigidbody2D Rigidbody2D { get; private set; }

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        protected Item _item;

        protected override void Awake()
        {
            base.Awake();
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public override void Interaction(Player player)
        {
            base.Interaction(player);
        }

        public override void InteractionSucces()
        {
            base.InteractionSucces();
        }

        public void SetItem(Item item)
        {
            _item = item;
            _nameText.text = item.ItemSO.itemName;
            _itemSprite.sprite = item.ItemSO.itemDefaultSprite;
        }

        public void OnPop()
        {
            _keyGuideObject.SetActive(false);
            _holdOutlineMat.SetFloat(_FillAmountShaderHash, 0);
        }

        public void OnPush()
        {

        }
    }
}
