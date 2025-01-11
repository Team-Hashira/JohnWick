using Hashira.UI.StatusWindow.SkillPanel.SkillSlots;
using UnityEngine;

namespace Hashira.UI.StatusWindow.SkillPanel
{
    public class SkillPanel : MonoBehaviour
    {
        [SerializeField] private SelectionSign _selectionSign;
        private SkillSlot[] _skillSlots;
        
        private void OnEnable()
        {
            _skillSlots = GetComponentsInChildren<SkillSlot>();
            _selectionSign.currentSelectedSkillSlot = _skillSlots[0];
        }
    }
}
