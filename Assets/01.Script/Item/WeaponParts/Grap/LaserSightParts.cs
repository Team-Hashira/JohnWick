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
        private Vector3 _laserOffset;

        public override void Equip(GunWeapon weapon)
        {
            base.Equip(weapon);
            _entityWeapon = weapon.EntityWeapon;
            _weaponVisual = _entityWeapon.VisualTrm;
            _lineRenderer = _entityWeapon.LaserRenderer;

            _laserOffset = weapon.GunSO.partsEquipUIPosDict[EWeaponPartsType.Grip] / (1080 / 10 * 2); // (y해상도) / (카메라 y길이)

            _entityWeapon.OnCurrentWeaponChanged += HandleCurrentWeaponChangedEvent;
            HandleCurrentWeaponChangedEvent(_entityWeapon.CurrentWeapon);
        }

        private void HandleCurrentWeaponChangedEvent(Weapon weapon)
        {
            bool isOn = weapon == _weapon;
            bool isHaveLaser = weapon != null && weapon is GunWeapon gunWeapon && gunWeapon.GetParts(EWeaponPartsType.Grip)?.WeaponPartsSO == WeaponPartsSO;

            _lineRenderer.enabled = isOn || isHaveLaser;
            if (isOn)
                _lineRenderer.SetPosition(0, _laserOffset);
        }

        public override void PartsUpdate()
        {
            base.PartsUpdate();
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(_weaponVisual.position + _weaponVisual.rotation * _laserOffset, _weaponVisual.right, 100, _WhatIsObstacle))
                _lineRenderer.SetPosition(1, Vector3.right * hit.distance);
            else
                _lineRenderer.SetPosition(1, Vector3.right * 100);
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _entityWeapon.OnCurrentWeaponChanged -= HandleCurrentWeaponChangedEvent;
        }
    }
}
