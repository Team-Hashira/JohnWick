using System;
using System.Collections.Generic;
using Hashira.Entities.Components;
using Hashira.Players;
using Hashira.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.StatusWindow
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private PartSlot _partSlotPrefab; 
        [SerializeField] private Image _iconImage;
        private Weapon _baseWeapon;

        private readonly List<PartSlot> _partSlotList = new List<PartSlot>();
        private Player _player;
        private EntityWeapon _entityWeapon;

        private void Awake()
        {
            _player = GameManager.Instance.Player;
        }

        private void Start()
        {
            _entityWeapon = _player.GetEntityComponent<EntityWeapon>();
            _entityWeapon.OnCurrnetWeaponChanged += HandleWeaponChanged;
        }

        private void OnDestroy()
        {
            _entityWeapon.OnCurrnetWeaponChanged -= HandleWeaponChanged;
        }

        private void HandleWeaponChanged(Weapon weapon)
        {
            _baseWeapon = weapon;
            _iconImage.sprite = weapon.WeaponSO.itemSprite;

            // 기존에 있던 UI 삭제
            _partSlotList.ForEach(partSlot => Destroy(partSlot.gameObject));
            _partSlotList.Clear();
            
            // 새로 추가
            foreach (var posPair in weapon.WeaponSO.partsEquipPosDict)
                AddPartSlot(posPair.Key, posPair.Value);
        }

        private void AddPartSlot(EWeaponPartsType partType, Vector2 position)
        {
            PartSlot partSlot = Instantiate(_partSlotPrefab, transform);
            partSlot.Init(partType);
            _partSlotList.Add(partSlot);
        }
    }
}
