using AYellowpaper.SerializedCollections;
using Hashira.Items.Weapons;
using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Hashira.Items.PartsSystem
{
    public class PartsRenderer : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<EWeaponPartsType, SpriteRenderer> _partsSpriteDictionary = new();
        [SerializeField] private int _pixelPerUnit = 16;

        private GunWeapon _currentWeapon;

        public void SetGun(GunWeapon gun)
        {
            Debug.Log("SetGun");
            if (_currentWeapon != null)
                _currentWeapon.OnPartsChanged -= HandlePartsChangedEvnet;
            _currentWeapon = gun;
            if (_currentWeapon != null)
                _currentWeapon.OnPartsChanged += HandlePartsChangedEvnet;

            foreach (EWeaponPartsType partsType in Enum.GetValues(typeof(EWeaponPartsType)))
            {
                _partsSpriteDictionary[partsType].sprite 
                    = gun?.GetParts(partsType)?.WeaponPartsSO.partsSpriteDictionary[gun.GunSO];
                if (gun != null && gun.GunSO.partsEquipPosDict.TryGetValue(partsType, out Vector2Int pos))
                {
                    Vector2 partsPosition = pos;
                    _partsSpriteDictionary[partsType].transform.localPosition = partsPosition / _pixelPerUnit;
                }
            }
        }

        private void HandlePartsChangedEvnet(EWeaponPartsType type, WeaponParts parts)
        {
            _partsSpriteDictionary[type].sprite = parts?.WeaponPartsSO.partsSpriteDictionary[_currentWeapon.GunSO];
        }
    }
}
