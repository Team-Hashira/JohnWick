using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Players;
using Hashira.Items.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
namespace Hashira.UI
{
    public class ProfileContainer : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private Image _hpSlider;
        [SerializeField] private TextMeshProUGUI _hpText;

        [Header("Weapon")]
        [SerializeField] private Image _weaponSwapCoolTimeContainer;
        [SerializeField] private ReloadContainer _reloadContainer; 
        [SerializeField] private Image _weaponIconImage;
        [SerializeField] private Slider _weaponLoadSlider;
        [SerializeField] private TextMeshProUGUI _weaponLoadText;
        [SerializeField] private TextMeshProUGUI _weaponSlotNumberText;

        private Player _player;
        private EntityHealth _playerHealth;
        private EntityWeaponHolder _entityGunWeapon;
            
        private void Awake()
        {
            _player = GameManager.Instance.Player;
            _weaponLoadText.text = "-";
            _weaponLoadSlider.value = 1;
            _playerHealth = _player.GetEntityComponent<EntityHealth>();
            _entityGunWeapon = _player.GetEntityComponent<EntityWeaponHolder>();
            
            _playerHealth.OnHealthChangedEvent += HandleHpChange;
            _entityGunWeapon.OnCurrentWeaponChanged += HandleWeaponChange;
            _entityGunWeapon.OnReloadEvent += _reloadContainer.HandleReload;
        }

        private void OnDestroy()
        {
            _playerHealth.OnHealthChangedEvent -= HandleHpChange;
            _entityGunWeapon.OnCurrentWeaponChanged -= HandleWeaponChange;
            _entityGunWeapon.OnReloadEvent -= _reloadContainer.HandleReload;
        }

        private void HandleHpChange(int lastValue, int newValue)
        {
            _hpSlider.fillAmount = newValue/(float)_playerHealth.MaxHealth;
            _hpText.text = $"{newValue}/{_playerHealth.MaxHealth}";
        }

        private void Update()
        {
            if (_entityGunWeapon.CurrentItem != null)
            {
                GunWeapon gunWeapon = _entityGunWeapon.CurrentItem as GunWeapon;
                HandleUseWeapon(gunWeapon.BulletAmount, gunWeapon.StatDictionary["MagazineCapacity"].IntValue);

                // Swap CoolTime
                if (gunWeapon.CanSwap == false)
                    _weaponSwapCoolTimeContainer.fillAmount = gunWeapon.currentCoolTime / gunWeapon.WeaponSO.SwapCoolTime;
                else
                    _weaponSwapCoolTimeContainer.fillAmount = 1;
            }
            else
            {
                HandleUseWeapon(-1, -1);
            }
        }

        private void HandleWeaponChange(Weapon weapon)
        {
            _weaponSlotNumberText.text = (_entityGunWeapon.CurrentIndex+1).ToString();

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
