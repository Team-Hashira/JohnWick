using Hashira.Entities.Interacts;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Items
{
    public class DroppedItem : KeyInteractObject
    {
        public Rigidbody2D Rigidbody2D { get; private set; }
        private Item _item;

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
        }
    }
}
