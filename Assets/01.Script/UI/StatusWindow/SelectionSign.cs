using DG.Tweening;
using Hashira.PerkSystem;
using Hashira.UI.StatusWindow.PerkPanel.SkillSlots;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow.PerkPanel
{
    public class SelectionSign : MonoBehaviour
    {
        private PerkSlot _currentSelectedPerkSlot;
        
        [SerializeField] private Button _throwButton;
        [SerializeField] private Button _destroyButton;

        private RectTransform _rectTransform;
        
        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _throwButton.onClick.AddListener(HandleThrowSkill);
            _destroyButton.onClick.AddListener(HandleDestroySkill);
        }

        public void SetCurrentSelectedPerkSlot(PerkSlot perkSlot)
        {
            _currentSelectedPerkSlot = perkSlot;
            _rectTransform.position = perkSlot.transform.position;
        }
        
        private void HandleThrowSkill()
        {
            Perk perk = _currentSelectedPerkSlot.GetBaseSkill();
            PerkManager.RemovePerk(perk);
            
            // TODO 걍 버리기만(스킬 자체는 필드에서 언제든지 다시 회수할 수 있도록)
        }

        private void HandleDestroySkill()
        {
            Perk perk = _currentSelectedPerkSlot.GetBaseSkill();
            PerkManager.RemovePerk(perk);
            
            // TODO 스킬 파괴(리턴)
        }
    }
}
