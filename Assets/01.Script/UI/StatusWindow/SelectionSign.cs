using DG.Tweening;
using Hashira.SkillSystem;
using Hashira.UI.StatusWindow.SkillPanel.SkillSlots;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow.SkillPanel
{
    public class SelectionSign : MonoBehaviour
    {
        private SkillSlot _currentSelectedSkillSlot;
        
        [SerializeField] private Button _throwButton;
        [SerializeField] private Button _destroyButton;

        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _throwButton.onClick.AddListener(HandleThrowSkill);
            _destroyButton.onClick.AddListener(HandleDestroySkill);
        }

        public void SetCurrentSelectedSkillSlot(SkillSlot skillSlot)
        {
            _currentSelectedSkillSlot = skillSlot;
            _rectTransform.position = skillSlot.transform.position;
        }
        
        private void HandleThrowSkill()
        {
            Skill skill = _currentSelectedSkillSlot.GetBaseSkill();
            SkillManager.RemoveSkill(skill);
            
            // TODO 걍 버리기만(스킬 자체는 필드에서 언제든지 다시 회수할 수 있도록)
        }

        private void HandleDestroySkill()
        {
            Skill skill = _currentSelectedSkillSlot.GetBaseSkill();
            SkillManager.RemoveSkill(skill);
            
            // TODO 스킬 파괴(리턴)
        }
    }
}
