using DG.Tweening;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class StatusWindow : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private InputReaderSO _inputReader;
        
        //Values
        private bool _isShowStatusWindow => gameObject.activeSelf;
        private bool _isFading = false;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _inputReader.UIActions.Enable();
            _canvasGroup.interactable = false;
        }

        private void Start()
        {
            _inputReader.OnStatusWindowEnableEvent += HandleStatusWindowEnable;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _inputReader.OnStatusWindowEnableEvent -= HandleStatusWindowEnable;
        }

        private void HandleStatusWindowEnable()
        {
            //페이드 중일 때는 어떤 것도 콜하지 않음
            if (_isFading == true) return;
            _isFading = true;
            
            if (_isShowStatusWindow == false) Show();
            else Hide();
        }

        ///보여주기
        private void Show()
        {
            gameObject.SetActive(true);
            _inputReader.PlayerActions.Disable();
            _canvasGroup.DOFade(1, 0.5f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _isFading = false;
                    _canvasGroup.interactable = true;
                });
        }
        
        ///숨기기
        private void Hide()
        {
            _inputReader.PlayerActions.Enable();
            _canvasGroup.interactable = false;
            _canvasGroup.DOFade(0, 0.5f)
                .SetUpdate(true)
                .OnComplete(()=>
                {
                    gameObject.SetActive(false);
                    _isFading = false;
                });
        }
    }
}
