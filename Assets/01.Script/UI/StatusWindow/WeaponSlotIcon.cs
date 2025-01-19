using System;
using Hashira.Entities.Components;
using Hashira.Entities.Interacts;
using Hashira.Players;
using Hashira.UI.DragSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class WeaponSlotIcon : MonoBehaviour, IDraggableObject
    {
        [SerializeField] private DroppedWeapon _droppedWeaponPrefab;
        public bool CanDrag => Parent.BaseWeapon != null;
        public Vector2 DragStartPosition { get; set; }
        public Vector2 DragEndPosition { get; set; }
        
        public RectTransform RectTransform { get; set; }
        public IWeaponSlot Parent { get; private set; }
        [SerializeField] private Image _image;

        private EntityWeapon _entityWeapon;

        private Player _player;
        
        private void Awake()
        {
            _image.color = Color.clear;  
            _player = GameManager.Instance.Player;
            RectTransform = transform as RectTransform;
        }

        private void Start()
        {
            _entityWeapon = _player.GetEntityComponent<EntityWeapon>();
        }

        public void Init(IWeaponSlot gunWeaponSlot)
        {
            _image.sprite = gunWeaponSlot.BaseWeapon?.WeaponSO.itemSprite;
            _image.color = gunWeaponSlot.BaseWeapon != null ? Color.white : Color.clear;  
            Parent = gunWeaponSlot;
        }
        
        public void OnDragStart()
        {
            (Parent as MonoBehaviour)?.transform.SetAsLastSibling();
            (Parent as MonoBehaviour)?.transform.parent.SetAsLastSibling();
        }

        public void OnDragging(Vector2 curPos)
        {
            var raycastResult = DragController.GetUIUnderCursor();
            if(raycastResult.Count > 1)
                _image.color = raycastResult[1].gameObject.name.Equals("BlackSolid") ? Color.red : Color.white;
            transform.eulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * 25f) * 10f);
        }

        public void OnDragEnd(Vector2 curPos)
        {
            if (Parent.BaseWeapon == null)
            {
                SetToOriginTrm();
                return;
            }

            var raycastResult = DragController.GetUIUnderCursor();
            if (raycastResult[1].gameObject.name.Equals("BlackSolid"))
            {
                // TODO 이거 나중에 풀링 꼭!!!! 하기 
                var droppedWeapon = Instantiate(_droppedWeaponPrefab, _player.transform.position, Quaternion.identity);
                droppedWeapon.SetItem(Parent.BaseWeapon);
                _entityWeapon.RemoveWeapon(Parent.SlotIndex);
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