using Hashira.Entities.Interacts;
using Hashira.Items.WeaponPartsSystem;
using Hashira.UI.DragSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class PartSlotIcon : MonoBehaviour, IDraggableObject
    {
        [SerializeField] private DroppedParts _droppedParts;
        public Vector2 DragStartPosition { get; set; }
        public Vector2 DragEndPosition { get; set; }
        
        public RectTransform RectTransform { get; set; }
        public PartSlot Parent { get; private set; }
        [SerializeField] private Image _image;
        
        private void Awake()
        {
            RectTransform = transform as RectTransform;
        }

        public void Init(PartSlot partSlot, WeaponParts weaponPart)
        {
            _image.sprite = weaponPart?.WeaponPartsSO.itemSprite;
            _image.color = weaponPart != null ? Color.white : Color.clear;  
            Debug.Log(weaponPart?.WeaponPartsSO.name);
            Parent = partSlot;
        }
        
        public void OnDragStart()
        {
            Parent.transform.SetAsLastSibling();
            Parent.transform.parent.SetAsLastSibling();
        }

        public void OnDragging(Vector2 curPos)
        {
            transform.eulerAngles = new Vector3(0f, Mathf.Sin(Time.time * 10), 0f);
        }

        public void OnDragEnd(Vector2 curPos)
        {
            var raycastResult = DragController.GetUIUnderCursor();
            for (int i = 0; i < raycastResult.Count; i++)
            {
                Debug.Log(raycastResult[i].gameObject.name);
            }
            
            foreach (var result in raycastResult)
            {
                if (result.gameObject.name.Equals("BlackSolid"))
                {
                    // TODO 이거 나중에 풀링 꼭!!!! 하기 
                    var droppedItem = Instantiate(_droppedParts, GameManager.Instance.Player.transform.position, Quaternion.identity);
                    droppedItem.SetParts(Parent.BasePart);
                    Parent.Parent.baseWeapon.EquipParts(Parent.partType, null);
                    break;
                } 
            }
                
            foreach (var result in raycastResult)
            {
                if (!result.gameObject.TryGetComponent(out PartSlot slot)) continue;
                if (Parent.Parent.baseWeapon == null) break;
                // 같은 종류가 아니면 교체 안됨
                if (slot.partType != Parent.partType) break;
                
                var temp = slot.BasePart;
                
                // 데이터의 교체
                slot.Parent.baseWeapon?.EquipParts(slot.partType, Parent.BasePart);
                Parent.Parent.baseWeapon.EquipParts(Parent.partType, temp);
                
            }
            
            // 원위치
            RectTransform.anchoredPosition = Vector2.zero;
            transform.eulerAngles = Vector3.zero;
        }
    }
}
