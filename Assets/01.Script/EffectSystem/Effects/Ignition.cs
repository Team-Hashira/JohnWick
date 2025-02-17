using Hashira.Entities;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public class Ignition : Effect
    {
        private int _damage = 20;
        private float _damageDelay = 1f;
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
                entity.GetEntityComponent<EntityHealth>().ApplyDamage(_damage, new RaycastHit2D(), null);
            }
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}
