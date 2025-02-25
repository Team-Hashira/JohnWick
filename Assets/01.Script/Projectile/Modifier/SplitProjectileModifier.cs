using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class SplitProjectileModifier : ProjectileModifier
    {
        [SerializeField] private int _addProjectileCount;
        [SerializeField] private float _cooldown;
        private bool _isCanMultyAttack;

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            for (int i = 0; i < _addProjectileCount; i++)
                _attacker.AddBurstBullets();
            _attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            _isCanMultyAttack = false;
        }

        private void HandleProjectileCreateEvent(List<Projectile> list)
        {
            for (int i = 0; i < _addProjectileCount; i++)
                _attacker.RemoveBurstBullets();
            CooldownUtillity.StartCooldown("SplitProjectileModifier");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (CooldownUtillity.CheckCooldown("SplitProjectile", _cooldown))
            {
                _isCanMultyAttack = true;
                for (int i = 0; i < _addProjectileCount; i++)
                    _attacker.AddBurstBullets();
            }
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
            if (CooldownUtillity.CheckCooldown("SplitProjectile", _cooldown))
                for (int i = 0; i < _addProjectileCount; i++)
                _attacker.RemoveBurstBullets();
        }
    }
}
