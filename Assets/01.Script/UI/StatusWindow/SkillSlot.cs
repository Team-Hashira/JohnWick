using Hashira.SkillSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow.SkillPanel.SkillSlots
{
    public class SkillSlot : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        private SkillPanel _skillPanel;
        public Button button;
        public Skill GetBaseSkill() => _baseSkill;
        
        private Skill _baseSkill;
        public int SlotIndex { get; set; }
        private void Awake()
        {
            _skillPanel = GetComponentInParent<SkillPanel>();
            button = GetComponent<Button>();
            button.onClick.AddListener(HandleSelectSkill);
        }

        private void HandleSelectSkill()
        {
            _skillPanel.selectionSign.SetCurrentSelectedSkillSlot(this);
            _skillPanel.currentSelectionIndex = SlotIndex;
        }
        
        public void Init(Skill skill)
        {
            _baseSkill = skill;
            _iconImage.sprite = skill.skillUIDataSO.icon;
        }
    }
}
