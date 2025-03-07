using Hashira.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Hashira.LatestUI
{
    public class CardUsingUI : UIBase, IToggleUI
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private UseableCardDrawer _useableCardDrower;
        [field: SerializeField] public string Key { get; set; }


        [SerializeField] private Button _rerollBtn, _stageBtn;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            _rerollBtn.onClick.AddListener(_useableCardDrower.Reroll);
            _stageBtn.onClick.AddListener(() =>
            {
                //Stage 시작
                GameManager.Instance.StartStage();
                Debug.Log("시작");
                Close();
            });
            Cost.OnCostChangedEvent += CostTextUpdate;
            CostTextUpdate(Cost.CurrentCost);

            Close();
        }

        private void CostTextUpdate(int cost)
        {
            _textMeshProUGUI.text = $"{cost}";
        }

        private void Update()
        {
            //Debug
            //if (Keyboard.current.iKey.wasPressedThisFrame)
            //{
            //    Open();
            //}
        }

        public void Close()
        {
            _canvasGroup.alpha = 0;
        }

        public void Open()
        {
            _canvasGroup.alpha = 1;
            _useableCardDrower.CardDraw();
        }
    }
}
