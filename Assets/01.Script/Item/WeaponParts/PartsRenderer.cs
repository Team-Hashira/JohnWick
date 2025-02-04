using AYellowpaper.SerializedCollections;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class PartsRenderer : MonoBehaviour
    {
        [field: SerializeField] public SerializedDictionary<EWeaponPartsType, SpriteRenderer> PartsSpriteDictionary { get; private set; } = new();
        [SerializeField] private int _pixelPerUnit = 16;

        private GunWeapon _currentWeapon;

        public void SetGun(GunWeapon gun)
        {
            if (_currentWeapon != null)
                _currentWeapon.OnPartsChanged -= HandlePartsChangedEvnet;
            _currentWeapon = gun;
            if (_currentWeapon != null)
                _currentWeapon.OnPartsChanged += HandlePartsChangedEvnet;

            foreach (EWeaponPartsType partsType in Enum.GetValues(typeof(EWeaponPartsType)))
            {
                PartsSpriteDictionary[partsType].sprite
                    = gun?.GetParts(partsType)?.WeaponPartsSO.partsSpriteDictionary[gun.GunSO];
                if (gun != null && gun.GunSO.partsEquipPosDict.TryGetValue(partsType, out Vector2Int pos))
                {
                    Vector2 partsPosition = pos;
                    PartsSpriteDictionary[partsType].transform.localPosition = partsPosition / _pixelPerUnit;
                }
            }
        }

        private void HandlePartsChangedEvnet(EWeaponPartsType type, WeaponParts parts)
        {
            PartsSpriteDictionary[type].sprite = parts?.WeaponPartsSO.partsSpriteDictionary[_currentWeapon.GunSO];
        }
    }
}
