using System;
using Hashira.Entities.Components;
using Hashira.Entities.Interacts;
using Hashira.Items;
using Hashira.Items.Weapons;
using Hashira.Players;
using Hashira.UI.DragSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class WeaponSlotIcon : MonoBehaviour, IDraggableObject
    {
        [SerializeField] private DroppedWeapon _droppedWeaponPrefab;
        public bool CanDrag => Parent.Item != null;
        public Vector2 DragStartPosition { get; set; }
        public Vector2 DragEndPosition { get; set; }
        
        public RectTransform RectTransform { get; set; }
        public ISlot Parent { get; private set; }
        [SerializeField] private Image _image;

        private EntityWeaponHolder _entityGunWeapon;
        private EntitySubItemHolder _entityMeleeWeapon;

        private Player _player;
        
        private void Awake()
        {
            _image.color = Color.clear;  
            _player = GameManager.Instance.Player;
            RectTransform = transform as RectTransform;
        }

        private void OnDisable()
        {
            SetToOriginTrm();
            UIMouseController.Instance?.ResetDrag();
        }

        private void Start()
        {
            _entityGunWeapon = _player.GetEntityComponent<EntityWeaponHolder>();
            _entityMeleeWeapon = _player.GetEntityComponent<EntitySubItemHolder>();
        }

        public void Init(ISlot itemSlot)
        {
            _image.sprite = (itemSlot.Item as GunWeapon)?.WeaponSO.itemIcon;
            _image.color = itemSlot.Item != null ? Color.white : Color.clear;  
            Parent = itemSlot;
        }
        
        public void OnDragStart()
        {
            (Parent as MonoBehaviour)?.transform.SetAsLastSibling();
            (Parent as MonoBehaviour)?.transform.parent.SetAsLastSibling();
        }

        public void OnDragging(Vector2 curPos)
        {
            var raycastResult = DragSystem.UIMouseController.GetUIUnderCursor();
            if(raycastResult.Count > 1)
                _image.color = raycastResult[1].gameObject.name.Equals("BlackSolid") ? Color.red : Color.white;
            transform.eulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * 25f) * 10f);
        }

        public void OnDragEnd(Vector2 curPos)
        {
            if (Parent.Item == null)
            {
                SetToOriginTrm();
                return;
            }

            var raycastResult = DragSystem.UIMouseController.GetUIUnderCursor();
            if (raycastResult[1].gameObject.name.Equals("BlackSolid"))
            {
                Vector2 pos = GameManager.Instance.Player.transform.position;
                Weapon dropWeapon = Parent.Item as Weapon;
                if (Parent is IWeaponSlot weaponSlot)
                    _entityGunWeapon.RemoveWeapon(weaponSlot.SlotIndex);
                else if (Parent is ISubItemSlot subItemSlot)
                    _entityMeleeWeapon.RemoveWeapon(subItemSlot.SlotIndex - _entityGunWeapon.Items.Length);
                ItemDropUtility.DroppedItem(dropWeapon, pos);
            } 
            
            SetToOriginTrm();
        }
        
        private void SetToOriginTrm()
        {
            // 원위치
            RectTransform.anchoredPosition = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
            Init(Parent);
        }
    }
}