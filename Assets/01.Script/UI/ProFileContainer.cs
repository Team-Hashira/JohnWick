using System;
using Hashira.Entities;
using Hashira.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class ProFileContainer : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TextMeshProUGUI _hpText;

        [Header("Weapon")]
        [SerializeField] private Image _weaponImage;
        [SerializeField] private Slider _weaponLoadSlider;
        [SerializeField] private TextMeshProUGUI _weaponLoadText;

        private Player _player;
        private EntityHealth _playerHealth;

        private void Awake()
        {
            _player = GameManager.Instance.Player;
        }

        private void Start()
        {
            _playerHealth = _player.GetEntityComponent<EntityHealth>();
            _playerHealth.OnHealthChangedEvent += HandleHpChange;
        }

        private void OnDestroy()
        {
            _playerHealth.OnHealthChangedEvent -= HandleHpChange;
        }

        private void HandleHpChange(int lastValue, int newValue)
        {
            _hpSlider.value = _playerHealth.MaxHealth / newValue;
            _hpText.text = $"{newValue}/{_playerHealth.MaxHealth}";
        }
        
        //TODO 현재 무기의 이미정보 가져오기
        private void HandleWeaponChange()
        {
            
        }
        
        //TODO 무기 사용할 때마다 장전 상태 가져오기
        private void HandleUseWeapon()
        {
            _weaponLoadText.text = "260/360";
        }
    }
}
