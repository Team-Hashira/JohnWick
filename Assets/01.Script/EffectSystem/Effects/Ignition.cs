using Hashira.Entities;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class Ignition : Effect
    {
        private readonly int[] _damage = { 20, 30, 50 };
        private float _damageDelay = 0.5f;
        private float _lastDamageTime;

        public override void Enable()
        {
            base.Enable();
            _lastDamageTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (_lastDamageTime + _damageDelay < Time.time)
            {
                _lastDamageTime = Time.time;
                entity.GetEntityComponent<EntityHealth>().ApplyDamage(_damage[level <= 3 ? level - 1 : 2], default, null, attackType: EAttackType.Fire);
            }
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}
