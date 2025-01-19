using Hashira.Entities.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private RectTransform _circleAim;
        [SerializeField] private RectTransform _crossAim;
        [SerializeField] private InputReaderSO _inputReader;
        private RectTransform[] _crossAims = new RectTransform[4];
        private Vector2[] _crossAimStartPositions = new Vector2[4];
        private float _startCircleSize;

        private RectTransform rectTransform;
        private EntityWeapon _playerWeapon;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
            _startCircleSize = _circleAim.sizeDelta.x;

            for (int i = 0; i < 4; i++)
            {
                _crossAims[i] = _crossAim.GetChild(i) as RectTransform;
                _crossAimStartPositions[i] = _crossAims[i].anchoredPosition;
            }
        }

        private void Start()
        {
            _playerWeapon = GameManager.Instance.Player.GetEntityComponent<EntityWeapon>();
        }

        private void Update()
        {
            if (_inputReader.MousePosition != Vector2.zero)
            {
                rectTransform.anchoredPosition = _inputReader.MousePosition;
            }

            SetSize(_playerWeapon.Recoil * 0.2f + 1);
        }

        public void SetSize(float size)
        {
            _circleAim.sizeDelta = Vector2.one * (size * _startCircleSize);
            for (int i = 0; i < 4; i++)
            {
                _crossAims[i].anchoredPosition = _crossAimStartPositions[i] * size;
            }
        }
    }
}
