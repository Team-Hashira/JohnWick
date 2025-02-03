using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Players;
using Hashira.Items.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class ProfileContainer : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TextMeshProUGUI _hpText;

        [Header("Weapon")]
        [SerializeField] private ReloadContainer _reloadContainer; 
        [SerializeField] private Image _weaponIconImage;
        [SerializeField] private Slider _weaponLoadSlider;
        [SerializeField] private TextMeshProUGUI _weaponLoadText;

        private Player _player;
        private EntityHealth _playerHealth;
        private EntityWeapon _entityWeapon;
            
        private void Awake()
        {
            _player = GameManager.Instance.Player;
            _weaponLoadText.text = "-";
            _weaponLoadSlider.value = 1;
            _playerHealth = _player.GetEntityComponent<EntityHealth>();
            _entityWeapon = _player.GetEntityComponent<EntityWeapon>();
            
            _playerHealth.OnHealthChangedEvent += HandleHpChange;
            _entityWeapon.OnCurrentWeaponChanged += HandleWeaponChange;
            _entityWeapon.OnReloadEvent += _reloadContainer.HandleReload;
        }

        private void OnDestroy()
        {
            _playerHealth.OnHealthChangedEvent -= HandleHpChange;
            _entityWeapon.OnCurrentWeaponChanged -= HandleWeaponChange;
            _entityWeapon.OnReloadEvent -= _reloadContainer.HandleReload;
        }

        private void HandleHpChange(int lastValue, int newValue)
        {
            _hpSlider.value = (float)_playerHealth.MaxHealth / newValue;
            _hpText.text = $"{newValue}/{_playerHealth.MaxHealth}";
        }

        private void Update()
        {
            if (_entityWeapon.CurrentWeapon is GunWeapon gunWeapon)
            {
                HandleUseWeapon(gunWeapon.BulletAmount, gunWeapon.StatDictionary["MagazineCapacity"].IntValue);
            }
            else if (_entityWeapon.CurrentWeapon is MeleeWeapon meleeWeapon)
            {
                HandleUseWeapon(-1, -1);
            }
        }

        private void HandleWeaponChange(Weapon weapon)
        {
            if (weapon == null)
            {
                _weaponIconImage.sprite = null;
                _weaponIconImage.color = Color.clear;
            }
            else
            {
                _weaponIconImage.sprite = weapon.WeaponSO.itemDefaultSprite;
                _weaponIconImage.color = Color.white;
            }
        }
        
        //TODO 무기 사용할 때마다 장전 상태 가져오기
        private void HandleUseWeapon(int amount, int maxAmount)
        {
            if (maxAmount < 0 || amount < 0)
            {
                _weaponLoadText.text = "-";
                _weaponLoadSlider.value = 1;
                return;
            }
            
            _weaponLoadText.text = $"{amount}/{maxAmount}";
            _weaponLoadSlider.value = (float)amount/maxAmount;
        }
    }
}
