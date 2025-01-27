using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private Image _circleAim;
        [SerializeField] private RectTransform _crossAim;
        [SerializeField] private InputReaderSO _inputReader;
        private Image[] _crossAims = new Image[4];
        private Vector2[] _crossAimStartPositions = new Vector2[4];
        private float _startCircleSize;

        private RectTransform rectTransform;
        private EntityWeapon _playerWeapon;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
            _startCircleSize = _circleAim.rectTransform.sizeDelta.x;

            for (int i = 0; i < 4; i++)
            {
                _crossAims[i] = _crossAim.GetChild(i).GetComponent<Image>();
                _crossAimStartPositions[i] = _crossAims[i].rectTransform.anchoredPosition;
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
            if (_playerWeapon.CurrentWeapon is GunWeapon gun)
                SetColor(gun.IsCanFire && gun.BulletAmount > 0 ? Color.white : Color.red);
            else
                SetColor(Color.white);
        }

        public void SetSize(float size)
        {
            _circleAim.rectTransform.sizeDelta = Vector2.one * (size * _startCircleSize);
            for (int i = 0; i < 4; i++)
            {
                _crossAims[i].rectTransform.anchoredPosition = _crossAimStartPositions[i] * size;
            }
        }

        public void SetColor(Color color)
        {
            _circleAim.color = color;
            for (int i = 0; i < 4; i++)
            {
                _crossAims[i].color = color;
            }
        }
    }
}
