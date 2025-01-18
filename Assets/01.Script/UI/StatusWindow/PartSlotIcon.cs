using Hashira.Items.WeaponPartsSystem;
using Hashira.UI.DragSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class PartSlotIcon : MonoBehaviour, IDraggableObject
    {
        public Vector2 DragStartPosition { get; set; }
        public Vector2 DragEndPosition { get; set; }
        
        public RectTransform RectTransform { get; set; }
        public PartSlot BaseSlot { get; private set; }
        private Image _image;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            RectTransform = transform as RectTransform;
            Init(null, null);
        }

        public void Init(PartSlot partSlot, WeaponParts weaponPart)
        {
            _image ??= GetComponent<Image>();
            _image.sprite = weaponPart?.WeaponPartsSO.itemSprite;
            _image.color = weaponPart != null ? Color.white : Color.clear;  
            BaseSlot = partSlot;
        }
        
        public void OnDragStart()
        {
        }

        public void OnDragging(Vector2 curPos)
        {
        }

        public void OnDragEnd(Vector2 curPos)
        {
            var raycastResult = DragController.GetUIUnderCursor();

            foreach (var result in raycastResult)
            {
                if (result.gameObject.name.Equals("BlackSold"))
                {
                    Debug.Log("응 이거 버리는 거얌^^");
                }

                if (result.gameObject.TryGetComponent(out PartSlot slot))
                {
                    var temp = slot.BasePart;

                    slot.Init(BaseSlot.BasePart);
                    BaseSlot.Init(temp);

                    RectTransform.position = DragStartPosition;
                }
            }
        }
    }
}
