using Hashira.Entities.Interacts;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.UI.DragSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class PartSlotIcon : MonoBehaviour, IDraggableObject
    {
        [SerializeField] private DroppedParts _droppedPartsPrefab;
        public bool CanDrag => PartSlot.Item != null;
        public Vector2 DragStartPosition { get; set; }
        public Vector2 DragEndPosition { get; set; }
        
        public RectTransform RectTransform { get; set; }
        public PartSlot PartSlot { get; private set; }
        [SerializeField] private Image _image;
        
        private void Awake()
        {
            RectTransform = transform as RectTransform;
        }

        private void OnDisable()
        {
            SetToOriginTrm();
            UIMouseController.Instance?.ResetDrag();
        }

        public void Init(PartSlot partSlot)
        {
            _image.sprite = partSlot.Item?.ItemSO.itemDefaultSprite;
            _image.color = partSlot.Item != null ? Color.white : Color.clear;  
            PartSlot = partSlot;
        }

        public void OnDragStart()
        {
            PartSlot.transform.SetAsLastSibling();
            PartSlot.transform.parent.SetAsLastSibling();
        }

        public void OnDragging(Vector2 curPos)
        {
            var raycastResult = DragSystem.UIMouseController.GetUIUnderCursor();
            if(raycastResult.Count > 1)
                _image.color = raycastResult[1].gameObject.name.Equals("BlackSolid") ? Color.red : Color.white;
            transform.eulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * 25) * 10f);
        }

        public void OnDragEnd(Vector2 curPos)
        {
            // 없으면 움직이면 안됨
            if (PartSlot.Item == null)
            {
                SetToOriginTrm();
                return;
            }
            
            var raycastResult = DragSystem.UIMouseController.GetUIUnderCursor();
            if (raycastResult[1].gameObject.name.Equals("BlackSolid"))
            {
                Vector2 pos = GameManager.Instance.Player.transform.position;
                ItemDropUtility.DroppedItem(PartSlot.Item, pos);
                PartSlot.EquipParts(PartSlot.partType, null);
                SetToOriginTrm();
                return;
            } 
                
            foreach (var result in raycastResult)
            {
                // PartSlot에 놓지 않으면 건너뛰기
                if (result.gameObject.TryGetComponent(out PartSlot slot) == false) continue;
                if (PartSlot.isAllType == false && PartSlot.WeaponSlot.GunWeapon == null) break;

                // 모든 타입을 받을 수 없으면
                if(slot.isAllType == false)
                {
                    // 같은 종류가 아니면 교체 안됨
                    if (slot.partType != PartSlot.partType) break;
                }

                // 데이터 추출
                WeaponParts targetPart = slot.UnEquipParts(PartSlot.partType);
                WeaponParts basePart = PartSlot.UnEquipParts(PartSlot.partType);

                // 데이터의 교체
                slot.EquipParts(PartSlot.partType, basePart);
                PartSlot.EquipParts(PartSlot.partType, targetPart);
            }

            SetToOriginTrm();
        }


        private void SetToOriginTrm()
        {
            // 원위치
            RectTransform.anchoredPosition = Vector2.zero;
            transform.eulerAngles = Vector3.zero;
            Init(PartSlot);
        }
    }
}
