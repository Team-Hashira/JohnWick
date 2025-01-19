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
            if (InputReader.MousePosition != Vector2.zero)
            {
                rectTransform.anchoredPosition = InputReader.MousePosition;
            }
        }
    }
}
