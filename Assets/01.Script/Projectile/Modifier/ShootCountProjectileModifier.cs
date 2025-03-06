using Hashira.Projectiles;
using UnityEngine;

namespace Hashira
{
    public class ShootCountProjectileModifier : ProjectileModifier
    {
        private int _shootCount;
        private int _delay;
        private int _currentDelay;
        private bool _isOn;

        public void Init(int count, int delay)
        {
            _shootCount = count;
            _delay = delay;
            _currentDelay = 0;
        }

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            _isOn = true;
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            if (_currentDelay <= 0 && _isOn)
            {
                _currentDelay = _delay;
                for (int i = 0; i < _shootCount; i++)
                    _attacker.AddShootCount();
            }

            _currentDelay--;
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
            for (int i = 0; i < _shootCount; i++)
                _attacker.RemoveShootCount();
        }
    }
}
