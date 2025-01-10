using DG.Tweening;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class StatusWindow : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private InputReaderSO _inputReaderSO;
        
        //Values
        private bool _isShowStatusWindow = false;
        private bool _isFading = false;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _isShowStatusWindow = false;
        }

        private void Start()
        {
            _inputReaderSO.OnStatusWindowEnableEvent += HandleStatusWindowEnable;
        }

        private void OnDestroy()
        {
            _inputReaderSO.OnStatusWindowEnableEvent -= HandleStatusWindowEnable;
        }

        private void HandleStatusWindowEnable()
        {
            _isShowStatusWindow = !_isShowStatusWindow;

            //페이드 중일 때는 어떤 것도 콜하지 않음
            if (_isFading == true) return;
            _isFading = true;
            
            if (_isShowStatusWindow == true) Show();
            else Hide();
        }

        ///보여주기
        private void Show()
        {
            _inputReaderSO.PlayerActions.Disable();
            _canvasGroup.DOFade(1, 0.5f)
                .SetUpdate(true).OnComplete(()=>_isFading = false);
            _canvasGroup.interactable = true;
        }
        
        ///숨기기
        private void Hide()
        {
            _inputReaderSO.PlayerActions.Enable();
            _canvasGroup.DOFade(0, 0.5f)
                .SetUpdate(true).OnComplete(()=>_isFading = false);
            _canvasGroup.interactable = false;
        }
    }
}
