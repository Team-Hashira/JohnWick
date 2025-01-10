using Hashira.SkillSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow.SkillSlots
{
    public class SkillSlot : MonoBehaviour
    {
        private Skill _baseSkill;
        [SerializeField] private Image _iconImage;
        
        public void Init(Skill skill)
        {
            _baseSkill = skill;
            _iconImage.sprite = skill.skillUIDataSO.icon;
        }
    }
}
