using Hashira.UI.StatusWindow.PerkPanel.SkillSlots;
using UnityEngine;

namespace Hashira.UI.StatusWindow.PerkPanel
{
    public class PerkPanel : MonoBehaviour
    {
        [SerializeField] private InputReaderSO _inputReaderSO;
        public SelectionSign selectionSign;
        private PerkSlot[] _perkSlots;
        public int currentSelectionIndex;
        private Vector2Int _perkSlotSize = new Vector2Int(4, 4);        
        
        private void Awake()
        {
            _perkSlots = GetComponentsInChildren<PerkSlot>();
            for (int i = 0; i < _perkSlots.Length; i++)
            {
                _perkSlots[i].SlotIndex = i;
            }
            
            selectionSign.SetCurrentSelectedPerkSlot(_perkSlots[currentSelectionIndex]);
            _inputReaderSO.OnNavigateEvent += HandlePerkSelectMove;
        }

        private void OnDestroy()
        {
            _inputReaderSO.OnNavigateEvent -= HandlePerkSelectMove;
        }

        private void HandlePerkSelectMove(Vector2 dir)
        {
            int res = currentSelectionIndex - (int)dir.y * _perkSlotSize.y;
            if (res >= 0 && res < _perkSlots.Length)
                currentSelectionIndex = res;
            currentSelectionIndex += (int)dir.x;
            currentSelectionIndex = Mathf.Clamp(currentSelectionIndex, 0, _perkSlots.Length-1);

            selectionSign.SetCurrentSelectedPerkSlot(_perkSlots[currentSelectionIndex]);
        }
    }
}
