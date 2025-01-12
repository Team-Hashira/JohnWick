using System;
using Hashira.UI.StatusWindow.SkillPanel.SkillSlots;
using UnityEngine;

namespace Hashira.UI.StatusWindow.SkillPanel
{
    public class SkillPanel : MonoBehaviour
    {
        [SerializeField] private InputReaderSO _inputReaderSO;
        public SelectionSign selectionSign;
        private SkillSlot[] _skillSlots;
        public int currentSelectionIndex;
        private Vector2Int _skillSlotSize = new Vector2Int(4, 4);        
        
        private void Awake()
        {
            _skillSlots = GetComponentsInChildren<SkillSlot>();
            for (int i = 0; i < _skillSlots.Length; i++)
            {
                _skillSlots[i].SlotIndex = i;
            }
            
            selectionSign.SetCurrentSelectedSkillSlot(_skillSlots[currentSelectionIndex]);
            _inputReaderSO.OnNavigateEvent += HandleSkillSelectMove;
        }

        private void OnDestroy()
        {
            _inputReaderSO.OnNavigateEvent -= HandleSkillSelectMove;
        }

        private void HandleSkillSelectMove(Vector2 dir)
        {
            int res = currentSelectionIndex - (int)dir.y * _skillSlotSize.y;
            if (res >= 0 && res < _skillSlots.Length)
                currentSelectionIndex = res;
            currentSelectionIndex += (int)dir.x;
            currentSelectionIndex = Mathf.Clamp(currentSelectionIndex, 0, _skillSlots.Length-1);

            selectionSign.SetCurrentSelectedSkillSlot(_skillSlots[currentSelectionIndex]);
        }

        private void Start()
        {
        }
    }
}
