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

        private void Awake()
        {
            _skillSlots = GetComponentsInChildren<SkillSlot>();
            selectionSign.SetCurrentSelectedSkillSlot(_skillSlots[0]);
            _inputReaderSO.OnNavigateEvent += HandleSkillSelectMove;
        }

        private void OnDestroy()
        {
            _inputReaderSO.OnNavigateEvent -= HandleSkillSelectMove;
        }

        private void HandleSkillSelectMove(Vector2 dir)
        {
            
        }

        private void Start()
        {
        }
    }
}
