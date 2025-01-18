using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hashira.UI.DragSystem
{
    public class DragController : MonoBehaviour
    {
        public Canvas canvas;
        [SerializeField] private InputReaderSO _inputReader;
        private static GraphicRaycaster _graphicRaycaster; // UI가 포함된 Canvas에 연결된 GraphicRaycaster
        private static EventSystem _eventSystem;          // EventSystem 오브젝트
        private static Vector2 MousePosition { get; set; } 
        private IDraggableObject _currentDragObject;
        
        private void Awake()
        {
            _graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            _eventSystem = EventSystem.current;

            _inputReader.OnClickEvent += HandleMouseClick;
        }

        private void OnDestroy()
        {
            _inputReader.OnClickEvent -= HandleMouseClick;
        }

        private void HandleMouseClick(bool isMouseDown)
        {
            if (isMouseDown)
            {
                var rayCastResult = GetUIUnderCursor();
                if (rayCastResult.Count == 0 || rayCastResult[0].gameObject == null) return;
                if (!rayCastResult[0].gameObject.TryGetComponent(out IDraggableObject draggableObject)) return;
                _currentDragObject = draggableObject;
                _currentDragObject.DragStartPosition = _currentDragObject.RectTransform.position;
                _currentDragObject?.OnDragStart();
            }
            else
            {
                if(_currentDragObject == null) return;
                _currentDragObject.DragEndPosition = _currentDragObject.RectTransform.position;
                _currentDragObject?.OnDragEnd(MousePosition);
                _currentDragObject = null;
            }
        }

        public static List<RaycastResult> GetUIUnderCursor()
        {
            PointerEventData pointerEventData = new PointerEventData(_eventSystem)
            {
                position = MousePosition // 마우스 위치 설정
            };

            List<RaycastResult> results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(pointerEventData, results);

            return results;
        }

        private void Update()
        {
            // 마우스 위치 
            MousePosition = _inputReader.MousePosition;
            
            if (_currentDragObject == null) return;
            Debug.Log(_currentDragObject.RectTransform.gameObject.name);
            _currentDragObject.RectTransform.position = MousePosition;
            _currentDragObject.OnDragging(MousePosition);
        }
    }
}