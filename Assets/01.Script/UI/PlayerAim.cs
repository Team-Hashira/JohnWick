using UnityEngine;

namespace Hashira
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private InputReaderSO InputReader;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        private void Update()
        {
            rectTransform.anchoredPosition = InputReader.MousePosition;
        }
    }
}
