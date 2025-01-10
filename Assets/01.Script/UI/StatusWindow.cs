using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class StatusWindow : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private InputReaderSO _inputReaderSO;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            //보여주기
        }

        public void Hide()
        {
            //숨기기
        }
    }
}
