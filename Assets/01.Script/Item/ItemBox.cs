using Hashira.Entities;
using Hashira.Entities.Interacts;
using Hashira.Items;
using UnityEngine;
using Doryu.CustomAttributes;

namespace Hashira
{
    public class ItemBox : KeyInteractObject
    {
        [SerializeField] private bool _isRandomItem;
        [SerializeField, ToggleField("_isRandomItem", false)] private ItemSO _item;
        [SerializeField, ToggleField("_isRandomItem", true)] private ItemGroupSO _itemGroup;
        [SerializeField] private string objectName;

        [SerializeField] private int _itemCount = 1;
        [SerializeField] private int _getableCount = 1;
        [SerializeField] private float _gap = 1.0f;
        [SerializeField] private float _jump = 5f;
        private int _currentGetableCount = 0;
        private DroppedItem[] _droppedItems;


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
            if (CanInteraction == false) return;
            base.Interaction(entity);

            _droppedItems = new DroppedItem[_itemCount]; 

			for (int i = 0; i < _itemCount; i++)
            {
				ItemSO itemSO = _isRandomItem ? _itemGroup[Random.Range(0, _itemGroup.Length)] : _item;
				Item item = itemSO.GetItemClass();
				DroppedItem droppedItem = ItemDropUtility.DroppedItem(item, transform.position);

                float percent = (float)i / (_itemCount - 1);

                float min = -_gap * _itemCount * 0.5f, max = _gap * _itemCount * 0.5f;

				Vector2 velocity = Vector2.up * _jump + Vector2.right * Mathf.Lerp(min, max, percent);

				droppedItem.Rigidbody2D.AddForce(velocity, ForceMode2D.Impulse);

				droppedItem.OnInteractionEvent += HandleSelectComplate;
                _droppedItems[i] = droppedItem;
			}

            CanInteraction = false;
		}

        private void HandleSelectComplate()
        {
            ++_currentGetableCount;

            if (_currentGetableCount >= _getableCount)
            {
                for (int i = 0; i < _droppedItems.Length; i++)
                {
                    if (_droppedItems[i] == false) continue;

                    Destroy(_droppedItems[i].gameObject);
				}

                Destroy(gameObject);
			}
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
