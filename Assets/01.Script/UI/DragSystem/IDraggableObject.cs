using UnityEngine;

namespace Hashira.UI.DragSystem
{
    public interface IDraggableObject
    {
        public Vector2 DragPosition { get; set; }
        public RectTransform RectTransform { get; set; }
        public void OnDragStart();
        public void OnDragging(Vector2 curPos);
        public void OnDragEnd(Vector2 curPos);
    }
}