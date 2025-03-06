using UnityEngine;
using UnityEngine.InputSystem;

namespace Hashira.LatestUI
{
    public class CardUsingUI : UIBase, IToggleUI
    {
        private CanvasGroup _canvasGroup;
        [field: SerializeField] public string Key { get; set; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            Close();
        }

        private void Update()
        {
            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                Open();
                //CardDraw();
            }
        }

        public void Close()
        {
            _canvasGroup.alpha = 0;
        }

        public void Open()
        {
            _canvasGroup.alpha = 1;
        }
    }
}
