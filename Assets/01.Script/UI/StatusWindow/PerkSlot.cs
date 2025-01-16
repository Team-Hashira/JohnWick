using Hashira.PerkSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow.PerkPanel.SkillSlots
{
    public class PerkSlot : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        private PerkPanel _perkPanel;
        [HideInInspector] public Button button;
        public Perk GetBaseSkill() => _basePerk;
        private Perk _basePerk;
        public int SlotIndex { get; set; }
        
        private void Awake()
        {
            _perkPanel = GetComponentInParent<PerkPanel>();
            button = GetComponent<Button>();
            button.onClick.AddListener(HandleSelectSkill);
        }

        private void Start()
        {
            PerkManager.OnAddedNewPerkEvent += HandleInit;
        }

        private void HandleSelectSkill()
        {
            _perkPanel.selectionSign.SetCurrentSelectedPerkSlot(this);
            _perkPanel.currentSelectionIndex = SlotIndex;
        }
        
        private void HandleInit(Perk perk)
        {
            _basePerk = perk;
            _iconImage.sprite = perk.perkUIDataSo.icon;
        }
    }
}
