using Hashira.Projectiles;
using UnityEngine;

namespace Hashira
{
    public class ShootCountProjectileModifier : ProjectileModifier
    {
        private int _shootCount;
        private int _delay;
        private int _currentDelay;
        private bool _isEnable;

        public void Init(int count, int delay)
        {
            _shootCount = count;
            _delay = delay;
            _currentDelay = 0;
        }

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            _isEnable = true;
            for (int i = 0; i < _shootCount; i++)
                _attacker.AddShootCount();
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            if (_isEnable)
            {
                _isEnable = false;
                for (int i = 0; i < _shootCount; i++)
                    _attacker.RemoveShootCount();
            }

            _currentDelay++;
            if (_currentDelay > _delay)
            {
                _isEnable = true;
                _currentDelay = 0;
                for (int i = 0; i < _shootCount; i++)
                    _attacker.AddShootCount();
            }
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
            if (_isEnable)
            {
                for (int i = 0; i < _shootCount; i++)
                    _attacker.RemoveShootCount();
                _currentDelay = _delay;
            }
        }
    }
}
