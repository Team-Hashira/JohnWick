using Hashira.Weapons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityWeaponHolder : MonoBehaviour, IEntityComponent
    {
        private Dictionary<WeaponSO, Weapon> _weaponDictionary;
        private int _weaponIndex;
        public Weapon CurrentWeapon { get; private set; }
        private WeaponSO[] _weapons = new WeaponSO[3] { null, null, null };

        public void Initialize(Entity entity)
        {
            _weaponDictionary = new Dictionary<WeaponSO, Weapon>();
            Weapon[] waepons = transform.GetComponentsInChildren<Weapon>();
            waepons.ToList().ForEach(weapon =>
            {
                weapon.gameObject.SetActive(false);
                _weaponDictionary.Add(weapon.WeaponSO, weapon); 
            });


            _weaponIndex = -1;

            WeaponSawp();
        }

        public WeaponSO EquipWeapon(WeaponSO weaponSO)
        {
            if (CurrentWeapon != null)
                CurrentWeapon.gameObject.SetActive(false);

            WeaponSO prevWeapon = _weapons[_weaponIndex];
            _weapons[_weaponIndex] = weaponSO;

            if (weaponSO != null)
            {
                CurrentWeapon = _weaponDictionary[weaponSO];
                CurrentWeapon.gameObject.SetActive(true);
            }

            return prevWeapon;
        }

        public void WeaponSawp()
        {
            if (CurrentWeapon != null)
                CurrentWeapon.gameObject.SetActive(false);

            _weaponIndex++;
            if (_weaponIndex >= 3) _weaponIndex = 0;

            if (_weapons[_weaponIndex] != null)
            {
                CurrentWeapon = _weaponDictionary[_weapons[_weaponIndex]];
                CurrentWeapon.gameObject.SetActive(true);
            }
        }
    }
}
