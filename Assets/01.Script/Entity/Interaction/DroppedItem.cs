using Crogen.CrogenPooling;
using Hashira.Items;
using Hashira.UI.InGame;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public class DroppedItem : KeyInteractObject, IHoldInteractable, IPoolingObject
    {
        private readonly static int _FillAmountShaderHash = Shader.PropertyToID("_FillAmount");

        [Header("==========DroppedItem setting==========")]
        [SerializeField] protected SpriteRenderer _itemSprite;
        [SerializeField] protected SpriteRenderer _holdOutlineSprite;
        [SerializeField] protected ItemDataUI _itemData;
        [SerializeField] protected ItemDataUI _comparisonItemData;
        protected Transform _ItemDataTrm;
        protected Transform _ComparisonItemDataTrm;

        private float _descriptionOpenDelay = 0.5f;
        private float _holdStartTime = 0f;
        private bool _isHolding;

        protected Entity _entity;

        protected override void Awake()
        {
            base.Awake();
            _ItemDataTrm = _itemData.transform.parent;
            _ComparisonItemDataTrm = _comparisonItemData.transform.parent;
            _ItemDataTrm.gameObject.SetActive(false);
            _ComparisonItemDataTrm.gameObject.SetActive(false);
        }

        public void HoldInteractionStart(Entity entity)
        {
            _entity = entity;
            _isHolding = true;
            _holdStartTime = Time.time;
        }

        public void HoldInteractionEnd()
        {
            _entity = null;
            _isHolding = false;
            _ItemDataTrm.gameObject.SetActive(false);
            _ComparisonItemDataTrm.gameObject.SetActive(false);
            _holdOutlineSprite.material.SetFloat(_FillAmountShaderHash, 0);
        }

        public void SetItem(ItemSO itemSO)
        {
            Item item = itemSO.GetItemClass();
            SetItem(item);
        }
        public virtual void SetItem(Item item)
        {
            _itemSprite.sprite = item.ItemSO?.itemIcon;
            string itemName = item.ItemSO == null ? "" : item.ItemSO.itemDisplayName;
            _nameText.text = $"{itemName}";
        }

        private void Update()
        {
            if (_isHolding)
            {
                float amount = (Time.time - _holdStartTime) / _descriptionOpenDelay;
                if (amount < 1)
                    _holdOutlineSprite.material.SetFloat(_FillAmountShaderHash, amount);
                else if (_ItemDataTrm.gameObject.activeSelf == false)
                {
                    SetItemData();
                }
            }
        }

        public virtual void SetItemData() { }
        public string OriginPoolType { get; set; }
        public GameObject gameObject { get; set; }
        public virtual void OnPop() {}

        public virtual void OnPush() {}
    }
}
