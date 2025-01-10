using Hashira.SkillSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow.SkillPanel.SkillSlots
{
    public class SkillSlot : MonoBehaviour
    {
        private Skill _baseSkill;
        [SerializeField] private Image _iconImage;
        
        public Skill GetBaseSkill() => _baseSkill;
        
        public void Init(Skill skill)
        {
            _baseSkill = skill;
            _iconImage.sprite = skill.skillUIDataSO.icon;
        }
    }
}
