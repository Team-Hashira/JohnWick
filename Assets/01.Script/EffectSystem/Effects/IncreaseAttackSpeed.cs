using Hashira.Core.StatSystem;
using Hashira.EffectSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira
{
    public class IncreaseAttackSpeed : Effect, ICoolTimeEffect
    {

        public override int MaxActiveCount => 4;

        public float Duration { get; set; }
        public float Time { get; set; }

        private Entity _entity;
        private EntityGunWeapon _entityGunWeapon;
        private StatElement _attackSpeedStat;

        public override void Enable()
        {
            base.Enable();
            _entity = entityEffector.Entity;
            _entityGunWeapon = _entity.GetEntityComponent<EntityGunWeapon>();
            _entityGunWeapon.OnCurrentWeaponChanged += HandleCurrentWeaponChanged;
            HandleCurrentWeaponChanged(_entityGunWeapon.CurrentWeapon);
            //TODO 여기에 이펙트
        }

        private void HandleCurrentWeaponChanged(Weapon weapon)
        {
            _attackSpeedStat?.RemoveModify("IncreaseAttackSpeed", EModifyMode.Percnet);
            _attackSpeedStat = weapon?.StatDictionary["AttackSpeed"];
            _attackSpeedStat?.AddModify("IncreaseAttackSpeed", 30.0f, EModifyMode.Percnet);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Disable()
        {
            base.Disable();
            _entityGunWeapon.OnCurrentWeaponChanged += HandleCurrentWeaponChanged;
            _attackSpeedStat?.RemoveModify("IncreaseAttackSpeed", EModifyMode.Percnet);
            //TODO 여기에 이펙트
        }

        public void OnTimeOut()
        {

        }
    }
}
