using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using System;
using TMPro;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class LaserSightParts : WeaponParts
    {
        private static LayerMask _WhatIsObstacle = LayerMask.GetMask("Ground", "Enemy", "Object");

        private LineRenderer _lineRenderer;
        private EntityWeapon _entityWeapon;
        private Transform _weaponVisual;

        public override void Equip(GunWeapon weapon)
        {
            base.Equip(weapon);
            _entityWeapon = weapon.EntityWeapon;
            _weaponVisual = _entityWeapon.VisualTrm;
            _lineRenderer = _entityWeapon.LaserRenderer;

            _entityWeapon.OnCurrentWeaponChanged += HandleCurrentWeaponChangedEvent;
            HandleCurrentWeaponChangedEvent(_entityWeapon.CurrentWeapon);
        }

        private void HandleCurrentWeaponChangedEvent(Weapon weapon)
        {
            bool isOn = weapon == _weapon;
            bool isHaveLaser = weapon != null && weapon is GunWeapon gunWeapon && gunWeapon.GetParts(EWeaponPartsType.Grip)?.WeaponPartsSO == WeaponPartsSO;

            Debug.Log(isOn);
            if (weapon is GunWeapon gunWeapon1)
                Debug.Log(gunWeapon1.GetParts(EWeaponPartsType.Grip)?.WeaponPartsSO.name);

            _lineRenderer.enabled = isOn || isHaveLaser;
        }

        public override void PartsUpdate()
        {
            base.PartsUpdate();
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(_lineRenderer.transform.position, _lineRenderer.transform.right, 100, _WhatIsObstacle))
                _lineRenderer.SetPosition(1, Vector3.right * hit.distance);
            else
                _lineRenderer.SetPosition(1, Vector3.right * 100);
        }

        public override void UnEquip()
        {
            base.UnEquip();
            if (_entityWeapon.CurrentWeapon == _weapon) _lineRenderer.enabled = false;
            _entityWeapon.OnCurrentWeaponChanged -= HandleCurrentWeaponChangedEvent;
        }
    }
}
