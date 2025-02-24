using Hashira.Entities;
using Hashira.Entities.Interacts;
using Hashira.Items;
using UnityEngine;
using Doryu.CustomAttributes;
using Crogen.CrogenPooling;
using Hashira.Players;

namespace Hashira
{
    public class ItemBox : KeyInteractObject
    {
        [SerializeField] private bool _isRandomItem;
        [SerializeField, ToggleField("_isRandomItem", false)] private ItemSO _item;
        [SerializeField, ToggleField("_isRandomItem", true)] private ItemGroupSO _itemGroup;
        [SerializeField] private string objectName;

        [Space(25)]
        [SerializeField] private Sprite _openedSprite;
        [SerializeField] private int _itemCount = 1;
        [SerializeField] private int _getableCount = 1;
        [SerializeField] private float _gap = 1.0f;
        [SerializeField] private float _jump = 5f;
        private int _currentGetableCount = 0;
        private DroppedItem[] _droppedItems;

        private SpriteRenderer _spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            Initialized();

			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

        public void Initialized()
        {
            _nameText.text = objectName;
        }

        public override void Interaction(Player player)
        {
            if (CanInteraction == false) return;
            base.Interaction(player);

            _droppedItems = new DroppedItem[_itemCount]; 

			for (int i = 0; i < _itemCount; i++)
            {
				ItemSO itemSO = _isRandomItem ? _itemGroup[Random.Range(0, _itemGroup.Length)] : _item;
				Item item = itemSO.GetItemClass();
				DroppedItem droppedItem = ItemDropUtility.DroppedItem(item, transform.position);

                float percent = (i + 0.5f) / _itemCount;

                float min = -_gap * _itemCount * 0.5f, max = _gap * _itemCount * 0.5f;

				Vector2 velocity = Vector2.up * _jump + Vector2.right * Mathf.Lerp(min, max, percent);

				droppedItem.Rigidbody2D.AddForce(velocity, ForceMode2D.Impulse);

				droppedItem.OnInteractionSuccesEvent += HandleSelectComplate;
                _droppedItems[i] = droppedItem;
			}

            CanInteraction = false;
            _spriteRenderer.sprite = _openedSprite;
		}

        private void HandleSelectComplate()
        {
            ++_currentGetableCount;

            if (_currentGetableCount >= _getableCount)
            {
                for (int i = 0; i < _droppedItems.Length; i++)
                {
                    if (_droppedItems[i] == null) continue;
                    else if (_droppedItems[i].gameObject.activeSelf == false)
                    {
                        //_droppedItems[i].OnInteractionSuccesEvent -= HandleSelectComplate;
                        _droppedItems[i] = null;
                        continue;
                    }
                    //_droppedItems[i].Push();
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
