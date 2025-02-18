using AYellowpaper.SerializedCollections;
using Hashira.Core;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class PartsRenderer : MonoBehaviour
    {
        private SerializedDictionary<EWeaponPartsType, SpriteRenderer> _partsSpriteDictionary = new();
        [field: SerializeField] public LineRenderer LaserRenderer { get; private set; }

        private GunWeapon _currentWeapon;

        public SpriteRenderer this[EWeaponPartsType eWeaponPartsType]
        {
            get
            {
                if (_partsSpriteDictionary.TryGetValue(eWeaponPartsType, out SpriteRenderer renderer))
                    return renderer;
                else
                    return null;
            }
        }

        public void Init()
        {
            int index = 0;
            foreach (EWeaponPartsType partsType in Enum.GetValues(typeof(EWeaponPartsType)))
            {
                _partsSpriteDictionary.Add(partsType, transform.GetChild(index++).GetComponent<SpriteRenderer>());
            }
        }

        public void SetGun(GunWeapon gun)
        {
            if (_currentWeapon != null)
                _currentWeapon.OnPartsChanged -= HandlePartsChangedEvnet;
            _currentWeapon = gun;
            if (_currentWeapon != null)
                _currentWeapon.OnPartsChanged += HandlePartsChangedEvnet;

            foreach (EWeaponPartsType partsType in Enum.GetValues(typeof(EWeaponPartsType)))
            {
                SpriteRenderer spriteRenderer = _partsSpriteDictionary[partsType];
                spriteRenderer.sprite
                    = gun?.GetParts(partsType)?.WeaponPartsSO.partsSpriteDictionary[gun.GunSO];
                if (gun != null && gun.GunSO.partsEquipPosDict.TryGetValue(partsType, out Vector2Int pos))
                {
                    Vector2 partsPosition = pos;
                    spriteRenderer.transform.localPosition 
                        = partsPosition / Core.EnumUtility.CommonPixelPerUnit;
                }
            }
        }

        private void HandlePartsChangedEvnet(EWeaponPartsType type, WeaponParts parts)
        {
            _partsSpriteDictionary[type].sprite = parts?.WeaponPartsSO.partsSpriteDictionary[_currentWeapon.GunSO];
        }
    }
}
