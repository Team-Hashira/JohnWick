using Hashira.Core;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Hashira.LatestUI
{
    public class CardUsingUI : UIBase, IToggleUI
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private UseableCardDrawer _useableCardDrower;
        [SerializeField] private Image _playerImage;
        [field: SerializeField] public string Key { get; set; }


        [SerializeField] private int _rerollCost = 10;
        [SerializeField] private Button _rerollBtn, _stageBtn;
        [SerializeField] private TextMeshProUGUI _rerollCostText, _currentCostText;

        [SerializeField] private Transform _backgroundTrm;
        [SerializeField] private GameObject _backgroundGlitch;
         
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            _rerollCostText.text = $"{_rerollCost}";

            _rerollBtn.onClick.AddListener(() => 
            { 
                if (Cost.TryRemoveCost(_rerollCost))
                    _useableCardDrower.CardDraw();
            });
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
            _currentCostText.text = $"{cost}";
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
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _backgroundTrm.gameObject.SetActive(false);
        }

        public void Open()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _backgroundTrm.gameObject.SetActive(true);
            _backgroundGlitch.SetActive(true);

            float defaultScaleY = _backgroundTrm.localScale.y;
            _backgroundTrm.localScale = new Vector3(_backgroundTrm.localScale.x, 0, _backgroundTrm.localScale.z);
            _backgroundTrm.DOKill();
            _backgroundTrm.DOScaleY(defaultScaleY, 0.35f).SetEase(Ease.OutExpo)
                .OnComplete(() => _backgroundGlitch.SetActive(false));

            Vector2 defaulCardDrawerPos = _useableCardDrower.RectTransform.anchoredPosition;
            _useableCardDrower.RectTransform.anchoredPosition = defaulCardDrawerPos + new Vector2(0, -100);
            _useableCardDrower.RectTransform.DOAnchorPos(defaulCardDrawerPos, 0.3f).SetEase(Ease.OutBack);
            Vector2 defaulPlayerImagePos = _playerImage.rectTransform.anchoredPosition;
            _playerImage.rectTransform.anchoredPosition = defaulPlayerImagePos + new Vector2(0, -500);
            _playerImage.rectTransform.DOAnchorPos(defaulPlayerImagePos, 0.2f).SetEase(Ease.OutBack);
            _useableCardDrower.CardDraw();
        }
    }
}
