using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items;
using Hashira.Items.Weapons;
using Hashira.UI.InGame;
using System;
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
        private Material _holdOutlineMat;

        protected Entity _entity;
        protected EntityGunWeapon _entityWeapon;

        protected override void Awake()
        {
            base.Awake();
            _ItemDataTrm = _itemData.transform.parent;
            _ComparisonItemDataTrm = _comparisonItemData.transform.parent;
            _holdOutlineMat = _holdOutlineSprite.material;
            _holdOutlineMat.SetFloat(_FillAmountShaderHash, 0);
            _ItemDataTrm.gameObject.SetActive(false);
            _ComparisonItemDataTrm.gameObject.SetActive(false);
        }

        public void HoldInteractionStart(Entity entity)
        {
            _entity = entity;
            _entityWeapon = _entity.GetEntityComponent<EntityGunWeapon>();
            _entityWeapon.OnCurrentWeaponChanged += HandleCurrentWeaponChangedEvent;
            _isHolding = true;
            
            _holdStartTime = Time.time;
        }

        private void HandleCurrentWeaponChangedEvent(Weapon weapon)
        {
            SetItemData();
        }

        public void HoldInteractionEnd()
        {
            _entityWeapon.OnCurrentWeaponChanged -= HandleCurrentWeaponChangedEvent;
            _entity = null;
            _isHolding = false;
            _ItemDataTrm.gameObject.SetActive(false);
            _ComparisonItemDataTrm.gameObject.SetActive(false);
            _holdOutlineMat.SetFloat(_FillAmountShaderHash, 0);
        }

        public void SetItem(ItemSO itemSO)
        {
            Item item = itemSO.GetItemClass();
            SetItem(item);
        }
        public virtual void SetItem(Item item)
        {
            _itemSprite.sprite = item.ItemSO?.itemDefaultSprite;
            string itemName = item.ItemSO == null ? "" : item.ItemSO.itemDisplayName;
            _nameText.text = $"{itemName}";
        }

        private void Update()
        {
            if (_isHolding)
            {
                float amount = (Time.time - _holdStartTime) / _descriptionOpenDelay;
                if (amount < 1)
                    _holdOutlineMat.SetFloat(_FillAmountShaderHash, amount);
                else if (_ItemDataTrm.gameObject.activeSelf == false)
                {
                    SetItemData();
                }
            }
        }

        public virtual void SetItemData() { }
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }
        public virtual void OnPop() {}

        public virtual void OnPush() {}
    }
}
