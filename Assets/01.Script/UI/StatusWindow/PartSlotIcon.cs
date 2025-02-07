using Crogen.CrogenPooling;
using Hashira.Entities.Interacts;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.UI.DragSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class PartSlotIcon : MonoBehaviour, IDraggableObject
    {
        [SerializeField] private DroppedParts _droppedPartsPrefab;
        public bool CanDrag => Parent.BasePart != null;
        public Vector2 DragStartPosition { get; set; }
        public Vector2 DragEndPosition { get; set; }
        
        public RectTransform RectTransform { get; set; }
        public PartSlot Parent { get; private set; }
        [SerializeField] private Image _image;
        
        private void Awake()
        {
            RectTransform = transform as RectTransform;
        }

        public void Init(PartSlot partSlot)
        {
            _image.sprite = partSlot.BasePart?.WeaponPartsSO.itemDefaultSprite;
            _image.color = partSlot.BasePart != null ? Color.white : Color.clear;  
            Parent = partSlot;
        }
        
        public void OnDragStart()
        {
            Parent.transform.SetAsLastSibling();
            Parent.transform.parent.SetAsLastSibling();
        }

        public void OnDragging(Vector2 curPos)
        {
            var raycastResult = DragController.GetUIUnderCursor();
            if(raycastResult.Count > 1)
                _image.color = raycastResult[1].gameObject.name.Equals("BlackSolid") ? Color.red : Color.white;
            transform.eulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * 25) * 10f);
        }

        public void OnDragEnd(Vector2 curPos)
        {
            if (Parent.BasePart == null)
            {
                SetToOriginTrm();
                return;
            }
            
            var raycastResult = DragController.GetUIUnderCursor();
            if (raycastResult[1].gameObject.name.Equals("BlackSolid"))
            {
                Vector2 pos = GameManager.Instance.Player.transform.position;
                ItemDropUtility.DroppedItem(Parent.BasePart, pos);
                Parent.Parent.GunWeapon.EquipParts(Parent.partType, null);
                SetToOriginTrm();
                return;
            } 
                
            foreach (var result in raycastResult)
            {
                if (!result.gameObject.TryGetComponent(out PartSlot slot)) continue;
                if (Parent.Parent.GunWeapon == null) break;
                // 같은 종류가 아니면 교체 안됨
                if (slot.partType != Parent.partType) break;
                
                var temp = slot.BasePart;
                
                // 데이터의 교체
                slot.Parent.GunWeapon?.EquipParts(slot.partType, Parent.BasePart);
                Parent.Parent.GunWeapon.EquipParts(Parent.partType, temp);
                
            }

            SetToOriginTrm();
        }


        private void SetToOriginTrm()
        {
            // 원위치
            RectTransform.anchoredPosition = Vector2.zero;
            transform.eulerAngles = Vector3.zero;
            Init(Parent);
        }
    }
}
