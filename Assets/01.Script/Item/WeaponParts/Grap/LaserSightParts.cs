using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class LaserSightParts : WeaponParts
    {
        private static LayerMask _WhatIsObstacle = LayerMask.GetMask("Ground", "Enemy", "Object");

        private LineRenderer _lineRenderer;
        private EntityGunWeapon _entityWeapon;
        private Transform _weaponVisual;

        public override void Equip(GunWeapon weapon)
        {
            base.Equip(weapon);
            _entityWeapon = weapon.EntityGunWeapon;
            _weaponVisual = _entityWeapon.VisualTrm;

            Sprite sprite = WeaponPartsSO.partsSpriteDictionary[weapon.GunSO];
            int pixelPerUnit = PartsRenderer.PixelPerUnit;
            _lineRenderer.transform.localPosition 
                = new Vector3((sprite.rect.width - sprite.pivot.x) / pixelPerUnit, -sprite.rect.height / pixelPerUnit / 2);

            _entityWeapon.OnCurrentWeaponChanged += HandleCurrentWeaponChangedEvent;
            HandleCurrentWeaponChangedEvent(_entityWeapon.CurrentWeapon);
        }

        protected override void HandlePartsRendererChangedEvent(PartsRenderer renderer)
        {
            base.HandlePartsRendererChangedEvent(renderer);

            _lineRenderer = renderer.LaserRenderer;
        }

        private void HandleCurrentWeaponChangedEvent(Weapon weapon)
        {
            bool isOn = weapon == _weapon;
            bool isHaveLaser 
                = weapon != null && weapon is GunWeapon gunWeapon 
                && gunWeapon.GetParts(EWeaponPartsType.Grip)?.WeaponPartsSO == WeaponPartsSO;

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
